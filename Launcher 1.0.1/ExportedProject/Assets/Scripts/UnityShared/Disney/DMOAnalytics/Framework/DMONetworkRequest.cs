using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public class DMONetworkRequest : IDMONetworkRequest
	{
		private const int mTimeOut = 60000;

		private MonoBehaviour mGameObj;

		private Queue<Dictionary<string, object>> mEventQueue = new Queue<Dictionary<string, object>>();

		private static string multipartSeparator = "GOGO-87dae883c6039b244c0d341f45f8-SEP";

		private static IDMONetworkRequest mInstance;

		public MonoBehaviour GameObj
		{
			set
			{
				if (mGameObj == null)
				{
					mGameObj = value;
				}
			}
		}

		public static IDMONetworkRequest Instance
		{
			get
			{
				if (mInstance == null)
				{
					mInstance = new DMONetworkRequest();
				}
				return mInstance;
			}
		}

		private DMONetworkRequest()
		{
		}

		public void FlushQueue()
		{
			if (DMOAnalyticsHelper.ICanUseNetwork && mEventQueue.Count > 0)
			{
				DMOAnalyticsHelper.Log("Flush the analytics queue");
				while (mEventQueue.Count > 0)
				{
					Dictionary<string, object> gameData = mEventQueue.Peek();
					StartCoroutine(gameData);
					mEventQueue.Dequeue();
				}
			}
		}

		public void StartCoroutine(Dictionary<string, object> GameData)
		{
			if (mGameObj == null)
			{
				GameObject gameObject = new GameObject("DMOAnalytics Coroutines");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				mGameObj = gameObject.AddComponent<MonoBehaviour>();
			}
			if (GameData != null)
			{
				if (Application.internetReachability != 0 && DMOAnalyticsHelper.ICanUseNetwork)
				{
					mGameObj.StartCoroutine(_runEventRequest(GameData));
				}
				else
				{
					_addGameDataToEventQueue(GameData);
				}
			}
		}

		private string getTimeStamp()
		{
			DateTime utcNow = DateTime.UtcNow;
			DateTime value = Convert.ToDateTime("1/1/1970 0:00:00 AM");
			TimeSpan timeSpan = utcNow.Subtract(value);
			return ((long)(((timeSpan.Days * 24 + timeSpan.Hours) * 60 + timeSpan.Minutes) * 60 + timeSpan.Seconds)).ToString();
		}

		private string getHostURL()
		{
			return DMOAnalyticsSysInfo.getAnalyticsURL();
		}

		private bool _compareTwoDictionary(Dictionary<string, object> GameData, Dictionary<string, object> CollectData)
		{
			bool result = false;
			object obj = GameData["method"];
			object obj2 = GameData["timestamp"];
			bool flag = GameData.ContainsKey("data");
			if (obj.Equals(CollectData["method"]) && obj2.Equals(CollectData["timestamp"]))
			{
				if (!flag)
				{
					result = true;
				}
				else
				{
					object obj3 = GameData["data"];
					if (obj3.Equals(CollectData["data"]))
					{
						result = true;
					}
				}
			}
			return result;
		}

		private void _addGameDataToEventQueue(Dictionary<string, object> GameData)
		{
			bool flag = true;
			foreach (Dictionary<string, object> item in mEventQueue)
			{
				if (_compareTwoDictionary(GameData, item))
				{
					flag = false;
					DMOAnalyticsHelper.Log("GameData is duplicate, no need to add again: " + GameData["method"]);
					break;
				}
			}
			if (flag)
			{
				mEventQueue.Enqueue(GameData);
			}
		}

		private IEnumerator _runEventRequest(Dictionary<string, object> postData)
		{
			Dictionary<string, string> dictHeaders = new Dictionary<string, string>();
			string authorizationField = string.Format("FD {0}:{1}", DMOAnalyticsSysInfo.getCellophaneKey(), DMOAnalyticsSysInfo.getCellophaneSecret());
			dictHeaders.Add("Authorization", authorizationField);
			string contentType = "multipart/form-data; boundary=" + multipartSeparator;
			dictHeaders.Add("Content-Type", contentType);
			string apiSer = _signatureFromPostData(postData);
			postData.Add("api_sig", apiSer);
			string dataStr = getDataBodyStr(postData);
			string hostURL = getHostURL();
			DMOAnalyticsHelper.Log("Connecting to Server: " + hostURL);
			WWW www = new WWW(hostURL, Encoding.UTF8.GetBytes(dataStr), dictHeaders);
			yield return www;
			bool isPosted = www.isDone && www.error == null && www.text != null;
			if (!isPosted)
			{
				DMOAnalyticsHelper.Log("Errr in WWW request:" + www.error);
			}
			if (isPosted)
			{
				try
				{
					string value = www.responseHeaders["CONTENT-LENGTH"];
					int num = Convert.ToInt32(value);
					if (num >= 1)
					{
					}
				}
				catch (Exception ex)
				{
					DMOAnalyticsHelper.Log("Errr in WWW response: " + ex.ToString());
				}
			}
			DMOAnalyticsHelper.Log("WWW request is posted");
		}

		private string getDataBodyStr(Dictionary<string, object> postData)
		{
			string text = "";
			foreach (KeyValuePair<string, object> postDatum in postData)
			{
				object value = postDatum.Value;
				if (value != null)
				{
					text += string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", multipartSeparator, postDatum.Key, (string)value);
				}
			}
			text += "--";
			text += multipartSeparator;
			return text + "--\r\n";
		}

		private string _signatureFromPostData(Dictionary<string, object> postData)
		{
			string[] array = new string[postData.Keys.Count];
			string[] array2 = new string[postData.Keys.Count];
			postData.Keys.CopyTo(array2, 0);
			Array.Sort(array2, StringComparer.Ordinal);
			int num = 0;
			string[] array3 = array2;
			foreach (string text in array3)
			{
				string text2 = (string)postData[text];
				string stringToEscape = text2.Replace(" ", "+");
				array[num] = text + "=" + Uri.EscapeDataString(stringToEscape);
				num++;
			}
			string inString = Uri.EscapeDataString(string.Join("&", array));
			string s = _tempFixForDoubleEncoding(inString);
			string s2 = string.Format("{0}&", DMOAnalyticsSysInfo.getAnalyticsSecret());
			byte[] bytes = Encoding.UTF8.GetBytes(s2);
			byte[] bytes2 = Encoding.UTF8.GetBytes(s);
			HMACSHA1 hMACSHA = new HMACSHA1(bytes);
			hMACSHA.Initialize();
			byte[] inArray = hMACSHA.ComputeHash(bytes2);
			return Convert.ToBase64String(inArray);
		}

		private string _tempFixForDoubleEncoding(string inString)
		{
			string text = inString.Replace("(", "%2528");
			return text.Replace(")", "%2529");
		}
	}
}
