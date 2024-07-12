using System;
using System.Collections;
using System.IO;
using Disney.Kelowna.Common.Environment;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace Disney.Kelowna.Common.Manifest
{
	public class ManifestService : IManifestService
	{
		private ICoroutine coroutine;

		private IEnumerator request;

		private readonly DateTimeOffset contentDate;

		private readonly string clientVersionStr;

		private readonly string clientPlatform;

		private readonly Disney.Kelowna.Common.Environment.Environment serverEnvironment;

		public object Result { get; set; }

		public ManifestService()
		{
			contentDate = DateTimeOffset.MinValue;
			clientVersionStr = ClientInfo.GetClientVersionStr();
			clientPlatform = ContentHelper.GetPlatformString();
			serverEnvironment = Disney.Kelowna.Common.Environment.Environment.PRODUCTION;
		}

		public DateTimeOffset GetContentDate()
		{
			return contentDate;
		}

		public string GetClientVersionStr()
		{
			return clientVersionStr;
		}

		public string GetClientPlatform()
		{
			return clientPlatform;
		}

		public Disney.Kelowna.Common.Environment.Environment GetServerEnvironment()
		{
			return serverEnvironment;
		}

		public ContentManifest LoadEmbeddedManifest()
		{
			return contentManifest_loadEmbedded();
		}

		public ContentManifest LoadCachedCdnManifest()
		{
			return contentManifest_loadFromCache();
		}

		private ContentManifestDirectory manifestDirectory_loadEmbedded()
		{
			TextAsset textAsset = Resources.Load<TextAsset>("Configuration/embedded_manifest_directory.json");
			return Service.Get<JsonService>().Deserialize<ContentManifestDirectory>(textAsset.text);
		}

		private ContentManifest contentManifest_loadEmbedded()
		{
			ContentManifestDirectory contentManifestDirectory = manifestDirectory_loadEmbedded();
			ContentManifestDirectoryEntry contentManifestDirectoryEntry = contentManifestDirectory.FindEmbeddedEntry(contentDate, clientVersionStr, clientPlatform);
			if (contentManifestDirectoryEntry == null)
			{
				Log.LogErrorFormatted(this, "contentManifest_loadEmbedded(): unable to FindEmbeddedEntry for {0}, {1}, {2}", contentDate.ToString(), clientVersionStr, clientPlatform);
			}
			return new ContentManifest(Resources.Load<TextAsset>(contentManifestDirectoryEntry.url));
		}

		private ContentManifest contentManifest_loadFromCache()
		{
			string path = Path.Combine(Application.persistentDataPath, "ContentManifest.txt");
			if (File.Exists(path))
			{
				ContentManifest contentManifest = new ContentManifest();
				contentManifest.ReadFromFile(path);
				return contentManifest;
			}
			return null;
		}
	}
}
