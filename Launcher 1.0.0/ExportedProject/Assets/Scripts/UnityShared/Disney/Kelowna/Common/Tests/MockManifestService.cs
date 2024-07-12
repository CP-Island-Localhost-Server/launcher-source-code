using System;
using System.Collections;
using Disney.Kelowna.Common.Environment;
using Disney.Kelowna.Common.Manifest;
using UnityEngine;

namespace Disney.Kelowna.Common.Tests
{
	public class MockManifestService : IManifestService
	{
		public const string MOCK_MANIFEST_PATH = "fixtures/mock_content_manifest";

		public const string MOCK_MERGED_MANIFEST_PATH = "fixtures/mock_merged_content_manifest";

		public object Result { get; set; }

		public IEnumerator LoadManifestDirectory()
		{
			yield break;
		}

		public ContentManifest LoadEmbeddedManifest()
		{
			return new ContentManifest(Resources.Load<TextAsset>("fixtures/mock_content_manifest"));
		}

		public ContentManifest LoadCachedCdnManifest()
		{
			return new ContentManifest(Resources.Load<TextAsset>("fixtures/mock_content_manifest"));
		}

		public string GetContentManifestPathFromDirectory(ContentManifestDirectory directory)
		{
			return "";
		}

		public DateTimeOffset GetContentDate()
		{
			return CommonDateTime.CreateDate(2017, 1, 1);
		}

		public string GetClientVersionStr()
		{
			return ClientInfo.GetClientVersionStr();
		}

		public string GetClientPlatform()
		{
			return ContentHelper.GetPlatformString();
		}

		public Disney.Kelowna.Common.Environment.Environment GetServerEnvironment()
		{
			return Disney.Kelowna.Common.Environment.Environment.DEV;
		}
	}
}
