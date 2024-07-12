using System;
using System.Collections;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class LoadingHandler : MonoBehaviour
	{
		private const int RUN_LAUNCHER_INSTALLER_RETRY_COUNT = 60;

		private const int RUN_LAUNCHER_INSTALLER_CONNECT_TIMEOUT = 500;

		private const int DELAY_BEFORE_QUIT = 8;

		[SerializeField]
		private LauncherLoadingBar loadingBar = null;

		[SerializeField]
		private LauncherLoadingSnippets loadingSnippets = null;

		[SerializeField]
		private GameObject splashScreen = null;

		[SerializeField]
		private LauncherPlayButton playButton = null;

		[SerializeField]
		private bool resetProgressEveryStep = false;

		private bool launcherNeedsUpdate = false;

		private bool clientNeedsUpdate = false;

		private InstallerManifestEntry launcherManifestEntry;

		private InstallerManifestEntry clientManifestEntry;

		private bool isRunningLauncherActions;

		private InstallType installType;

		private bool retryCurrentStep = false;

		private SingleInstanceApplication singleInstanceApp;

		private bool launcherCanBeClosed = false;

		private OpenClientCommand openClientCommand;

		private bool isQuitting = false;

		private bool isQuitBILogged = false;

		private DateTimeOffset? doNotQuitUntil = null;

		public bool LauncherNeedsUpdate
		{
			get
			{
				return launcherNeedsUpdate;
			}
		}

		public bool ClientNeedsUpdate
		{
			get
			{
				return clientNeedsUpdate;
			}
		}

		public InstallerManifestEntry LauncherManifestEntry
		{
			get
			{
				return launcherManifestEntry;
			}
		}

		public InstallerManifestEntry ClientManifestEntry
		{
			get
			{
				return clientManifestEntry;
			}
		}

		public bool IsRunningLauncherActions
		{
			get
			{
				return isRunningLauncherActions;
			}
		}

		public InstallType InstallType
		{
			get
			{
				return installType;
			}
		}

		public int CurrentStepAttemptNumber { get; private set; }

		private void OnValidate()
		{
		}

		private void Start()
		{
			playButton.Interactable = false;
			Service.Set(this);
			singleInstanceApp = Service.Get<SingleInstanceApplication>();
			splashScreen.SetActive(true);
			installType = InstallType.Update;
			launcherManifestEntry = Service.Get<InstallerManifestService>().GetLatestLauncherEntry();
			clientManifestEntry = Service.Get<InstallerManifestService>().GetLatestClientEntry();
			if (launcherManifestEntry != null)
			{
				Version version = new Version(Application.version);
				launcherNeedsUpdate = version < launcherManifestEntry.GetVersion();
				Service.Get<ICPSwrveService>().Action("check_launcher_version", launcherManifestEntry.Version, Application.version, launcherNeedsUpdate ? "yes" : "no");
			}
			else
			{
				Log.LogError(this, "Could not find a valid 'Launcher' Installer Manifest entry");
			}
			string text = null;
			if (!ClientInstalledCheck.IsInstalled())
			{
				clientNeedsUpdate = true;
				installType = InstallType.New;
			}
			else if (clientManifestEntry != null)
			{
				text = ClientInstalledCheck.GetInstalledVersion();
				Version version = new Version(text);
				clientNeedsUpdate = version < clientManifestEntry.GetVersion();
			}
			else
			{
				Log.LogError(this, "Could not find a valid 'Client' Installer Manifest entry");
			}
			Service.Get<ICPSwrveService>().Action("check_game_version", clientManifestEntry.Version, text, clientNeedsUpdate ? "yes" : "no");
			if (launcherNeedsUpdate || clientNeedsUpdate)
			{
				CoroutineRunner.Start(runLauncherActions(), this, "LoadingHandler.load()");
				splashScreen.SetActive(false);
				Service.Get<ICPSwrveService>().Action("install_app", installType.ToString().ToLower());
			}
			else
			{
				Service.Get<ICPSwrveService>().Action("install_app", "none");
				RunTheClient();
				doNotQuitUntil = DateTimeOffset.UtcNow.AddSeconds(8.0);
			}
		}

		public void RunTheClient()
		{
			openClientCommand = new OpenClientCommand();
			openClientCommand.Execute();
			launcherActionCompleted(LauncherStatus.ClientRunning);
		}

		public void RetryAllLauncherActions()
		{
			if (!isRunningLauncherActions)
			{
				CoroutineRunner.Start(runLauncherActions(), this, "LoadingHandler.runLauncherActions()");
			}
			else
			{
				CoroutineRunner.StartPersistent(delayRetryAllLauncherActions(), this, "delayRetryAllLauncherActions");
			}
		}

		private IEnumerator delayRetryAllLauncherActions()
		{
			yield return null;
			if (!isRunningLauncherActions)
			{
				RetryAllLauncherActions();
			}
			else
			{
				Log.LogError(this, "Cannot retry actions because actions are already running.");
			}
		}

		public void RetryCurrentLauncherAction()
		{
			retryCurrentStep = true;
		}

		private IEnumerator runLauncherActions()
		{
			isRunningLauncherActions = true;
			loadingSnippets.Show();
			LauncherAction[] launcherActions = GetComponents<LauncherAction>();
			float totalRatio = 0f;
			LauncherAction[] array = launcherActions;
			foreach (LauncherAction launcherAction2 in array)
			{
				totalRatio += launcherAction2.LoadRatio;
			}
			if (totalRatio < Mathf.Epsilon)
			{
				isRunningLauncherActions = false;
				yield break;
			}
			loadingBar.IsLoadingBarAnimating = true;
			float progress = 0f;
			try
			{
				LauncherAction[] array2 = launcherActions;
				foreach (LauncherAction launcherAction in array2)
				{
					CurrentStepAttemptNumber = 0;
					retryCurrentStep = true;
					while (retryCurrentStep)
					{
						CurrentStepAttemptNumber++;
						retryCurrentStep = false;
						if (!string.IsNullOrEmpty(launcherAction.ButtonToken))
						{
							playButton.Text = Service.Get<Localizer>().GetTokenTranslation(launcherAction.ButtonToken);
						}
						float actionRatio = launcherAction.LoadRatio / totalRatio;
						ICoroutine coroutine = CoroutineRunner.Start(launcherAction.Run(), this, "LoadActionLoad");
						while (!coroutine.Completed)
						{
							float currentStepProgress = Math.Min(1f, launcherAction.GetProgress());
							if (resetProgressEveryStep)
							{
								loadingBar.LoadProgress = currentStepProgress;
							}
							else
							{
								loadingBar.LoadProgress = progress + currentStepProgress * actionRatio;
							}
							yield return null;
						}
						launcherActionCompleted(launcherAction.GetLauncherStatus());
						if (launcherAction.CanRunNextStep)
						{
							progress += actionRatio;
							loadingBar.LoadProgress = (resetProgressEveryStep ? 1f : progress);
						}
						else if (!retryCurrentStep)
						{
							loadingBar.LoadProgress = 0f;
							loadingBar.IsLoadingBarAnimating = false;
							loadingSnippets.Hide();
							isRunningLauncherActions = false;
							yield break;
						}
					}
				}
			}
			finally
			{
			}
			loadingBar.SetFinished();
			loadingSnippets.Hide();
			playButton.SetIsReady();
			isRunningLauncherActions = false;
		}

		private void launcherActionCompleted(LauncherStatus launcherStatus)
		{
			if (launcherStatus == LauncherStatus.ClientRunning)
			{
				launcherCanBeClosed = true;
			}
		}

		private void Update()
		{
			if (!isQuitting && isReadyToQuit())
			{
				isQuitting = true;
				InitiateApplicationQuit();
			}
		}

		private void OnDestroy()
		{
			singleInstanceApp.Dispose();
			singleInstanceApp = null;
		}

		private void OnApplicationQuit()
		{
			isQuitting = true;
			logQuitBI();
		}

		public void InitiateApplicationQuit()
		{
			logQuitBI();
			QuitHelper.Quit();
		}

		private void logQuitBI()
		{
			if (!isQuitBILogged && Service.IsSet<ICPSwrveService>())
			{
				Service.Get<ICPSwrveService>().Quit();
				isQuitBILogged = true;
			}
		}

		private bool isReadyToQuit()
		{
			if (!launcherCanBeClosed)
			{
				return false;
			}
			if (doNotQuitUntil.HasValue && doNotQuitUntil > DateTimeOffset.UtcNow)
			{
				return false;
			}
			return true;
		}
	}
}
