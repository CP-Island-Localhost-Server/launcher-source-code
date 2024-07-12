using System;
using System.Collections.Generic;
using SwrveUnityMiniJSON;
using UnityEngine;

namespace SwrveUnity.Helpers
{
	public class UnityWwwHelper
	{
		public static WwwDeducedError DeduceWwwError(WWW request)
		{
			if (request.responseHeaders.Count > 0)
			{
				string value = null;
				Dictionary<string, string>.Enumerator enumerator = request.responseHeaders.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string key = enumerator.Current.Key;
					if (string.Equals(key, "X-Swrve-Error", StringComparison.OrdinalIgnoreCase))
					{
						request.responseHeaders.TryGetValue(key, out value);
						break;
					}
				}
				if (value != null)
				{
					SwrveLog.LogError("Request response headers [\"X-Swrve-Error\"]: " + value + " at " + request.url);
					try
					{
						if (!string.IsNullOrEmpty(request.text))
						{
							SwrveLog.LogError("Request response headers [\"X-Swrve-Error\"]: " + ((IDictionary<string, object>)Json.Deserialize(request.text))["message"]);
						}
					}
					catch (Exception ex)
					{
						SwrveLog.LogError(ex.Message);
					}
					return WwwDeducedError.ApplicationErrorHeader;
				}
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				SwrveLog.LogError("Request error: " + request.error + " in " + request.url);
				return WwwDeducedError.NetworkError;
			}
			return WwwDeducedError.NoError;
		}
	}
}
