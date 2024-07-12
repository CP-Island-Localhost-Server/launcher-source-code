using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using JsonFx.Json;
using UnityEngine;

namespace Disney.MobileNetwork
{
	[ExecuteInEditMode]
	public class Kochava : MonoBehaviour
	{
		public delegate void AttributionCallback(string callbackString);

		public enum KochSessionTracking
		{
			full = 0,
			basic = 1,
			minimal = 2,
			none = 3
		}

		public enum KochLogLevel
		{
			error = 0,
			warning = 1,
			debug = 2
		}

		public class LogEvent
		{
			public string text;

			public float time;

			public KochLogLevel level;

			public LogEvent(string text, KochLogLevel level)
			{
				this.text = text;
				time = Time.time;
				this.level = level;
			}
		}

		public class QueuedEvent
		{
			public float eventTime;

			public Dictionary<string, object> eventData;
		}

		private const string CURRENCY_DEFAULT = "USD";

		public const string KOCHAVA_VERSION = "20160914";

		public const string KOCHAVA_PROTOCOL_VERSION = "4";

		private const int MAX_LOG_SIZE = 50;

		private const int MAX_QUEUE_SIZE = 75;

		private const int MAX_POST_TIME = 15;

		private const int POST_FAIL_RETRY_DELAY = 30;

		private const int QUEUE_KVINIT_WAIT_DELAY = 15;

		private const string API_URL = "https://control.kochava.com";

		private const string TRACKING_URL = "https://control.kochava.com/track/kvTracker?v4";

		private const string INIT_URL = "https://control.kochava.com/track/kvinit";

		private const string QUERY_URL = "https://control.kochava.com/track/kvquery";

		private const string KOCHAVA_QUEUE_STORAGE_KEY = "kochava_queue_storage";

		private const int KOCHAVA_ATTRIBUTION_INITIAL_TIMER = 7;

		private const int KOCHAVA_ATTRIBUTION_DEFAULT_TIMER = 60;

		public string kochavaAppId = "";

		public string kochavaAppIdIOS = "";

		public string kochavaAppIdAndroid = "";

		public string kochavaAppIdKindle = "";

		public string kochavaAppIdBlackberry = "";

		public string kochavaAppIdWindowsPhone = "";

		public bool debugMode = false;

		public bool incognitoMode = false;

		public bool requestAttribution = false;

		private bool retrieveAttribution = false;

		private bool debugServer = false;

		[HideInInspector]
		public string appVersion = "";

		[HideInInspector]
		public string partnerName = "";

		public bool appLimitAdTracking = false;

		[HideInInspector]
		public string userAgent = "";

		public bool adidSupressed = false;

		private static int device_id_delay = 60;

		private string whitelist = null;

		private static bool adidBlacklisted = false;

		private static AttributionCallback attributionCallback;

		[HideInInspector]
		public string appIdentifier = "";

		private string appPlatform = "desktop";

		private string kochavaDeviceId = "";

		private string attributionDataStr = "";

		private List<string> devIdBlacklist = new List<string>();

		private List<string> eventNameBlacklist = new List<string>();

		public string appCurrency = "USD";

		public KochSessionTracking sessionTracking = KochSessionTracking.minimal;

		private int KVTRACKER_WAIT = 60;

		private List<LogEvent> _EventLog = new List<LogEvent>();

		private Dictionary<string, object> hardwareIdentifierData = new Dictionary<string, object>();

		private Dictionary<string, object> hardwareIntegrationData = new Dictionary<string, object>();

		private Dictionary<string, object> appData;

		private Queue<QueuedEvent> eventQueue = new Queue<QueuedEvent>();

		private float processQueueKickstartTime = 0f;

		private bool queueIsProcessing = false;

		private float _eventPostingTime = 0f;

		private bool doReportLocation = false;

		private int locationAccuracy = 50;

		private int locationTimeout = 15;

		private int locationStaleness = 15;

		private int iAdAttributionAttempts = 3;

		private int iAdAttributionWait = 20;

		private int iAdRetryWait = 10;

		private bool send_id_updates = false;

		private static Kochava _S;

		private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static float uptimeDelta;

		private static float uptimeDeltaUpdate;

		public static bool DebugMode
		{
			get
			{
				return _S.debugMode;
			}
			set
			{
				_S.debugMode = value;
			}
		}

		public static bool IncognitoMode
		{
			get
			{
				return _S.incognitoMode;
			}
			set
			{
				_S.incognitoMode = value;
			}
		}

		public static bool RequestAttribution
		{
			get
			{
				return _S.requestAttribution;
			}
			set
			{
				_S.requestAttribution = value;
			}
		}

		public static bool AppLimitAdTracking
		{
			get
			{
				return _S.appLimitAdTracking;
			}
			set
			{
				_S.appLimitAdTracking = value;
			}
		}

		public static bool AdidSupressed
		{
			get
			{
				return _S.adidSupressed;
			}
			set
			{
				_S.adidSupressed = value;
			}
		}

		public static string AttributionDataStr
		{
			get
			{
				return _S.attributionDataStr;
			}
			set
			{
				_S.attributionDataStr = value;
			}
		}

		public static List<string> DevIdBlacklist
		{
			get
			{
				return _S.devIdBlacklist;
			}
			set
			{
				_S.devIdBlacklist = value;
			}
		}

		public static List<string> EventNameBlacklist
		{
			get
			{
				return _S.eventNameBlacklist;
			}
			set
			{
				_S.eventNameBlacklist = value;
			}
		}

		public static KochSessionTracking SessionTracking
		{
			get
			{
				return _S.sessionTracking;
			}
			set
			{
				_S.sessionTracking = value;
			}
		}

		public static List<LogEvent> EventLog
		{
			get
			{
				return _S._EventLog;
			}
		}

		public static int eventQueueLength
		{
			get
			{
				return _S.eventQueue.Count;
			}
		}

		public static float eventPostingTime
		{
			get
			{
				return _S._eventPostingTime;
			}
		}

		public static void SetAttributionCallback(AttributionCallback callback)
		{
			attributionCallback = callback;
		}

		public void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if ((bool)_S)
			{
				Log("detected two concurrent integration objects - please place your integration object in a scene which will not be reloaded.");
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (_S == null)
			{
				_S = this;
			}
			base.gameObject.name = "_Kochava Analytics";
			Log("Kochava SDK Initialized.\nVersion: 20160914\nProtocol Version: 4", KochLogLevel.debug);
		}

		public void OnEnable()
		{
			if (Application.isPlaying && _S == null)
			{
				_S = this;
			}
		}

		public void Init()
		{
			if (kochavaAppId.Length == 0 && kochavaAppIdIOS.Length == 0 && kochavaAppIdAndroid.Length == 0 && kochavaAppIdKindle.Length == 0 && kochavaAppIdBlackberry.Length == 0 && kochavaAppIdWindowsPhone.Length == 0 && partnerName.Length == 0)
			{
				Log("No Kochava App Id or Partner Name - SDK will terminate");
				UnityEngine.Object.Destroy(base.gameObject);
			}
			try
			{
				if (hardwareIdentifierData.ContainsKey("package"))
				{
					appIdentifier = hardwareIdentifierData["package"].ToString();
					Log("appIdentifier set to: " + appIdentifier, KochLogLevel.debug);
				}
				if (PlayerPrefs.HasKey("kochava_app_id"))
				{
					kochavaAppId = PlayerPrefs.GetString("kochava_app_id");
					Log("Loaded kochava_app_id from persistent storage: " + kochavaAppId, KochLogLevel.debug);
				}
				if (PlayerPrefs.HasKey("kochava_device_id"))
				{
					kochavaDeviceId = PlayerPrefs.GetString("kochava_device_id");
					Log("Loaded kochava_device_id from persistent storage: " + kochavaDeviceId, KochLogLevel.debug);
				}
				else if (incognitoMode)
				{
					kochavaDeviceId = "KA" + Guid.NewGuid().ToString().Replace("-", "");
					Log("Using autogenerated \"incognito\" kochava_device_id: " + kochavaDeviceId, KochLogLevel.debug);
				}
				else
				{
					string text = "";
					if (PlayerPrefs.HasKey("data_orig_kochava_device_id"))
					{
						text = PlayerPrefs.GetString("data_orig_kochava_device_id");
					}
					if (text != "")
					{
						kochavaDeviceId = text;
						Log("Using \"orig\" kochava_device_id: " + kochavaDeviceId, KochLogLevel.debug);
					}
					else
					{
						kochavaDeviceId = "KU" + SystemInfo.deviceUniqueIdentifier.Replace("-", "");
						Log("Using autogenerated kochava_device_id: " + kochavaDeviceId, KochLogLevel.debug);
					}
				}
				if (!PlayerPrefs.HasKey("data_orig_kochava_app_id") && kochavaAppId != "")
				{
					PlayerPrefs.SetString("data_orig_kochava_app_id", kochavaAppId);
				}
				if (!PlayerPrefs.HasKey("data_orig_kochava_device_id") && kochavaDeviceId != "")
				{
					PlayerPrefs.SetString("data_orig_kochava_device_id", kochavaDeviceId);
				}
				if (!PlayerPrefs.HasKey("data_orig_session_tracking"))
				{
					PlayerPrefs.SetString("data_orig_session_tracking", sessionTracking.ToString());
				}
				if (!PlayerPrefs.HasKey("data_orig_currency") && appCurrency != "")
				{
					PlayerPrefs.SetString("data_orig_currency", appCurrency);
				}
				if (PlayerPrefs.HasKey("currency"))
				{
					appCurrency = PlayerPrefs.GetString("currency");
					Log("Loaded currency from persistent storage: " + appCurrency, KochLogLevel.debug);
				}
				if (PlayerPrefs.HasKey("blacklist"))
				{
					try
					{
						string @string = PlayerPrefs.GetString("blacklist");
						devIdBlacklist = new List<string>();
						string[] array = JsonReader.Deserialize<string[]>(@string);
						for (int num = array.Length - 1; num >= 0; num--)
						{
							devIdBlacklist.Add(array[num]);
						}
						Log("Loaded device_id blacklist from persistent storage: " + @string, KochLogLevel.debug);
					}
					catch (Exception ex)
					{
						Log("Failed loading device_id blacklist from persistent storage: " + ex, KochLogLevel.warning);
					}
				}
				if (PlayerPrefs.HasKey("attribution"))
				{
					try
					{
						attributionDataStr = PlayerPrefs.GetString("attribution");
						Log("Loaded attribution data from persistent storage: " + attributionDataStr, KochLogLevel.debug);
					}
					catch (Exception ex)
					{
						Log("Failed loading attribution data from persistent storage: " + ex, KochLogLevel.warning);
					}
				}
				if (PlayerPrefs.HasKey("session_tracking"))
				{
					try
					{
						string string2 = PlayerPrefs.GetString("session_tracking");
						sessionTracking = (KochSessionTracking)Enum.Parse(typeof(KochSessionTracking), string2, true);
						Log("Loaded session tracking mode from persistent storage: " + string2, KochLogLevel.debug);
					}
					catch (Exception ex)
					{
						Log("Failed loading session tracking mode from persistent storage: " + ex, KochLogLevel.warning);
					}
				}
				if (!PlayerPrefs.HasKey("kvinit_wait"))
				{
					PlayerPrefs.SetString("kvinit_wait", "60");
				}
				if (!PlayerPrefs.HasKey("kvinit_last_sent"))
				{
					PlayerPrefs.SetString("kvinit_last_sent", "0");
				}
				if (!PlayerPrefs.HasKey("kvtracker_wait"))
				{
					PlayerPrefs.SetString("kvtracker_wait", "60");
				}
				if (!PlayerPrefs.HasKey("last_location_time"))
				{
					PlayerPrefs.SetString("last_location_time", "0");
				}
				double num2 = double.Parse(PlayerPrefs.GetString("kvinit_last_sent"));
				double num3 = CurrentTime();
				double num4 = double.Parse(PlayerPrefs.GetString("kvinit_wait"));
				if (num3 - num2 > num4)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("partner_name", partnerName);
					dictionary.Add("package", appIdentifier);
					dictionary.Add("platform", appPlatform);
					dictionary.Add("session_tracking", sessionTracking.ToString());
					dictionary.Add("currency", (appCurrency == null || appCurrency == "") ? "USD" : appCurrency);
					dictionary.Add("os_version", SystemInfo.operatingSystem);
					Dictionary<string, object> dictionary2 = dictionary;
					if (requestAttribution && !PlayerPrefs.HasKey("attribution"))
					{
						retrieveAttribution = true;
					}
					Log("retrieve attrib: " + retrieveAttribution);
					if (hardwareIdentifierData.ContainsKey("IDFA"))
					{
						dictionary2.Add("idfa", hardwareIdentifierData["IDFA"]);
					}
					if (hardwareIdentifierData.ContainsKey("IDFV"))
					{
						dictionary2.Add("idfv", hardwareIdentifierData["IDFV"]);
					}
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3.Add("kochava_app_id", PlayerPrefs.GetString("data_orig_kochava_app_id"));
					dictionary3.Add("kochava_device_id", PlayerPrefs.GetString("data_orig_kochava_device_id"));
					dictionary3.Add("session_tracking", PlayerPrefs.GetString("data_orig_session_tracking"));
					dictionary3.Add("currency", PlayerPrefs.GetString("data_orig_currency"));
					Dictionary<string, object> value = dictionary3;
					Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
					dictionary4.Add("action", "init");
					dictionary4.Add("data", dictionary2);
					dictionary4.Add("data_orig", value);
					dictionary4.Add("kochava_app_id", kochavaAppId);
					dictionary4.Add("kochava_device_id", kochavaDeviceId);
					dictionary4.Add("sdk_version", "Unity3D-20160914");
					dictionary4.Add("sdk_protocol", "4");
					Dictionary<string, object> value2 = dictionary4;
					StartCoroutine(Init_KV(JsonWriter.Serialize(value2)));
					return;
				}
				appData = new Dictionary<string, object>
				{
					{ "kochava_app_id", kochavaAppId },
					{ "kochava_device_id", kochavaDeviceId },
					{ "sdk_version", "Unity3D-20160914" },
					{ "sdk_protocol", "4" }
				};
				if (PlayerPrefs.HasKey("eventname_blacklist"))
				{
					string[] array2 = JsonReader.Deserialize<string[]>(PlayerPrefs.GetString("eventname_blacklist"));
					List<string> list = new List<string>();
					for (int num = 0; num < array2.Length; num++)
					{
						list.Add(array2[num]);
					}
					eventNameBlacklist = list;
				}
			}
			catch (Exception ex)
			{
				Log("Overall failure in init: " + ex, KochLogLevel.warning);
			}
		}

		private IEnumerator Init_KV(string postData)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				Log("internet not reachable", KochLogLevel.warning);
				yield return new WaitForSeconds(30f);
				StartCoroutine(Init_KV(postData));
				yield break;
			}
			Log("Initiating kvinit handshake...", KochLogLevel.debug);
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/json" },
				{ "User-Agent", userAgent }
			};
			Log(postData, KochLogLevel.debug);
			float wwwLoadTime = Time.time;
			WWW www = new WWW("https://control.kochava.com/track/kvinit", Encoding.UTF8.GetBytes(postData), headers);
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				Log("Kvinit handshake failed: " + www.error + ", seconds: " + (Time.time - wwwLoadTime) + ")", KochLogLevel.warning);
				yield return new WaitForSeconds(30f);
				StartCoroutine(Init_KV(postData));
				yield break;
			}
			Dictionary<string, object> serverResponse = new Dictionary<string, object>();
			if (www.text != "")
			{
				try
				{
					serverResponse = JsonReader.Deserialize<Dictionary<string, object>>(www.text);
				}
				catch (Exception ex)
				{
					Log("Failed Deserialize JSON response to kvinit: " + ex, KochLogLevel.warning);
				}
			}
			Log(www.text, KochLogLevel.debug);
			if (!serverResponse.ContainsKey("success"))
			{
				Log("Kvinit handshake parsing failed: " + www.text, KochLogLevel.warning);
				yield return new WaitForSeconds(30f);
				StartCoroutine(Init_KV(postData));
				yield break;
			}
			PlayerPrefs.SetString("kvinit_last_sent", CurrentTime().ToString());
			Log("...kvinit handshake complete, processing response flags...", KochLogLevel.debug);
			if (serverResponse.ContainsKey("flags"))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)serverResponse["flags"];
				if (dictionary.ContainsKey("kochava_app_id"))
				{
					kochavaAppId = dictionary["kochava_app_id"].ToString();
					PlayerPrefs.SetString("kochava_app_id", kochavaAppId);
					Log("Saved kochava_app_id to persistent storage: " + kochavaAppId, KochLogLevel.debug);
				}
				if (dictionary.ContainsKey("kochava_device_id"))
				{
					kochavaDeviceId = dictionary["kochava_device_id"].ToString();
				}
				if (dictionary.ContainsKey("resend_initial") && (bool)dictionary["resend_initial"])
				{
					PlayerPrefs.DeleteKey("watchlistProperties");
					Log("Refiring initial event, as requested by kvinit response flag", KochLogLevel.debug);
				}
				if (dictionary.ContainsKey("session_tracking"))
				{
					try
					{
						sessionTracking = (KochSessionTracking)Enum.Parse(typeof(KochSessionTracking), dictionary["session_tracking"].ToString());
						PlayerPrefs.SetString("session_tracking", sessionTracking.ToString());
						Log("Saved session_tracking mode to persistent storage: " + sessionTracking, KochLogLevel.debug);
					}
					catch (Exception ex)
					{
						Log("Failed System.Enum.Parse of KochSessionTracking: " + ex, KochLogLevel.warning);
					}
				}
				if (dictionary.ContainsKey("currency"))
				{
					appCurrency = dictionary["currency"].ToString();
					if (appCurrency.Equals(""))
					{
						appCurrency = "USD";
					}
					PlayerPrefs.SetString("currency", appCurrency);
					Log("Saved currency to persistent storage: " + appCurrency, KochLogLevel.debug);
				}
				if (dictionary.ContainsKey("getattribution_wait"))
				{
					string s = dictionary["getattribution_wait"].ToString();
					int num = int.Parse(s);
					if (num < 1)
					{
						num = 1;
					}
					if (num > 30)
					{
						num = 30;
					}
					PlayerPrefs.SetString("getattribution_wait", num.ToString());
					Log("Saved getattribution_wait to persistent storage: " + num, KochLogLevel.debug);
				}
				if (dictionary.ContainsKey("delay_for_referrer_data"))
				{
					device_id_delay = (int)dictionary["delay_for_referrer_data"];
					Log("delay_for_referrer_data received: " + device_id_delay, KochLogLevel.debug);
					if (device_id_delay < 0)
					{
						Log("device_id_delay returned was less than 0 (" + device_id_delay + "), setting device_id_delay to 0.");
						device_id_delay = 0;
					}
					else if (device_id_delay > 120)
					{
						Log("device_id_delay returned was greater than 120 (" + device_id_delay + "), setting device_id_delay to 120.");
						device_id_delay = 120;
					}
					else
					{
						Log("setting device_id_delay to: " + device_id_delay);
					}
				}
				if (dictionary.ContainsKey("kvinit_wait"))
				{
					string s2 = dictionary["kvinit_wait"].ToString();
					int num2 = int.Parse(s2);
					if (num2 < 60)
					{
						num2 = 60;
					}
					if (num2 > 604800)
					{
						num2 = 604800;
					}
					PlayerPrefs.SetString("kvinit_wait", num2.ToString());
					Log("Saved kvinit_wait to persistent storage: " + num2, KochLogLevel.debug);
				}
				else
				{
					PlayerPrefs.SetString("kvinit_wait", "60");
					Log("Saved kvinit_wait to persistent storage: 60", KochLogLevel.debug);
				}
				if (dictionary.ContainsKey("kvtracker_wait"))
				{
					string s3 = dictionary["kvtracker_wait"].ToString();
					int num3 = int.Parse(s3);
					if (num3 < 60)
					{
						num3 = 60;
					}
					if (num3 > 604800)
					{
						num3 = 604800;
					}
					PlayerPrefs.SetString("kvtracker_wait", num3.ToString());
					Log("Saved kvtracker_wait to persistent storage: " + num3, KochLogLevel.debug);
					KVTRACKER_WAIT = num3;
				}
				else
				{
					PlayerPrefs.SetString("kvtracker_wait", "60");
					Log("Saved kvtracker_wait to persistent storage: 60", KochLogLevel.debug);
					KVTRACKER_WAIT = 60;
				}
				if (dictionary.ContainsKey("location_accuracy"))
				{
					string s4 = dictionary["location_accuracy"].ToString();
					int num4 = int.Parse(s4);
					if (num4 < 10)
					{
						num4 = 10;
					}
					if (num4 > 5000)
					{
						num4 = 5000;
					}
					locationAccuracy = num4;
				}
				if (dictionary.ContainsKey("location_timeout"))
				{
					string s5 = dictionary["location_timeout"].ToString();
					int num5 = int.Parse(s5);
					if (num5 < 3)
					{
						num5 = 3;
					}
					if (num5 > 60)
					{
						num5 = 60;
					}
					locationTimeout = num5;
				}
				if (dictionary.ContainsKey("location_staleness"))
				{
					string s6 = dictionary["location_staleness"].ToString();
					int num6 = int.Parse(s6);
					if (num6 < 1)
					{
						num6 = 1;
					}
					if (num6 > 10080)
					{
						num6 = 10080;
					}
					locationStaleness = num6;
				}
				if (dictionary.ContainsKey("iad_attribution_attempts"))
				{
					string s7 = dictionary["iad_attribution_attempts"].ToString();
					int num7 = int.Parse(s7);
					if (num7 < 1)
					{
						num7 = 1;
					}
					if (num7 > 10)
					{
						num7 = 10;
					}
					iAdAttributionAttempts = num7;
				}
				if (dictionary.ContainsKey("iad_attribution_wait"))
				{
					string s8 = dictionary["iad_attribution_wait"].ToString();
					int num8 = int.Parse(s8);
					if (num8 < 1)
					{
						num8 = 1;
					}
					if (num8 > 120)
					{
						num8 = 120;
					}
					iAdAttributionWait = num8;
				}
				if (dictionary.ContainsKey("iad_retry_wait"))
				{
					string s9 = dictionary["iad_retry_wait"].ToString();
					int num9 = int.Parse(s9);
					if (num9 < 1)
					{
						num9 = 1;
					}
					if (num9 > 60)
					{
						num9 = 60;
					}
					iAdRetryWait = num9;
				}
				if (dictionary.ContainsKey("send_id_updates") && (bool)dictionary["send_id_updates"])
				{
					send_id_updates = true;
				}
			}
			if (serverResponse.ContainsKey("blacklist"))
			{
				devIdBlacklist = new List<string>();
				if (serverResponse["blacklist"].GetType().GetElementType() == typeof(string))
				{
					try
					{
						string[] array = (string[])serverResponse["blacklist"];
						for (int num10 = array.Length - 1; num10 >= 0; num10--)
						{
							devIdBlacklist.Add(array[num10]);
							if (array[num10].ToLower().Equals("adid"))
							{
								adidBlacklisted = true;
							}
						}
					}
					catch (Exception ex)
					{
						Log("Failed parsing device_identifier blacklist received from server: " + ex, KochLogLevel.warning);
					}
				}
				try
				{
					string text = JsonWriter.Serialize(devIdBlacklist);
					PlayerPrefs.SetString("blacklist", text);
					Log("Saved device_identifier blacklist (" + devIdBlacklist.Count + " elements) to persistent storage: " + text, KochLogLevel.debug);
				}
				catch (Exception ex)
				{
					Log("Failed saving device_identifier blacklist to persistent storage: " + ex, KochLogLevel.warning);
				}
			}
			if (serverResponse.ContainsKey("whitelist") && serverResponse["whitelist"].GetType().GetElementType() == typeof(string))
			{
				string text2 = "{";
				try
				{
					string[] array2 = (string[])serverResponse["whitelist"];
					for (int num10 = array2.Length - 1; num10 >= 0; num10--)
					{
						if (array2[num10] == "location")
						{
							doReportLocation = true;
						}
						text2 = ((num10 == 0) ? (text2 + array2[num10]) : (text2 + array2[num10] + ","));
					}
				}
				catch (Exception ex)
				{
					Log("Failed parsing device_identifier whitelist received from server: " + ex, KochLogLevel.warning);
				}
				text2 += "}";
				Log("whitelist string: " + text2);
				whitelist = text2;
			}
			if (serverResponse.ContainsKey("eventname_blacklist"))
			{
				if (serverResponse["eventname_blacklist"].GetType().GetElementType() == typeof(string))
				{
					try
					{
						string[] array3 = (string[])serverResponse["eventname_blacklist"];
						for (int num10 = array3.Length - 1; num10 >= 0; num10--)
						{
							eventNameBlacklist.Add(array3[num10]);
						}
					}
					catch (Exception ex)
					{
						Log("Failed parsing eventname_blacklist received from server: " + ex, KochLogLevel.warning);
					}
				}
				PlayerPrefs.SetString("eventname_blacklist", JsonWriter.Serialize(eventNameBlacklist));
			}
			appData = new Dictionary<string, object>
			{
				{ "kochava_app_id", kochavaAppId },
				{ "kochava_device_id", kochavaDeviceId },
				{ "sdk_version", "Unity3D-20160914" },
				{ "sdk_protocol", "4" }
			};
			PlayerPrefs.SetString("kochava_device_id", kochavaDeviceId);
			Log("Saved kochava_device_id to persistent storage: " + kochavaDeviceId, KochLogLevel.debug);
			if (sessionTracking == KochSessionTracking.full || sessionTracking == KochSessionTracking.basic)
			{
				_S._fireEvent("session", new Dictionary<string, object> { { "state", "launch" } });
			}
			if (doReportLocation)
			{
				double num11 = CurrentTime();
				double num12 = double.Parse(PlayerPrefs.GetString("last_location_time"));
				if (!(num11 - num12 > (double)(locationStaleness * 60)))
				{
				}
			}
			yield return null;
			loadQueue();
		}

		private void DeviceInformationCallback(string deviceInfo)
		{
			try
			{
				hardwareIntegrationData = JsonReader.Deserialize<Dictionary<string, object>>(deviceInfo);
				Log("Received (" + hardwareIntegrationData.Count + ") parameters from Hardware Integration Library (device info): " + deviceInfo);
			}
			catch (Exception ex)
			{
				Log("Failed Deserialize hardwareIntegrationData: " + ex, KochLogLevel.warning);
			}
			if (!PlayerPrefs.HasKey("watchlistProperties"))
			{
				initInitial();
			}
			else
			{
				ScanWatchlistChanges();
			}
		}

		public static void InitInitial()
		{
			_S.initInitial();
		}

		private void initInitial()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			try
			{
				dictionary.Add("device", SystemInfo.deviceModel);
				if (hardwareIntegrationData.ContainsKey("package"))
				{
					dictionary.Add("package", hardwareIntegrationData["package"]);
				}
				else
				{
					dictionary.Add("package", appIdentifier);
				}
				if (hardwareIntegrationData.ContainsKey("app_version"))
				{
					dictionary.Add("app_version", hardwareIntegrationData["app_version"]);
				}
				else
				{
					dictionary.Add("app_version", appVersion);
				}
				if (hardwareIntegrationData.ContainsKey("app_short_string"))
				{
					dictionary.Add("app_short_string", hardwareIntegrationData["app_short_string"]);
				}
				else
				{
					dictionary.Add("app_short_string", appVersion);
				}
				dictionary.Add("currency", (appCurrency == "") ? "USD" : appCurrency);
				if (!devIdBlacklist.Contains("screen_size"))
				{
					dictionary.Add("disp_h", Screen.height);
					dictionary.Add("disp_w", Screen.width);
				}
				if (!devIdBlacklist.Contains("device_orientation") && hardwareIntegrationData.ContainsKey("device_orientation"))
				{
					dictionary.Add("device_orientation", hardwareIntegrationData["device_orientation"]);
				}
				if (!devIdBlacklist.Contains("screen_brightness") && hardwareIntegrationData.ContainsKey("screen_brightness"))
				{
					dictionary.Add("screen_brightness", hardwareIntegrationData["screen_brightness"]);
				}
				if (!devIdBlacklist.Contains("network_conn_type"))
				{
					bool flag = false;
					bool flag2 = false;
					switch (Application.internetReachability)
					{
					case NetworkReachability.ReachableViaCarrierDataNetwork:
						flag = true;
						break;
					case NetworkReachability.ReachableViaLocalAreaNetwork:
						flag2 = true;
						break;
					}
					if (flag)
					{
						dictionary.Add("network_conn_type", "cellular");
					}
					else if (flag2)
					{
						dictionary.Add("network_conn_type", "wifi");
					}
				}
				dictionary.Add("os_version", SystemInfo.operatingSystem);
				dictionary.Add("app_limit_tracking", appLimitAdTracking);
				if (!devIdBlacklist.Contains("hardware"))
				{
					dictionary.Add("device_processor", SystemInfo.processorType);
					dictionary.Add("device_cores", SystemInfo.processorCount);
					dictionary.Add("device_memory", SystemInfo.systemMemorySize);
					dictionary.Add("graphics_memory_size", SystemInfo.graphicsMemorySize);
					dictionary.Add("graphics_device_name", SystemInfo.graphicsDeviceName);
					dictionary.Add("graphics_device_vendor", SystemInfo.graphicsDeviceVendor);
					dictionary.Add("graphics_device_id", SystemInfo.graphicsDeviceID);
					dictionary.Add("graphics_device_vendor_id", SystemInfo.graphicsDeviceVendorID);
					dictionary.Add("graphics_device_version", SystemInfo.graphicsDeviceVersion);
					dictionary.Add("graphics_shader_level", SystemInfo.graphicsShaderLevel);
				}
				if (!devIdBlacklist.Contains("is_genuine") && Application.genuineCheckAvailable)
				{
					dictionary.Add("is_genuine", Application.genuine ? "1" : "0");
				}
				if (!devIdBlacklist.Contains("idfa") && hardwareIntegrationData.ContainsKey("IDFA"))
				{
					dictionary.Add("idfa", hardwareIntegrationData["IDFA"]);
				}
				if (!devIdBlacklist.Contains("idfv") && hardwareIntegrationData.ContainsKey("IDFV"))
				{
					dictionary.Add("idfv", hardwareIntegrationData["IDFV"]);
				}
				if (!devIdBlacklist.Contains("udid") && hardwareIntegrationData.ContainsKey("UDID"))
				{
					dictionary.Add("udid", hardwareIntegrationData["UDID"]);
				}
				if (!devIdBlacklist.Contains("iad_attribution") && hardwareIntegrationData.ContainsKey("iad_attribution"))
				{
					dictionary.Add("iad_attribution", hardwareIntegrationData["iad_attribution"]);
				}
				if (!devIdBlacklist.Contains("app_purchase_date") && hardwareIntegrationData.ContainsKey("app_purchase_date"))
				{
					dictionary.Add("app_purchase_date", hardwareIntegrationData["app_purchase_date"]);
				}
				if (!devIdBlacklist.Contains("iad_impression_date") && hardwareIntegrationData.ContainsKey("iad_impression_date"))
				{
					dictionary.Add("iad_impression_date", hardwareIntegrationData["iad_impression_date"]);
				}
				if (!devIdBlacklist.Contains("iad_attribution_details") && hardwareIntegrationData.ContainsKey("iad_attribution_details"))
				{
					dictionary.Add("iad_attribution_details", hardwareIntegrationData["iad_attribution_details"]);
				}
				if (!devIdBlacklist.Contains("android_id") && hardwareIntegrationData.ContainsKey("android_id"))
				{
					dictionary.Add("android_id", hardwareIntegrationData["android_id"]);
				}
				if (!devIdBlacklist.Contains("adid") && hardwareIntegrationData.ContainsKey("adid"))
				{
					dictionary.Add("adid", hardwareIntegrationData["adid"]);
				}
				if (!devIdBlacklist.Contains("fb_attribution_id") && hardwareIntegrationData.ContainsKey("fb_attribution_id"))
				{
					dictionary.Add("fb_attribution_id", hardwareIntegrationData["fb_attribution_id"]);
				}
				if (hardwareIntegrationData.ContainsKey("device_limit_tracking"))
				{
					dictionary.Add("device_limit_tracking", hardwareIntegrationData["device_limit_tracking"]);
				}
				if (!devIdBlacklist.Contains("bssid") && hardwareIntegrationData.ContainsKey("bssid"))
				{
					dictionary.Add("bssid", hardwareIntegrationData["bssid"]);
				}
				if (!devIdBlacklist.Contains("carrier_name") && hardwareIntegrationData.ContainsKey("carrier_name"))
				{
					dictionary.Add("carrier_name", hardwareIntegrationData["carrier_name"]);
				}
				if (!devIdBlacklist.Contains("volume") && hardwareIntegrationData.ContainsKey("volume"))
				{
					dictionary.Add("volume", hardwareIntegrationData["volume"]);
				}
				if (hardwareIntegrationData.ContainsKey("language"))
				{
					dictionary.Add("language", hardwareIntegrationData["language"]);
				}
				if (hardwareIntegrationData.ContainsKey("ids"))
				{
					dictionary.Add("ids", hardwareIntegrationData["ids"]);
				}
				if (hardwareIntegrationData.ContainsKey("conversion_type"))
				{
					dictionary.Add("conversion_type", hardwareIntegrationData["conversion_type"]);
				}
				if (hardwareIntegrationData.ContainsKey("conversion_data"))
				{
					dictionary.Add("conversion_data", hardwareIntegrationData["conversion_data"]);
				}
				dictionary.Add("usertime", (uint)CurrentTime());
				if ((uint)Time.time != 0)
				{
					dictionary.Add("uptime", (uint)Time.time);
				}
				float num = UptimeDelta();
				if (num >= 1f)
				{
					dictionary.Add("updelta", (uint)num);
				}
			}
			catch (Exception ex)
			{
				Log("Error preparing initial event: " + ex, KochLogLevel.error);
			}
			finally
			{
				_fireEvent("initial", dictionary);
				if (retrieveAttribution)
				{
					int num2 = 7;
					if (PlayerPrefs.HasKey("getattribution_wait"))
					{
						string @string = PlayerPrefs.GetString("getattribution_wait");
						num2 = int.Parse(@string);
					}
					Log("Will check for attribution in: " + num2);
					StartCoroutine("KochavaAttributionTimerFired", num2);
				}
			}
			try
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				if (hardwareIntegrationData.ContainsKey("device_limit_tracking"))
				{
					dictionary2.Add("device_limit_tracking", hardwareIntegrationData["device_limit_tracking"].ToString());
				}
				dictionary2.Add("os_version", SystemInfo.operatingSystem);
				dictionary2.Add("app_limit_tracking", appLimitAdTracking);
				if (hardwareIntegrationData.ContainsKey("language"))
				{
					dictionary2.Add("language", hardwareIntegrationData["language"].ToString());
				}
				if (hardwareIntegrationData.ContainsKey("app_version"))
				{
					dictionary2.Add("app_version", hardwareIntegrationData["app_version"].ToString());
				}
				else
				{
					dictionary2.Add("app_version", appVersion);
				}
				if (hardwareIntegrationData.ContainsKey("app_short_string"))
				{
					dictionary2.Add("app_short_string", hardwareIntegrationData["app_short_string"].ToString());
				}
				else
				{
					dictionary2.Add("app_short_string", appVersion);
				}
				if (!devIdBlacklist.Contains("idfa") && hardwareIntegrationData.ContainsKey("IDFA"))
				{
					dictionary2.Add("idfa", hardwareIntegrationData["IDFA"].ToString());
				}
				if (!devIdBlacklist.Contains("adid") && hardwareIntegrationData.ContainsKey("adid"))
				{
					dictionary2.Add("adid", hardwareIntegrationData["adid"]);
				}
				string text = JsonWriter.Serialize(dictionary2);
				PlayerPrefs.SetString("watchlistProperties", text);
				Log("watchlistString: " + text);
			}
			catch (Exception ex)
			{
				Log("Error setting watchlist: " + ex, KochLogLevel.error);
			}
		}

		public void ScanWatchlistChanges()
		{
			try
			{
				if (!PlayerPrefs.HasKey("watchlistProperties"))
				{
					return;
				}
				string @string = PlayerPrefs.GetString("watchlistProperties");
				Log("retrieve watchlist: " + @string);
				Dictionary<string, object> dictionary = null;
				dictionary = JsonReader.Deserialize<Dictionary<string, object>>(@string);
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				if (dictionary.ContainsKey("app_version"))
				{
					if (hardwareIntegrationData.ContainsKey("app_version"))
					{
						if (dictionary["app_version"].ToString() != hardwareIntegrationData["app_version"].ToString())
						{
							dictionary2.Add("app_version", hardwareIntegrationData["app_version"].ToString());
							dictionary["app_version"] = hardwareIntegrationData["app_version"].ToString();
						}
					}
					else if (dictionary["app_version"].ToString() != appVersion)
					{
						dictionary2.Add("app_version", appVersion);
						dictionary["app_version"] = appVersion;
					}
				}
				if (dictionary.ContainsKey("app_short_string"))
				{
					if (hardwareIntegrationData.ContainsKey("app_short_string"))
					{
						if (dictionary["app_short_string"].ToString() != hardwareIntegrationData["app_short_string"].ToString())
						{
							dictionary2.Add("app_short_string", hardwareIntegrationData["app_short_string"].ToString());
							dictionary["app_short_string"] = hardwareIntegrationData["app_short_string"].ToString();
						}
					}
					else if (dictionary["app_short_string"].ToString() != appVersion)
					{
						dictionary2.Add("app_short_string", appVersion);
						dictionary["app_short_string"] = appVersion;
					}
				}
				if (dictionary.ContainsKey("os_version") && dictionary["os_version"].ToString() != SystemInfo.operatingSystem)
				{
					dictionary2.Add("os_version", SystemInfo.operatingSystem);
					dictionary["os_version"] = SystemInfo.operatingSystem;
				}
				if (dictionary.ContainsKey("language") && hardwareIntegrationData.ContainsKey("language") && dictionary["language"].ToString() != hardwareIntegrationData["language"].ToString())
				{
					dictionary2.Add("language", hardwareIntegrationData["language"].ToString());
					dictionary["language"] = hardwareIntegrationData["language"].ToString();
				}
				if (dictionary.ContainsKey("device_limit_tracking") && hardwareIntegrationData.ContainsKey("device_limit_tracking") && dictionary["device_limit_tracking"].ToString() != hardwareIntegrationData["device_limit_tracking"].ToString())
				{
					dictionary2.Add("device_limit_tracking", hardwareIntegrationData["device_limit_tracking"].ToString());
					dictionary["device_limit_tracking"] = hardwareIntegrationData["device_limit_tracking"].ToString();
				}
				if (dictionary.ContainsKey("app_limit_tracking") && bool.Parse(dictionary["app_limit_tracking"].ToString()) != appLimitAdTracking)
				{
					dictionary2.Add("app_limit_tracking", appLimitAdTracking);
					dictionary["app_limit_tracking"] = appLimitAdTracking;
				}
				if (send_id_updates)
				{
					if (!devIdBlacklist.Contains("idfa") && dictionary.ContainsKey("idfa") && hardwareIntegrationData.ContainsKey("IDFA") && dictionary["idfa"].ToString() != hardwareIntegrationData["IDFA"].ToString())
					{
						dictionary2.Add("idfa", hardwareIntegrationData["IDFA"].ToString());
						dictionary["idfa"] = hardwareIntegrationData["IDFA"].ToString();
					}
					if (!devIdBlacklist.Contains("adid") && dictionary.ContainsKey("adid") && hardwareIntegrationData.ContainsKey("adid") && dictionary["adid"].ToString() != hardwareIntegrationData["adid"].ToString())
					{
						dictionary2.Add("adid", hardwareIntegrationData["adid"].ToString());
						dictionary["adid"] = hardwareIntegrationData["adid"].ToString();
					}
				}
				if (dictionary2.Count > 0)
				{
					string text = JsonWriter.Serialize(dictionary);
					string text2 = JsonWriter.Serialize(dictionary2);
					Log("final watchlist: " + text);
					Log("changeData: " + text2);
					PlayerPrefs.SetString("watchlistProperties", text);
					_S._fireEvent("update", dictionary2);
				}
				else
				{
					Log("No watchdata changed");
				}
			}
			catch (Exception ex)
			{
				Log("Error scanning watchlist: " + ex, KochLogLevel.error);
			}
		}

		public void Update()
		{
			if (Application.isPlaying && processQueueKickstartTime != 0f && Time.time > processQueueKickstartTime)
			{
				processQueueKickstartTime = 0f;
				StartCoroutine("processQueue");
			}
		}

		public static string GetKochavaDeviceId()
		{
			if (PlayerPrefs.HasKey("kochava_device_id"))
			{
				return PlayerPrefs.GetString("kochava_device_id");
			}
			return "";
		}

		public static void SetLimitAdTracking(bool appLimitTracking)
		{
			AppLimitAdTracking = appLimitTracking;
			_S.ScanWatchlistChanges();
		}

		public static void FireEvent(Dictionary<string, object> properties)
		{
			_S._fireEvent("event", properties);
		}

		public static void FireEvent(Hashtable propHash)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (DictionaryEntry item in propHash)
			{
				dictionary.Add((string)item.Key, item.Value);
			}
			_S._fireEvent("event", dictionary);
		}

		public static void FireEvent(string eventName, string eventData)
		{
			if (!EventNameBlacklist.Contains(eventName))
			{
				_S._fireEvent("event", new Dictionary<string, object>
				{
					{ "event_name", eventName },
					{
						"event_data",
						(eventData == null) ? "" : eventData
					}
				});
			}
		}

		public static void FireEventStandard(FireEventParameters fireEventParameters)
		{
			if (fireEventParameters != null && fireEventParameters.eventName != null && fireEventParameters.eventName.Length >= 1)
			{
				string value = JsonWriter.Serialize(fireEventParameters.valuePayload) ?? "";
				if (!EventNameBlacklist.Contains(fireEventParameters.eventName))
				{
					_S._fireEvent("event", new Dictionary<string, object>
					{
						{ "event_name", fireEventParameters.eventName },
						{ "event_data", value },
						{
							"event_standard",
							true.ToString()
						}
					});
				}
			}
		}

		public static void FireSpatialEvent(string eventName, float x, float y)
		{
			FireSpatialEvent(eventName, x, y, 0f, "");
		}

		public static void FireSpatialEvent(string eventName, float x, float y, string eventData)
		{
			FireSpatialEvent(eventName, x, y, 0f, (eventData == null) ? "" : eventData);
		}

		public static void FireSpatialEvent(string eventName, float x, float y, float z)
		{
			FireSpatialEvent(eventName, x, y, z, "");
		}

		public static void FireSpatialEvent(string eventName, float x, float y, float z, string eventData)
		{
			if (!EventNameBlacklist.Contains(eventName))
			{
				_S._fireEvent("spatial", new Dictionary<string, object>
				{
					{ "event_name", eventName },
					{ "event_data", eventData },
					{ "x", x },
					{ "y", y },
					{ "z", z }
				});
			}
		}

		public static void IdentityLink(string key, string val)
		{
			_S._fireEvent("identityLink", new Dictionary<string, object> { { key, val } });
		}

		public static void IdentityLink(Dictionary<string, object> identities)
		{
			_S._fireEvent("identityLink", identities);
		}

		public static void DeeplinkEvent(string uri, string sourceApp)
		{
			_S._fireEvent("deeplink", new Dictionary<string, object>
			{
				{ "uri", uri },
				{ "source_app", sourceApp }
			});
		}

		private void _fireEvent(string eventAction, Dictionary<string, object> eventData)
		{
			if (eventData.ContainsKey("event_name") && (eventData["event_name"] == null || eventData["event_name"].Equals("")))
			{
				Log("Cannot create event with null/empty event name.");
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (!eventData.ContainsKey("usertime"))
			{
				eventData.Add("usertime", (uint)CurrentTime());
			}
			if (!eventData.ContainsKey("uptime") && (uint)Time.time != 0)
			{
				eventData.Add("uptime", (uint)Time.time);
			}
			float num = UptimeDelta();
			if (!eventData.ContainsKey("updelta") && num >= 1f)
			{
				eventData.Add("updelta", (uint)num);
			}
			dictionary.Add("action", eventAction);
			dictionary.Add("data", eventData);
			if (eventPostingTime != 0f)
			{
				dictionary.Add("last_post_time", eventPostingTime);
			}
			if (debugMode)
			{
				dictionary.Add("debug", true);
			}
			if (debugServer)
			{
				dictionary.Add("debugServer", true);
			}
			bool isInitial = false;
			if (eventAction == "initial")
			{
				isInitial = true;
			}
			postEvent(dictionary, isInitial);
		}

		private void postEvent(Dictionary<string, object> data, bool isInitial)
		{
			QueuedEvent queuedEvent = new QueuedEvent();
			queuedEvent.eventTime = Time.time;
			queuedEvent.eventData = data;
			eventQueue.Enqueue(queuedEvent);
			if (isInitial)
			{
				StartCoroutine("processQueue");
			}
			else if (eventQueue.Count >= 75)
			{
				StartCoroutine("processQueue");
			}
			else
			{
				processQueueKickstartTime = Time.time + (float)KVTRACKER_WAIT;
			}
		}

		private void LocationReportCallback(string locationInfo)
		{
			Log("location info: " + locationInfo);
			PlayerPrefs.SetString("last_location_time", CurrentTime().ToString());
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary = JsonReader.Deserialize<Dictionary<string, object>>(locationInfo);
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("location", dictionary);
				_S._fireEvent("update", dictionary2);
			}
			catch (Exception ex)
			{
				Log("Failed Deserialize hardwareIntegrationData: " + ex, KochLogLevel.warning);
			}
		}

		private IEnumerator processQueue()
		{
			if (queueIsProcessing)
			{
				yield break;
			}
			queueIsProcessing = true;
			while (appData == null)
			{
				yield return new WaitForSeconds(15f);
				if (appData == null)
				{
					Log("Event posting delayed (AppData null, kvinit handshake incomplete or Unity reloaded assemblies)", KochLogLevel.debug);
				}
			}
			List<object> requestArray = new List<object>();
			List<object> saveArray = new List<object>();
			float postTime = Time.time;
			while (eventQueue.Count > 0)
			{
				QueuedEvent queuedEvent = eventQueue.Peek();
				saveArray.Add(queuedEvent);
				try
				{
					Dictionary<string, object> eventData = queuedEvent.eventData;
					foreach (KeyValuePair<string, object> appDatum in appData)
					{
						if (!eventData.ContainsKey(appDatum.Key))
						{
							eventData.Add(appDatum.Key, appDatum.Value);
						}
					}
					requestArray.Add(eventData);
					eventQueue.Dequeue();
				}
				catch (Exception ex)
				{
					Log("Event posting failure: " + ex, KochLogLevel.error);
				}
			}
			if (requestArray.Count > 0)
			{
				string postData = JsonWriter.Serialize(requestArray);
				Log("Posting event: " + postData.Replace("{", "{\n").Replace(",", ",\n"), KochLogLevel.debug);
				Dictionary<string, string> headers = new Dictionary<string, string>
				{
					{ "Content-Type", "application/json" },
					{ "User-Agent", userAgent }
				};
				Log(postData, KochLogLevel.debug);
				WWW www = new WWW("https://control.kochava.com/track/kvTracker?v4", Encoding.UTF8.GetBytes(postData), headers);
				yield return www;
				try
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					if (www.error == null && www.text != "")
					{
						Log("Server Response Received: " + WWW.UnEscapeURL(www.text), KochLogLevel.debug);
						dictionary = JsonReader.Deserialize<Dictionary<string, object>>(www.text);
					}
					bool flag = true;
					bool flag2 = dictionary.ContainsKey("success");
					if (!string.IsNullOrEmpty(www.error) || !flag2)
					{
						_eventPostingTime = -1f;
						if (!string.IsNullOrEmpty(www.error))
						{
							Log("Event Posting Failed: " + www.error, KochLogLevel.error);
						}
						else
						{
							Log("Event Posting Did Not Succeed: " + ((www.text == "") ? "(Blank response from server)" : www.text), KochLogLevel.error);
							if (dictionary.ContainsKey("error") || www.text == "")
							{
								flag = false;
							}
						}
						RequeuePostEvents(saveArray);
						if (flag)
						{
							processQueueKickstartTime = Time.time + 30f;
							queueIsProcessing = false;
							yield break;
						}
						Log("Event posting failure, event dequeued: " + dictionary["error"], KochLogLevel.warning);
					}
					else
					{
						_eventPostingTime = Time.time - postTime;
						Log("Event Posted (" + _eventPostingTime + " seconds to upload)");
						if (dictionary.ContainsKey("cta") && dictionary["CTA"].ToString() == "1")
						{
							Application.OpenURL(dictionary["URL"].ToString());
						}
					}
				}
				catch (Exception ex)
				{
					Log("Event posting response processing failure: " + ex, KochLogLevel.error);
				}
			}
			queueIsProcessing = false;
		}

		public void RequeuePostEvents(List<object> saveArray)
		{
			for (int i = 0; i < saveArray.Count; i++)
			{
				QueuedEvent item = (QueuedEvent)saveArray[i];
				eventQueue.Enqueue(item);
			}
		}

		public void OnApplicationPause(bool didPause)
		{
			if (sessionTracking == KochSessionTracking.full && appData != null)
			{
				_S._fireEvent("session", new Dictionary<string, object> { 
				{
					"state",
					didPause ? "pause" : "resume"
				} });
			}
			if (didPause)
			{
				saveQueue();
				return;
			}
			Log("received - app resume");
			if (PlayerPrefs.HasKey("watchlistProperties") && doReportLocation)
			{
				double num = CurrentTime();
				double num2 = double.Parse(PlayerPrefs.GetString("last_location_time"));
				if (!(num - num2 > (double)(locationStaleness * 60)))
				{
				}
			}
		}

		public void OnApplicationQuit()
		{
			if (sessionTracking == KochSessionTracking.full || sessionTracking == KochSessionTracking.basic || sessionTracking == KochSessionTracking.minimal)
			{
				_S._fireEvent("session", new Dictionary<string, object> { { "state", "quit" } });
			}
			saveQueue();
		}

		private void saveQueue()
		{
			if (eventQueue.Count > 0)
			{
				try
				{
					string text = JsonWriter.Serialize(eventQueue);
					PlayerPrefs.SetString("kochava_queue_storage", text);
					Log("Event Queue saved: " + text, KochLogLevel.debug);
				}
				catch (Exception ex)
				{
					Log("Failure saving event queue: " + ex, KochLogLevel.error);
				}
			}
		}

		private void loadQueue()
		{
			try
			{
				if (!PlayerPrefs.HasKey("kochava_queue_storage"))
				{
					return;
				}
				string @string = PlayerPrefs.GetString("kochava_queue_storage");
				int num = 0;
				QueuedEvent[] array = JsonReader.Deserialize<QueuedEvent[]>(@string);
				QueuedEvent[] array2 = array;
				foreach (QueuedEvent item in array2)
				{
					if (!eventQueue.Contains(item))
					{
						eventQueue.Enqueue(item);
						num++;
					}
				}
				Log("Loaded (" + num + ") events from persistent storage", KochLogLevel.debug);
				PlayerPrefs.DeleteKey("kochava_queue_storage");
				StartCoroutine("processQueue");
			}
			catch (Exception ex)
			{
				Log("Failure loading event queue: " + ex, KochLogLevel.debug);
			}
		}

		public static void ClearQueue()
		{
			_S.StartCoroutine("clearQueue");
		}

		private IEnumerator clearQueue()
		{
			try
			{
				Log("Clearing (" + eventQueueLength + ") events from upload queue...");
				_S.StopCoroutine("processQueue");
			}
			catch (Exception ex)
			{
				Log("Failure clearing event queue: " + ex, KochLogLevel.error);
			}
			yield return null;
			try
			{
				_S.queueIsProcessing = false;
				_S.eventQueue = new Queue<QueuedEvent>();
			}
			catch (Exception ex)
			{
				Log("Failure clearing event queue: " + ex, KochLogLevel.error);
			}
		}

		public void GetAd(int webView, int height, int width)
		{
			Log("Adserver Implementation Pending");
		}

		private static string[] Chop(string value, int length)
		{
			int num = value.Length;
			int num2 = (num + length - 1) / length;
			string[] array = new string[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i] = value.Substring(i * length, Mathf.Min(length, num));
				num -= length;
			}
			return array;
		}

		private void Log(string msg)
		{
			Log(msg, KochLogLevel.warning);
		}

		private void Log(string msg, KochLogLevel level)
		{
			if (msg.Length > 1000)
			{
				string[] array = Chop(msg, 1000);
				if (level == KochLogLevel.error)
				{
					Debug.Log("*** Kochava Error: ");
				}
				else if (debugMode)
				{
					Debug.Log("Kochava: ");
				}
				string[] array2 = array;
				foreach (string message in array2)
				{
					Debug.Log(message);
				}
			}
			else if (level == KochLogLevel.error)
			{
				Debug.Log("*** Kochava Error: " + msg + " ***");
			}
			else if (debugMode)
			{
				Debug.Log("Kochava: " + msg);
			}
			if (debugMode || level == KochLogLevel.error || level == KochLogLevel.warning)
			{
				_EventLog.Add(new LogEvent(msg, level));
			}
			if (_EventLog.Count > 50)
			{
				_EventLog.RemoveAt(0);
			}
		}

		public static void ClearLog()
		{
			_S._EventLog.Clear();
		}

		protected internal static double CurrentTime()
		{
			return (DateTime.UtcNow - Jan1st1970).TotalSeconds;
		}

		protected internal static float UptimeDelta()
		{
			uptimeDelta = Time.time - uptimeDeltaUpdate;
			uptimeDeltaUpdate = Time.time;
			return uptimeDelta;
		}

		private string CalculateMD5Hash(string input)
		{
			try
			{
				MD5 mD = MD5.Create();
				byte[] bytes = Encoding.ASCII.GetBytes(input);
				byte[] array = mD.ComputeHash(bytes);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				Log("Failure calculating MD5 hash: " + ex, KochLogLevel.error);
				return "";
			}
		}

		private string CalculateSHA1Hash(string input)
		{
			try
			{
				byte[] array = new SHA1Managed().ComputeHash(Encoding.ASCII.GetBytes(input));
				string text = string.Empty;
				byte[] array2 = array;
				foreach (byte b in array2)
				{
					text += b.ToString("x2");
				}
				return text;
			}
			catch (Exception ex)
			{
				Log("Failure calculating SHA1 hash: " + ex, KochLogLevel.error);
				return "";
			}
		}

		public IEnumerator KochavaAttributionTimerFired(int delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			Log("attribution timer wait completed");
			Dictionary<string, object> queryData = new Dictionary<string, object>
			{
				{ "action", "get_attribution" },
				{ "kochava_app_id", kochavaAppId },
				{ "kochava_device_id", kochavaDeviceId },
				{ "sdk_version", "Unity3D-20160914" },
				{ "sdk_protocol", "4" }
			};
			string queryString = JsonWriter.Serialize(queryData);
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{ "Content-Type", "application/xml" },
				{ "User-Agent", userAgent }
			};
			Log(queryString, KochLogLevel.debug);
			float wwwLoadTime = Time.time;
			WWW www = new WWW("https://control.kochava.com/track/kvquery", Encoding.UTF8.GetBytes(queryString), headers);
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				Log("kvquery (attribution) handshake failed: " + www.error + ", seconds: " + (Time.time - wwwLoadTime) + ")", KochLogLevel.warning);
				StartCoroutine("KochavaAttributionTimerFired", 60);
				yield break;
			}
			Dictionary<string, object> serverResponse = new Dictionary<string, object>();
			Log("server response: " + www.text);
			if (www.text != "")
			{
				try
				{
					serverResponse = JsonReader.Deserialize<Dictionary<string, object>>(www.text);
				}
				catch (Exception ex)
				{
					Log("Failed Deserialize JSON response to kvquery (attribution): " + ex, KochLogLevel.warning);
				}
			}
			Log(www.text, KochLogLevel.debug);
			if (!serverResponse.ContainsKey("success"))
			{
				Log("kvquery (attribution) handshake parsing failed: " + www.text, KochLogLevel.warning);
				StartCoroutine("KochavaAttributionTimerFired", 60);
			}
			else if (int.Parse(serverResponse["success"].ToString()) == 0)
			{
				Log("kvquery (attribution) did not return success = true " + www.text, KochLogLevel.warning);
				StartCoroutine("KochavaAttributionTimerFired", 60);
			}
			else
			{
				if (!serverResponse.ContainsKey("data"))
				{
					yield break;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				try
				{
					dictionary = (Dictionary<string, object>)serverResponse["data"];
				}
				catch (Exception ex)
				{
					Log("Failed Deserialize JSON attribution data chunk: " + ex, KochLogLevel.warning);
				}
				int num = 0;
				if (dictionary.ContainsKey("retry"))
				{
					num = int.Parse(dictionary["retry"].ToString());
					Log("attribution retry: " + num, KochLogLevel.warning);
				}
				if (num == -1 && dictionary.ContainsKey("attribution"))
				{
					string text = JsonWriter.Serialize(dictionary["attribution"]);
					PlayerPrefs.SetString("attribution", text);
					attributionDataStr = text;
					Log("Saved attribution chunk to persistent storage: " + text, KochLogLevel.warning);
					if (attributionCallback != null)
					{
						attributionCallback(text);
					}
				}
				if (num == 0)
				{
					StartCoroutine("KochavaAttributionTimerFired", 60);
				}
				else if (num > 0)
				{
					StartCoroutine("KochavaAttributionTimerFired", num);
				}
			}
		}

		public static string GetAttributionData()
		{
			return AttributionDataStr;
		}
	}
}
