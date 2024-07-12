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

		private InstallerManifestEntry latestLauncherEntry;

		private bool clientInstallResponseFileDownloadComplete = false;

		private bool clientUninstallResponseFileDownloadComplete = false;

		private bool launcherInstallResponseFileDownloadComplete = false;

		public override float GetProgress()
		{
			return 0f;
		}

		public override IEnumerator Run()
		{
			bool clientNeedsUpdate = Service.Get<LoadingHandler>().ClientNeedsUpdate;
			bool launcherNeedsUpdate = Service.Get<LoadingHandler>().LauncherNeedsUpdate;
			if (clientNeedsUpdate || launcherNeedsUpdate)
			{
				Service.Get<ICPSwrveService>().StartTimer("start_downloading_response_files", "start_downloading_response_files");
			}
			if (clientNeedsUpdate)
			{
				latestClientEntry = Service.Get<LoadingHandler>().ClientManifestEntry;
				string clientInstallResponseFileLocation = LauncherPaths.GetClientInstallResponseFileLocation();
				CdnGet cdnGet = new CdnGet(latestClientEntry.InstallResponseFileUrl, clientInstallResponseFileLocation, onClientInstallResponseFileComplete);
				cdnGet.Execute();
				string clientUninstallResponseFileLocation = LauncherPaths.GetClientUninstallResponseFileLocation();
				string tempLocation = LauncherPaths.GetTempLocation(clientUninstallResponseFileLocation);
				CdnGet cdnGet2 = new CdnGet(latestClientEntry.UninstallResponseFileUrl, tempLocation, onClientUninstallResponseFileComplete);
				cdnGet2.Execute();
			}
			else
			{
				clientInstallResponseFileDownloadComplete = true;
				clientUninstallResponseFileDownloadComplete = true;
			}
			if (launcherNeedsUpdate)
			{
				latestLauncherEntry = Service.Get<LoadingHandler>().LauncherManifestEntry;
				string launcherInstallResponseFileLocation = LauncherPaths.GetLauncherInstallResponseFileLocation();
				CdnGet cdnGet3 = new CdnGet(latestLauncherEntry.InstallResponseFileUrl, launcherInstallResponseFileLocation, onLauncherInstallResponseFileComplete);
				cdnGet3.Execute();
			}
			else
			{
				launcherInstallResponseFileDownloadComplete = true;
			}
			while (!clientInstallResponseFileDownloadComplete || !clientUninstallResponseFileDownloadComplete || !launcherInstallResponseFileDownloadComplete)
			{
				yield return null;
			}
			if (clientNeedsUpdate || launcherNeedsUpdate)
			{
				Service.Get<ICPSwrveService>().EndTimer("start_downloading_response_files");
			}
		}

		private void onClientInstallResponseFileComplete(bool success, string filename, string errorMessage)
		{
			if (!success)
			{
				canRunNextStep = false;
				Log.LogErrorFormatted(this, "Error downloading client install response file: {0}", errorMessage);
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
			clientInstallResponseFileDownloadComplete = true;
		}

		private void onClientUninstallResponseFileComplete(bool success, string filename, string errorMessage)
		{
			if (!success)
			{
				canRunNextStep = false;
				Log.LogErrorFormatted(this, "Error downloading client uninstall response file: {0}", errorMessage);
				Service.Get<ICPSwrveService>().Action("download_response_file", "uninstall_response_file_error", errorMessage);
				Service.Get<ICPSwrveService>().Action("error_prompt", "error_downloading_uninstall_response_file");
				showErrorPrompt();
			}
			clientUninstallResponseFileDownloadComplete = true;
		}

		private void onLauncherInstallResponseFileComplete(bool success, string filename, string errorMessage)
		{
			if (!success)
			{
				canRunNextStep = false;
				Log.LogErrorFormatted(this, "Error downloading launcher install response file: {0}", errorMessage);
				Service.Get<ICPSwrveService>().Action("download_response_file", "install_response_file_error", errorMessage);
				Service.Get<ICPSwrveService>().Action("error_prompt", "error_downloading_install_response_file");
				showErrorPrompt();
			}
			else
			{
				string text = File.ReadAllText(filename);
				text = text.Replace("{{INSTALL_PATH}}", Directory.GetCurrentDirectory().Replace('/', '\\'));
				text = text.Replace("{{VERSION}}", latestLauncherEntry.Version);
				File.WriteAllText(filename, text);
			}
			launcherInstallResponseFileDownloadComplete = true;
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
