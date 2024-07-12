using System;
using System.Collections.Generic;
using System.Globalization;
using SwrveUnity.Messaging;
using UnityEngine;

namespace SwrveUnity
{
	[Serializable]
	public class SwrveConfig
	{
		public const string DefaultEventsServer = "https://api.swrve.com";

		public const string DefaultContentServer = "https://content.swrve.com";

		public string UserId;

		public string AppVersion;

		public string AppStore = "standalone";

		public string Language;

		public string DefaultLanguage = "en";

		public bool TalkEnabled = true;

		public bool ConversationsEnabled = true;

		public bool LocationEnabled = false;

		public bool LocationAutostart = false;

		public bool AutoDownloadCampaignsAndResources = true;

		public SwrveOrientation Orientation = SwrveOrientation.Both;

		public string EventsServer = "https://api.swrve.com";

		public bool UseHttpsForEventsServer = true;

		public string ContentServer = "https://content.swrve.com";

		public bool UseHttpsForContentServer = true;

		public bool AutomaticSessionManagement = true;

		public int NewSessionInterval = 30;

		public int MaxBufferChars = 262144;

		public bool SendEventsIfBufferTooLarge = true;

		public bool StoreDataInPlayerPrefs = false;

		public Stack SelectedStack = Stack.US;

		public bool PushNotificationEnabled = false;

		public HashSet<string> PushNotificationEvents = new HashSet<string> { "Swrve.session.start" };

		public string GCMSenderId = null;

		public string GCMPushNotificationTitle = "#Your App Title";

		public string GCMPushNotificationIconId = null;

		public string GCMPushNotificationMaterialIconId = null;

		public string GCMPushNotificationLargeIconId = null;

		public int GCMPushNotificationAccentColor = -1;

		public string ADMPushNotificationTitle = "#Your App Title";

		public string ADMPushNotificationIconId = null;

		public string ADMPushNotificationMaterialIconId = null;

		public string ADMPushNotificationLargeIconId = null;

		public int ADMPushNotificationAccentColor = -1;

		public AndroidPushProvider AndroidPushProvider = AndroidPushProvider.GOOGLE_GCM;

		public float AutoShowMessagesMaxDelay = 5f;

		public Color? DefaultBackgroundColor = null;

		public bool LogGoogleAdvertisingId = false;

		public bool LogAndroidId = false;

		public bool LogAppleIDFV = false;

		public bool LogAppleIDFA = false;

		public List<UIUserNotificationCategory> pushCategories = new List<UIUserNotificationCategory>();

		public CultureInfo Culture
		{
			set
			{
				Language = value.Name;
			}
		}

		public void CalculateEndpoints(int appId)
		{
			if (string.IsNullOrEmpty(EventsServer) || EventsServer == "https://api.swrve.com")
			{
				EventsServer = CalculateEndpoint(UseHttpsForEventsServer, appId, SelectedStack, "api.swrve.com");
			}
			if (string.IsNullOrEmpty(ContentServer) || ContentServer == "https://content.swrve.com")
			{
				ContentServer = CalculateEndpoint(UseHttpsForContentServer, appId, SelectedStack, "content.swrve.com");
			}
		}

		private static string GetStackPrefix(Stack stack)
		{
			if (stack == Stack.EU)
			{
				return "eu-";
			}
			return "";
		}

		private static string HttpSchema(bool useHttps)
		{
			return useHttps ? "https" : "http";
		}

		private static string CalculateEndpoint(bool useHttps, int appId, Stack stack, string suffix)
		{
			return HttpSchema(useHttps) + "://" + appId + "." + GetStackPrefix(stack) + suffix;
		}
	}
}