using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class LauncherSelfUpdateAction : LauncherAction
	{
		[LocalizationToken]
		public string ErrorPromptTitleToken;

		[LocalizationToken]
		public string ErrorPromptBodyToken;

		private string setupLogPath = null;

		private float selfUpdateProgress = 0f;

		private bool canFinish = true;

		private bool selfUpdaterActivated = false;

		[DllImport("shell32.dll", EntryPoint = "SHChangeNotify")]
		private static extern bool __SHChangeNotify(long wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

		public override IEnumerator Run()
		{
			if (!Service.Get<LoadingHandler>().LauncherNeedsUpdate)
			{
				selfUpdateProgress = 1f;
				yield break;
			}
			selfUpdateProgress = 0.5f;
			yield return null;
			string launcherDownloadLocation = getDownloadLocationAndValidate();
			if (!string.IsNullOrEmpty(launcherDownloadLocation))
			{
				runWindowsSelfUpdater(launcherDownloadLocation);
			}
			while (!canFinish)
			{
				yield return null;
			}
			selfUpdateProgress = 1f;
			yield return null;
			if (selfUpdaterActivated)
			{
				canRunNextStep = false;
				Service.Get<LoadingHandler>().InitiateApplicationQuit();
			}
		}

		public override float GetProgress()
		{
			return selfUpdateProgress;
		}

		private string getDownloadLocationAndValidate()
		{
			string launcherDownloadLocation = LauncherPaths.GetLauncherDownloadLocation();
			if (string.IsNullOrEmpty(launcherDownloadLocation) || !File.Exists(launcherDownloadLocation))
			{
				Log.LogErrorFormatted(this, "No new launcher found at launcherDownloadLocation '{0}'.", launcherDownloadLocation);
				return null;
			}
			InstallerManifestEntry launcherManifestEntry = Service.Get<LoadingHandler>().LauncherManifestEntry;
			string contentHash = launcherManifestEntry.ContentHash;
			if (!string.IsNullOrEmpty(contentHash))
			{
				string text = ContentHash.CalculateHashForFile(false, launcherDownloadLocation);
				if (!string.Equals(text, contentHash, StringComparison.Ordinal))
				{
					Log.LogErrorFormatted(this, "getDownloadLocationAndValidate() - INVALID Hash for file '{0}'\n Expected '{1}', calculated '{2}'!", launcherDownloadLocation, contentHash, text);
					return null;
				}
			}
			return launcherDownloadLocation;
		}

		private void runWindowsSelfUpdater(string launcherDownloadLocation)
		{
			setupLogPath = Path.Combine(Application.temporaryCachePath, "launcher-self-update.log");
			string launcherInstallResponseFileLocation = LauncherPaths.GetLauncherInstallResponseFileLocation();
			string arguments = ((!string.IsNullOrEmpty(setupLogPath)) ? string.Format("/s /f1\"{0}\" /f2\"{1}\" /v/qn", launcherInstallResponseFileLocation.Replace('/', '\\'), setupLogPath.Replace('/', '\\')) : string.Format("/s /f1\"{0}\" /v/qn", launcherInstallResponseFileLocation.Replace('/', '\\')));
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = launcherDownloadLocation;
			processStartInfo.Arguments = arguments;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.EnableRaisingEvents = true;
			try
			{
				process.Start();
				process.Close();
				selfUpdaterActivated = true;
			}
			catch (Win32Exception ex)
			{
				process.Close();
				showExceptionError(ex);
			}
		}

		private void runMacSelfUpdater(string launcherDownloadLocation)
		{
			string launcherSelfUpdaterScript = LauncherPaths.GetLauncherSelfUpdaterScript();
			string arguments = "\"" + launcherSelfUpdaterScript + "\"";
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = "bash";
			processStartInfo.Arguments = arguments;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.RedirectStandardInput = false;
			processStartInfo.RedirectStandardOutput = false;
			processStartInfo.RedirectStandardError = false;
			LauncherPaths.SetBashEnvironmentVariables(processStartInfo.EnvironmentVariables);
			processStartInfo.EnvironmentVariables["CPI_LAUNCHER_DMG_FILENAME"] = launcherDownloadLocation;
			processStartInfo.EnvironmentVariables["CPI_LAUNCHER_PID"] = Process.GetCurrentProcess().Id.ToString();
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.EnableRaisingEvents = true;
			try
			{
				process.Start();
				process.Close();
				selfUpdaterActivated = true;
			}
			catch (Win32Exception ex)
			{
				process.Close();
				showExceptionError(ex);
			}
		}

		private void showExceptionError(Exception ex)
		{
			Log.LogException(this, ex);
			showError(null);
		}

		private void showError(string error)
		{
			if (!string.IsNullOrEmpty(error))
			{
				Log.LogErrorFormatted(this, "Error self-updating Launcher: {0}", error);
			}
			canRunNextStep = false;
			canFinish = false;
			Service.Get<LauncherPromptManager>().ShowError(ErrorPromptTitleToken, ErrorPromptBodyToken, ButtonFlags.Cancel | ButtonFlags.Retry, onPromptButtonClicked);
		}

		private void onPromptButtonClicked(ButtonFlags clickedButtonFlag)
		{
			canFinish = true;
			if (clickedButtonFlag == ButtonFlags.Retry)
			{
				LoadingHandler loadingHandler = Service.Get<LoadingHandler>();
				if (loadingHandler.CurrentStepAttemptNumber >= 3)
				{
					loadingHandler.RetryAllLauncherActions();
				}
				else
				{
					loadingHandler.RetryCurrentLauncherAction();
				}
			}
		}
	}
}
