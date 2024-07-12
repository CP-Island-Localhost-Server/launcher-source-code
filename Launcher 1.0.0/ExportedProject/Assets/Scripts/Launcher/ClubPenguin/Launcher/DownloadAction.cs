using System.Collections;
using System.IO;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class DownloadAction : LauncherAction
	{
		private const int DOWNLOAD_LOGGING_INCREMENT = 5;

		[LocalizationToken]
		public string ErrorPromptTitleToken;

		[LocalizationToken]
		public string ErrorPromptBodyToken;

		private InstallerManifestEntry latestClientEntry;

		private bool downloadComplete = false;

		private float downloadProgress = 0f;

		private int currentLogIncrementFactor;

		private CdnGet cdnGet;

		private void Start()
		{
			Service.Get<EventDispatcher>().AddListener<AppWillQuitEvent>(onAppWillQuit);
		}

		public override IEnumerator Run()
		{
			Service.Get<ICPSwrveService>().StartTimer("start_downloading", "start_downloading");
			canRunNextStep = true;
			downloadComplete = false;
			downloadProgress = 0f;
			currentLogIncrementFactor = -1;
			latestClientEntry = Service.Get<InstallerManifestService>().GetLatestClientEntry();
			cdnGet = new CdnGet(saveToFilename: LauncherPaths.GetClientDownloadLocation(), contentPath: latestClientEntry.InstallerUrl, onGetFileComplete: downloadFileCompleted);
			cdnGet.TimeoutSeconds = 0;
			cdnGet.Execute();
			while (!downloadComplete)
			{
				yield return null;
			}
			Service.Get<ICPSwrveService>().EndTimer("start_downloading");
		}

		public override float GetProgress()
		{
			if (cdnGet != null)
			{
				downloadProgress = cdnGet.GetProgress();
				int num = (int)(downloadProgress * 100f);
				int num2 = num / 5;
				if (num2 > currentLogIncrementFactor)
				{
					currentLogIncrementFactor = num2;
					string tier = ((Service.Get<LoadingHandler>().InstallType == InstallType.New) ? "new" : "existing");
					Service.Get<ICPSwrveService>().Action("download", tier, (num2 * 5).ToString(), latestClientEntry.Version);
				}
			}
			return downloadProgress;
		}

		private void OnDestroy()
		{
			Cancel();
		}

		private bool onAppWillQuit(AppWillQuitEvent evt)
		{
			Cancel();
			return false;
		}

		public void Cancel()
		{
			if (cdnGet != null)
			{
				cdnGet.Cancel();
				cdnGet = null;
			}
		}

		private void downloadFileCompleted(bool success, string filename, string errorMessage)
		{
			if (!success)
			{
				canRunNextStep = false;
				Log.LogErrorFormatted(this, "Error downloading client: {0}", errorMessage);
				Service.Get<ICPSwrveService>().Action("download", "error", errorMessage);
				Service.Get<ICPSwrveService>().Action("error_prompt", "error_downloading_client");
				showErrorPrompt();
			}
			else
			{
				if (File.Exists(filename))
				{
					if (!string.IsNullOrEmpty(latestClientEntry.ContentHash))
					{
						string text = ContentHash.CalculateHashForFile(false, filename);
						if (text != latestClientEntry.ContentHash)
						{
							canRunNextStep = false;
							Log.LogErrorFormatted(this, "Error downloading client: File content hash does not match. File hash: {0}, Manifest entry hash: {1}", text, latestClientEntry.ContentHash);
							showErrorPrompt();
							Service.Get<ICPSwrveService>().Action("download", "error", "content_hash_mismatch");
						}
					}
				}
				else
				{
					canRunNextStep = false;
					Log.LogErrorFormatted(this, "Error downloading client: Downloaded file not found at path: {0}", filename);
					showErrorPrompt();
					Service.Get<ICPSwrveService>().Action("download", "error", "file_not_found");
				}
				Service.Get<ICPSwrveService>().Action("download", "complete");
			}
			downloadComplete = true;
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
