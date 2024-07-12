using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Disney.Kelowna.Common
{
	public static class ContentHelper
	{
		public static string CDNUrl = "";

		[Conditional("FALSE")]
		public static void CheckAssetType<TExpectedBase, TAsset>()
		{
			Type typeFromHandle = typeof(TExpectedBase);
			Type typeFromHandle2 = typeof(TAsset);
			if (!typeFromHandle.IsAssignableFrom(typeFromHandle2))
			{
				throw new InvalidAssetTypeException(typeFromHandle2, typeFromHandle);
			}
		}

		public static string GetCdnUrl()
		{
			string value;
			if (!ConfigHelper.TryGetEnvironmentProperty<string>("CDN", out value))
			{
				throw new Exception("Configurator did not contain a value for the CDN URL");
			}
			string text = ((!string.IsNullOrEmpty(CDNUrl)) ? CDNUrl : value);
			text = text.Trim();
			if (text.EndsWith("/"))
			{
				return text;
			}
			return text + "/";
		}

		public static string GetCpipeMappingFilename()
		{
			string value;
			if (!ConfigHelper.TryGetEnvironmentProperty<string>("CPipeMappingFilename", out value))
			{
				throw new Exception("Configurator did not contain a value for the CPipe Mapping file!");
			}
			return value;
		}

		public static string GetClientApiVersionString()
		{
			string value;
			if (!ConfigHelper.TryGetEnvironmentProperty<string>("ClientApiVersion", out value))
			{
				return ClientInfo.GetClientVersionStr();
			}
			return value;
		}

		public static string GetPlatformString()
		{
			List<string> list = new List<string>();
			list.Add("ios");
			list.Add("android");
			list.Add("standaloneosxintel");
			list.Add("standaloneosxintel64");
			list.Add("standaloneosxuniversal");
			list.Add("standalonewindows");
			List<string> list2 = list;
			string platform = ClientInfo.Instance.Platform;
			if (!list2.Contains(platform))
			{
			}
			return platform;
		}
	}
}
