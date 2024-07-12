using System;
using System.Collections;
using System.Collections.Generic;
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
		public class CdnGetterHelper : CdnGetFileBIHelper
		{
			public delegate void ShowErrorPromptDelegate();

			private ShowErrorPromptDelegate showErrorPrompt;

			public CdnGetterHelper(string biTier1, string logTitle, string biLabel, string expectedContentHash, ShowErrorPromptDelegate showErrorPrompt)
				: base(biTier1, logTitle, biLabel, expectedContentHash)
			{
				this.showErrorPrompt = showErrorPrompt;
			}

			protected override void downloadFailed(string filename, string errorMessage)
			{
				base.downloadFailed(filename, errorMessage);
				if (showErrorPrompt != null)
				{
					showErrorPrompt();
				}
			}

			protected override void hashTestFailed(string filename, string calculatedHash)
			{
				base.hashTestFailed(filename, calculatedHash);
				if (showErrorPrompt != null)
				{
					showErrorPrompt();
				}
			}

			protected override void downloadedFileNotFound(string filename)
			{
				base.downloadedFileNotFound(filename);
				if (showErrorPrompt != null)
				{
					showErrorPrompt();
				}
			}
		}

		private class CdnDownloadHandler
		{
			public CdnGet cdnGet;

			public CdnGetFileBIHelper cdnGetFileHelper;

			public InstallerManifestEntry latestManifestEntry;

			public string BITier1
			{
				get
				{
					return cdnGetFileHelper.BITier1;
				}
			}
		}

		private const int DOWNLOAD_LOGGING_INCREMENT = 5;

		[LocalizationToken]
		public string ErrorPromptTitleToken;

		[LocalizationToken]
		public string ErrorPromptBodyToken;

		private float downloadProgress = 0f;

		private LauncherStatus launcherStatus = LauncherStatus.Unchanged;

		private int currentLogIncrementFactor;

		private List<CdnDownloadHandler> cdnDownloadHandlerList = new List<CdnDownloadHandler>();

		private void Start()
		{
			Service.Get<EventDispatcher>().AddListener<AppWillQuitEvent>(onAppWillQuit);
		}

		public override IEnumerator Run()
		{
			Service.Get<ICPSwrveService>().StartTimer("start_downloading", "start_downloading");
			cdnDownloadHandlerList.Clear();
			canRunNextStep = true;
			downloadProgress = 0f;
			launcherStatus = LauncherStatus.Unchanged;
			currentLogIncrementFactor = -1;
			startLauncherInstallerDownload();
			startClientInstallerDownload();
			while (isStillDownloading())
			{
				yield return null;
			}
			Service.Get<ICPSwrveService>().EndTimer("start_downloading");
			bool isSuccessfullyDownloaded = true;
			for (int i = 0; i < cdnDownloadHandlerList.Count; i++)
			{
				CdnDownloadHandler cdnDownloadHandler = cdnDownloadHandlerList[i];
				if (cdnDownloadHandler.cdnGet == null)
				{
					isSuccessfullyDownloaded = false;
				}
				else if (!cdnDownloadHandler.cdnGetFileHelper.IsDownloadSuccessful)
				{
					isSuccessfullyDownloaded = false;
				}
			}
			if (isSuccessfullyDownloaded)
			{
				launcherStatus = LauncherStatus.ClientDownloaded;
			}
			else
			{
				canRunNextStep = false;
			}
		}

		private bool isStillDownloading()
		{
			for (int i = 0; i < cdnDownloadHandlerList.Count; i++)
			{
				CdnDownloadHandler cdnDownloadHandler = cdnDownloadHandlerList[i];
				if (cdnDownloadHandler.cdnGet != null && !cdnDownloadHandler.cdnGetFileHelper.IsDownloadCompleted)
				{
					return true;
				}
			}
			return false;
		}

		private void startLauncherInstallerDownload()
		{
			if (Service.Get<LoadingHandler>().LauncherNeedsUpdate)
			{
				InstallerManifestEntry launcherManifestEntry = Service.Get<LoadingHandler>().LauncherManifestEntry;
				string launcherDownloadLocation = LauncherPaths.GetLauncherDownloadLocation();
				if (!fileAlreadyExist(launcherDownloadLocation, launcherManifestEntry.ContentHash))
				{
					File.Delete(launcherDownloadLocation);
					CdnDownloadHandler cdnDownloadHandler = new CdnDownloadHandler();
					cdnDownloadHandler.latestManifestEntry = launcherManifestEntry;
					cdnDownloadHandler.cdnGetFileHelper = new CdnGetterHelper("download_launcher", "Launcher", "launcher", launcherManifestEntry.ContentHash, showErrorPrompt);
					cdnDownloadHandler.cdnGet = new CdnGet(launcherManifestEntry.InstallerUrl, launcherDownloadLocation, cdnDownloadHandler.cdnGetFileHelper.CdnGetFileCompleted);
					cdnDownloadHandlerList.Add(cdnDownloadHandler);
					cdnDownloadHandler.cdnGet.TimeoutSeconds = 0;
					cdnDownloadHandler.cdnGet.Execute();
				}
			}
		}

		private void startClientInstallerDownload()
		{
			InstallerManifestEntry clientManifestEntry = Service.Get<LoadingHandler>().ClientManifestEntry;
			string clientDownloadLocation = LauncherPaths.GetClientDownloadLocation();
			if (!fileAlreadyExist(clientDownloadLocation, clientManifestEntry.ContentHash))
			{
				File.Delete(clientDownloadLocation);
				CdnDownloadHandler cdnDownloadHandler = new CdnDownloadHandler();
				cdnDownloadHandler.latestManifestEntry = clientManifestEntry;
				cdnDownloadHandler.cdnGetFileHelper = new CdnGetterHelper("download", "Client", "client", clientManifestEntry.ContentHash, showErrorPrompt);
				cdnDownloadHandler.cdnGet = new CdnGet(clientManifestEntry.InstallerUrl, clientDownloadLocation, cdnDownloadHandler.cdnGetFileHelper.CdnGetFileCompleted);
				cdnDownloadHandlerList.Add(cdnDownloadHandler);
				cdnDownloadHandler.cdnGet.TimeoutSeconds = 0;
				cdnDownloadHandler.cdnGet.Execute();
			}
		}

		private bool fileAlreadyExist(string filename, string expectedHash)
		{
			if (!string.IsNullOrEmpty(expectedHash) && !string.IsNullOrEmpty(filename) && File.Exists(filename))
			{
				string a = ContentHash.CalculateHashForFile(false, filename);
				if (string.Equals(a, expectedHash, StringComparison.Ordinal))
				{
					return true;
				}
			}
			return false;
		}

		public override float GetProgress()
		{
			float num;
			if (cdnDownloadHandlerList.Count <= 0)
			{
				num = 1f;
			}
			else
			{
				num = 0f;
				for (int i = 0; i < cdnDownloadHandlerList.Count; i++)
				{
					CdnDownloadHandler cdnDownloadHandler = cdnDownloadHandlerList[i];
					if (cdnDownloadHandler.cdnGet == null)
					{
						num += 1f;
						continue;
					}
					float progress = cdnDownloadHandler.cdnGet.GetProgress();
					num += progress;
					int num2 = (int)(progress * 100f);
					int num3 = num2 / 5;
					if (num3 > currentLogIncrementFactor)
					{
						currentLogIncrementFactor = num3;
						string tier = ((Service.Get<LoadingHandler>().InstallType == InstallType.New) ? "new" : "existing");
						Service.Get<ICPSwrveService>().Action(cdnDownloadHandler.BITier1, tier, (num3 * 5).ToString(), cdnDownloadHandler.latestManifestEntry.Version);
					}
				}
				num /= (float)cdnDownloadHandlerList.Count;
			}
			downloadProgress = num;
			return downloadProgress;
		}

		public override LauncherStatus GetLauncherStatus()
		{
			return launcherStatus;
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
			for (int i = 0; i < cdnDownloadHandlerList.Count; i++)
			{
				CdnDownloadHandler cdnDownloadHandler = cdnDownloadHandlerList[i];
				if (cdnDownloadHandler.cdnGet != null)
				{
					string text = string.Format("Cancelling {0} Download", cdnDownloadHandler.cdnGetFileHelper.LogTitle);
					cdnDownloadHandler.cdnGet.Cancel();
					cdnDownloadHandler.cdnGet = null;
				}
			}
		}

		private void showErrorPrompt()
		{
			Service.Get<LauncherPromptManager>().ShowError(ErrorPromptTitleToken, ErrorPromptBodyToken, ButtonFlags.Cancel | ButtonFlags.Retry, onPromptButtonClicked);
		}

		private static void onPromptButtonClicked(ButtonFlags clickedButtonFlag)
		{
			if (clickedButtonFlag == ButtonFlags.Retry)
			{
				Service.Get<LoadingHandler>().RetryAllLauncherActions();
			}
		}
	}
}
