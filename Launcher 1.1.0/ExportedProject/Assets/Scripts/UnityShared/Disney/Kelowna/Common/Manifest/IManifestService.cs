using System;
using Disney.Kelowna.Common.Environment;

namespace Disney.Kelowna.Common.Manifest
{
	public interface IManifestService
	{
		object Result { get; set; }

		ContentManifest LoadEmbeddedManifest();

		ContentManifest LoadCachedCdnManifest();

		DateTimeOffset GetContentDate();

		string GetClientApiVersionStr();

		string GetClientPlatform();

		Disney.Kelowna.Common.Environment.Environment GetServerEnvironment();
	}
}
