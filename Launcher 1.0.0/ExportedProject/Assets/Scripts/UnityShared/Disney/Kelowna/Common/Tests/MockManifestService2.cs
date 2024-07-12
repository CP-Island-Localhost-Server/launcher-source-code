using System;
using Disney.Kelowna.Common.Environment;
using Disney.Kelowna.Common.Manifest;
using UnityEngine;

namespace Disney.Kelowna.Common.Tests
{
	public class MockManifestService2 : IManifestService
	{
		public const string MOCK_MANIFEST_PATH = "fixtures/mock_content_manifest_txt";

		public const string MOCK_MERGED_MANIFEST_PATH = "fixtures/mock_merged_content_manifest_txt";

		private readonly string client_version;

		private readonly string platform;

		private readonly DateTimeOffset contentDate;

		private readonly Disney.Kelowna.Common.Environment.Environment environment;

		public object Result { get; set; }

		public MockManifestService2(string client_version, string platform, DateTimeOffset contentDate, Disney.Kelowna.Common.Environment.Environment environment)
		{
			this.client_version = client_version;
			this.platform = platform;
			this.contentDate = contentDate;
			this.environment = environment;
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
			Debug.LogFormat("GetContentManifestPathFromDirectory(): contentDate={0}, client_version={1}, platform={2}, environment={3}", CommonDateTime.Serialize(GetContentDate()), client_version, platform, environment);
			ContentManifestDirectoryEntry contentManifestDirectoryEntry = directory.FindEntry(GetContentDate(), client_version, platform, environment.ToString().ToLower());
			if (contentManifestDirectoryEntry != null)
			{
				return contentManifestDirectoryEntry.url;
			}
			return null;
		}

		public DateTimeOffset GetContentDate()
		{
			return contentDate;
		}

		public string GetClientVersionStr()
		{
			return client_version;
		}

		public string GetClientPlatform()
		{
			return platform;
		}

		public Disney.Kelowna.Common.Environment.Environment GetServerEnvironment()
		{
			return environment;
		}
	}
}
