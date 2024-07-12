using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using SwrveUnity.Helpers;
using UnityEngine;

namespace SwrveUnity.REST
{
	public class RESTClient : IRESTClient
	{
		private const string CONTENT_ENCODING_HEADER_KEY = "CONTENT-ENCODING";

		private List<string> metrics = new List<string>();

		public virtual IEnumerator Get(string url, Action<RESTResponse> listener)
		{
			Dictionary<string, string> headers = new Dictionary<string, string>();
			if (!Application.isEditor)
			{
				headers = AddMetricsHeader(headers);
				headers.Add("Accept-Encoding", "gzip");
			}
			long start = SwrveHelper.GetMilliseconds();
			using (WWW www = CrossPlatformUtils.MakeWWW(url, null, headers))
			{
				yield return www;
				long wwwTime = SwrveHelper.GetMilliseconds() - start;
				ProcessResponse(www, wwwTime, url, listener);
			}
		}

		public virtual IEnumerator Post(string url, byte[] encodedData, Dictionary<string, string> headers, Action<RESTResponse> listener)
		{
			if (!Application.isEditor)
			{
				headers = AddMetricsHeader(headers);
			}
			long start = SwrveHelper.GetMilliseconds();
			using (WWW www = CrossPlatformUtils.MakeWWW(url, encodedData, headers))
			{
				yield return www;
				long wwwTime = SwrveHelper.GetMilliseconds() - start;
				ProcessResponse(www, wwwTime, url, listener);
			}
		}

		protected Dictionary<string, string> AddMetricsHeader(Dictionary<string, string> headers)
		{
			if (metrics.Count > 0)
			{
				string value = string.Join(";", metrics.ToArray());
				headers.Add("Swrve-Latency-Metrics", value);
				metrics.Clear();
			}
			return headers;
		}

		private void AddMetrics(string url, long wwwTime, bool error)
		{
			Uri uri = new Uri(url);
			url = string.Format("{0}{1}{2}", uri.Scheme, "://", uri.Authority);
			string item = ((!error) ? string.Format("u={0},c={1},sh={1},sb={1},rh={1},rb={1}", url, wwwTime.ToString()) : string.Format("u={0},c={1},c_error=1", url, wwwTime.ToString()));
			metrics.Add(item);
		}

		protected void ProcessResponse(WWW www, long wwwTime, string url, Action<RESTResponse> listener)
		{
			try
			{
				WwwDeducedError wwwDeducedError = UnityWwwHelper.DeduceWwwError(www);
				if (wwwDeducedError == WwwDeducedError.NoError)
				{
					string decodedString = null;
					bool flag = ResponseBodyTester.TestUTF8(www.bytes, out decodedString);
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					string text = null;
					if (www.responseHeaders != null)
					{
						Dictionary<string, string>.Enumerator enumerator = www.responseHeaders.GetEnumerator();
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, string> current = enumerator.Current;
							dictionary.Add(current.Key.ToUpper(), current.Value);
						}
						if (dictionary.ContainsKey("CONTENT-ENCODING"))
						{
							text = dictionary["CONTENT-ENCODING"];
						}
					}
					if (www.bytes != null && www.bytes.Length > 4 && text != null && string.Equals(text, "gzip", StringComparison.OrdinalIgnoreCase) && decodedString != null && (!decodedString.StartsWith("{") || !decodedString.EndsWith("}")) && (!decodedString.StartsWith("[") || !decodedString.EndsWith("]")))
					{
						int num = BitConverter.ToInt32(www.bytes, 0);
						if (num > 0)
						{
							byte[] array = new byte[num];
							using (MemoryStream memoryStream = new MemoryStream(www.bytes))
							{
								using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream))
								{
									gZipInputStream.Read(array, 0, array.Length);
									gZipInputStream.Dispose();
								}
								flag = ResponseBodyTester.TestUTF8(array, out decodedString);
								memoryStream.Dispose();
							}
						}
					}
					if (flag)
					{
						AddMetrics(url, wwwTime, false);
						listener(new RESTResponse(decodedString, dictionary));
					}
					else
					{
						AddMetrics(url, wwwTime, true);
						listener(new RESTResponse(WwwDeducedError.ApplicationErrorBody));
					}
				}
				else
				{
					AddMetrics(url, wwwTime, true);
					listener(new RESTResponse(wwwDeducedError));
				}
			}
			catch (Exception message)
			{
				SwrveLog.LogError(message);
			}
		}
	}
}
