using System.Collections;
using System.IO;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class DownloadInstallShieldResponseFilesAction : LauncherAction
	{
		[LocalizationToken]
		public string ErrorPromptTitleToken;

		[LocalizationToken]
		public string ErrorPromptBodyToken;

		private InstallerManifestEntry latestClientEntry;

		private bool installResponseFileDownloadComplete;

		private bool uninstallResponseFileDownloadComplete;

		public override float GetProgress()
		{
			return 0f;
		}

		public override IEnumerator Run()
		{
			Service.Get<ICPSwrveService>().StartTimer("start_downloading_response_files", "start_downloading_response_files");
			latestClientEntry = Service.Get<InstallerManifestService>().GetLatestClientEntry();
			CdnGet installCdnGet = new CdnGet(saveToFilename: LauncherPaths.GetClientInstallResponseFileLocation(), contentPath: latestClientEntry.InstallResponseFileUrl, onGetFileComplete: onInstallResponseFileComplete);
			installCdnGet.Execute();
			string uninstallResponseFileLocation = LauncherPaths.GetClientUninstallResponseFileLocation();
			CdnGet uninstallCdnGet = new CdnGet(saveToFilename: LauncherPaths.GetTempLocation(uninstallResponseFileLocation), contentPath: latestClientEntry.UninstallResponseFileUrl, onGetFileComplete: onUninstallResponseFileComplete);
			uninstallCdnGet.Execute();
			while (!installResponseFileDownloadComplete && !uninstallResponseFileDownloadComplete)
			{
				yield return null;
			}
			Service.Get<ICPSwrveService>().EndTimer("start_downloading_response_files");
		}

		private void onInstallResponseFileComplete(bool success, string filename, string errorMessage)
		{
			if (!success)
			{
				canRunNextStep = false;
				Log.LogErrorFormatted(this, "Error downloading install response file: {0}", errorMessage);
				Service.Get<ICPSwrveService>().Action("download_response_file", "install_response_file_error", errorMessage);
				Service.Get<ICPSwrveService>().Action("error_prompt", "error_downloading_install_response_file");
				showErrorPrompt();
			}
			else
			{
				string text = File.ReadAllText(filename);
				text = text.Replace("{{INSTALL_PATH}}", Path.Combine(Directory.GetCurrentDirectory(), "Client").Replace('/', '\\'));
				text = text.Replace("{{VERSION}}", latestClientEntry.Version);
				File.WriteAllText(filename, text);
			}
			installResponseFileDownloadComplete = true;
		}

		private void onUninstallResponseFileComplete(bool success, string filename, string errorMessage)
		{
			if (!success)
			{
				canRunNextStep = false;
				Log.LogErrorFormatted(this, "Error downloading uninstall response file: {0}", errorMessage);
				Service.Get<ICPSwrveService>().Action("download_response_file", "uninstall_response_file_error", errorMessage);
				Service.Get<ICPSwrveService>().Action("error_prompt", "error_downloading_uninstall_response_file");
				showErrorPrompt();
			}
			uninstallResponseFileDownloadComplete = true;
		}

		private void showErrorPrompt()
		{
			Service.Get<LauncherPromptManager>().ShowError(ErrorPromptTitleToken, ErrorPromptBodyToken, ButtonFlags.Cancel | ButtonFlags.Retry, onPromptButtonClicked);
		}

		private void onPromptButtonClicked(ButtonFlags clickedButtonFlag)
		{
			if (clickedButtonFlag == ButtonFlags.Retry)
			{
				Service.Get<LoadingHandler>().RetryAllLauncherActions();
			}
		}
	}
}
