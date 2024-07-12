using System;
using System.Collections.Generic;
using System.Diagnostics;
using Disney.LaunchPadFramework;
using Tweaker.Core;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	public static class ContentHelper
	{
		public static string CDNUrl = "";

		[Invokable("Content.ClearWWWCache", Description = "Clears all cached items from WWW.LoadFromCacheOrDownload.")]
		public static void ToggleForceAssetDownload()
		{
			if (!Caching.CleanCache())
			{
				Log.LogError(typeof(ContentHelper), "An error occured when attempting to clear cache.");
			}
		}

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

		public static string GetPlatformString()
		{
			List<string> list = new List<string>();
			list.Add("ios");
			list.Add("android");
			list.Add("standalone");
			List<string> list2 = list;
			string text = ClientInfo.Instance.Platform;
			if (text.Contains("standalone"))
			{
				text = "standalone";
			}
			if (!list2.Contains(text))
			{
			}
			return text;
		}
	}
}
