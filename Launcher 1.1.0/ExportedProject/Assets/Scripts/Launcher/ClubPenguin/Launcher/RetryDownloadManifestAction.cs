using System;
using System.Collections;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class RetryDownloadManifestAction : LauncherAction
	{
		private const string MANIFEST_JSON = "InstallerManifest.json";

		public int RetriesPerPrompt = 1;

		[LocalizationToken]
		public string ErrorPromptTitleToken;

		[LocalizationToken]
		public string ErrorPromptBodyToken;

		private bool isComplete;

		private int currentRetryCount;

		public override float GetProgress()
		{
			return 0f;
		}

		public override IEnumerator Run()
		{
			InstallerManifestService installerManifestService = Service.Get<InstallerManifestService>();
			if (installerManifestService.IsInstallerManifestNull && !ClientInstalledCheck.IsInstalled())
			{
				currentRetryCount = RetriesPerPrompt;
				getManifest();
				while (!isComplete)
				{
					yield return null;
				}
			}
		}

		private void getManifest()
		{
			CdnGet cdnGet = new CdnGet("InstallerManifest.json", onDownloadComplete);
			cdnGet.TimeoutSeconds = 35;
			cdnGet.Execute();
		}

		private void onDownloadComplete(bool success, string payload, string errorMessage)
		{
			InstallerManifest installerManifest = null;
			if (success)
			{
				InstallerManifest installerManifest2 = new InstallerManifest();
				try
				{
					installerManifest2.Entries = Service.Get<JsonService>().Deserialize<InstallerManifestEntry[]>(payload);
				}
				catch (Exception ex)
				{
					installerManifest2.Entries = null;
					Log.LogException(this, ex);
				}
				InstallerManifestEntry latestClientEntry = InstallerManifestService.GetLatestClientEntry(installerManifest2);
				InstallerManifestEntry latestLauncherEntry = InstallerManifestService.GetLatestLauncherEntry(installerManifest2);
				if (latestClientEntry != null && latestLauncherEntry != null)
				{
					installerManifest = installerManifest2;
				}
				else
				{
					if (latestClientEntry == null)
					{
						Log.LogErrorFormatted(this, "Couldn't find a valid client Installer Manifest entry for platform [{0}]", InstallerManifest.CurrentPlatform);
					}
					if (latestLauncherEntry == null)
					{
						Log.LogErrorFormatted(this, "Couldn't find a valid launcher Installer Manifest entry for platform [{0}]", InstallerManifest.CurrentPlatform);
					}
					Log.LogErrorFormatted(this, "Installer Manifest: \n{0}", payload);
				}
			}
			else
			{
				Log.LogErrorFormatted(this, "Manifest download failed!\n{0}", errorMessage);
			}
			if (installerManifest == null)
			{
				if (currentRetryCount < RetriesPerPrompt)
				{
					currentRetryCount++;
					getManifest();
				}
				else
				{
					Service.Get<LauncherPromptManager>().ShowError(ErrorPromptTitleToken, ErrorPromptBodyToken, ButtonFlags.Retry, onPromptButtonClicked);
				}
			}
			else
			{
				Service.Get<InstallerManifestService>().UpdateInstallerManifest(installerManifest);
				isComplete = true;
			}
		}

		private void onPromptButtonClicked(ButtonFlags clickedButtonFlag)
		{
			if (clickedButtonFlag == ButtonFlags.Retry)
			{
				currentRetryCount = 1;
				getManifest();
			}
		}
	}
}
