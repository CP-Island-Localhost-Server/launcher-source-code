using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Disney.DMOAnalytics.Framework
{
	public class DMOAnalyticsHelper
	{
		public static bool isDebugEnvLog = false;

		public static bool ICanUseNetwork = true;

		public static bool RestrictedTracking = false;

		public static void Log(string msg)
		{
			if (isDebugEnvLog)
			{
				Debug.Log(msg);
			}
		}

		public static string GetStringFromDictionary(Dictionary<string, object> dictData)
		{
			string result = "";
			if (dictData != null)
			{
				result = JsonMapper.ToJson(dictData);
			}
			return result;
		}
	}
}
