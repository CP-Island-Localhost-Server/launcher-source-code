using System.Collections;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(InitCoreServicesAction))]
	[RequireComponent(typeof(InitLocalizerSetupAction))]
	public class InitContentSystemAction : InitActionComponent
	{
		public override bool HasSecondPass
		{
			get
			{
				return false;
			}
		}

		public override bool HasCompletedPass
		{
			get
			{
				return false;
			}
		}

		public override IEnumerator PerformFirstPass()
		{
			IGcsAccessTokenService gcsAccessTokenService = new GcsAccessTokenService(ConfigHelper.GetEnvironmentProperty<string>("GcsServiceAccountName"), new GcsP12AssetFileLoader(ConfigHelper.GetEnvironmentProperty<string>("GcsServiceAccountFile")));
			Service.Set(gcsAccessTokenService);
			ICPipeManifestService instance = new CPipeManifestService(ContentHelper.GetCdnUrl(), ContentHelper.GetCpipeMappingFilename(), gcsAccessTokenService);
			Service.Set(instance);
			ContentManifest manifest = new ContentManifest(Resources.Load<TextAsset>("Configuration/embedded_content_manifest"));
			string languageString = Service.Get<Localizer>().LanguageString;
			Service.Set(new Content(manifest, languageString));
			yield break;
		}
	}
}
