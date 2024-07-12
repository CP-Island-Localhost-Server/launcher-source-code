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

		private bool isRunningLauncherActions;

		private InstallType installType;

		private bool retryCurrentStep = false;

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
			splashScreen.SetActive(true);
			bool flag = false;
			installType = InstallType.Update;
			if (!InstalledCheck.IsInstalled())
			{
				flag = true;
				installType = InstallType.New;
			}
			else
			{
				InstallerManifestEntry latestClientEntry = Service.Get<InstallerManifestService>().GetLatestClientEntry();
				if (latestClientEntry != null)
				{
					Version version = new Version(InstalledCheck.GetInstalledVersion());
					flag = version < latestClientEntry.GetVersion();
				}
				else
				{
					Log.LogError(this, "Could not find a valid client Installer Manifest entry");
				}
			}
			if (flag)
			{
				CoroutineRunner.Start(runLauncherActions(), this, "LoadingHandler.load()");
				splashScreen.SetActive(false);
				Service.Get<ICPSwrveService>().Action("install_app", installType.ToString().ToLower());
				return;
			}
			bool flag2 = false;
			InstallerManifestEntry latestLauncherEntry = Service.Get<InstallerManifestService>().GetLatestLauncherEntry();
			if (latestLauncherEntry != null)
			{
				Version version = new Version(Application.version);
				flag2 = version < latestLauncherEntry.GetVersion();
			}
			if (!flag2)
			{
				OpenClientCommand openClientCommand = new OpenClientCommand();
				openClientCommand.Execute();
				QuitHelper.Quit();
			}
			else
			{
				playButton.SetIsReady();
				loadingBar.LoadProgress = 1f;
				loadingBar.SetFinished();
				splashScreen.SetActive(false);
			}
			Service.Get<ICPSwrveService>().Action("install_app", "none");
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
	}
}
