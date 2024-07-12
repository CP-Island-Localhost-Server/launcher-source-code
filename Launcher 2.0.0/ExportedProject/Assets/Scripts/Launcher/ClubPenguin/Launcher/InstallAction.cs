using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class InstallAction : LauncherAction
	{
		[LocalizationToken]
		public string ErrorPromptTitleToken;

		[LocalizationToken]
		public string ErrorPromptBodyToken;

		private string setupLogPath = null;

		private bool canFinish = true;

		private LauncherStatus launcherStatus = LauncherStatus.Unchanged;

		[DllImport("shell32.dll", EntryPoint = "SHChangeNotify")]
		private static extern bool __SHChangeNotify(long wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

		public override IEnumerator Run()
		{
			Service.Get<ICPSwrveService>().StartTimer("start_installing", "start_installing");
			Process process = null;
			try
			{
				process = startWindowsClientInstallProcess();
			}
			catch (Exception ex)
			{
				Log.LogException(this, ex);
			}
			while (process != null && !process.HasExited)
			{
				yield return null;
			}
			if (process != null)
			{
				if (process.StartInfo.RedirectStandardOutput)
				{
					string value = process.StandardOutput.ReadToEnd();
					if (string.IsNullOrEmpty(value))
					{
					}
				}
				if (process.StartInfo.RedirectStandardError)
				{
					string text = process.StandardError.ReadToEnd();
					if (!string.IsNullOrEmpty(text))
					{
						Log.LogErrorFormatted(this, "Error installing client: {0}", text);
					}
				}
				yield return new WaitForSeconds(1f);
				__SHChangeNotify(134217728L, 0u, IntPtr.Zero, IntPtr.Zero);
				if (File.Exists(setupLogPath))
				{
				}
				if (canFinish)
				{
					launcherStatus = LauncherStatus.ClientInstalled;
				}
				process.Dispose();
			}
			while (!canFinish)
			{
				yield return null;
			}
			Service.Get<ICPSwrveService>().EndTimer("start_installing");
		}

		public override float GetProgress()
		{
			return 1f;
		}

		public override LauncherStatus GetLauncherStatus()
		{
			return launcherStatus;
		}

		private Process startWindowsClientInstallProcess()
		{
			string clientDownloadLocation = LauncherPaths.GetClientDownloadLocation();
			setupLogPath = Path.Combine(Application.temporaryCachePath, "setup.log");
			string clientInstallResponseFileLocation = LauncherPaths.GetClientInstallResponseFileLocation();
			clientInstallResponseFileLocation = clientInstallResponseFileLocation.Replace('/', '\\');
			string arguments = string.Format("/s /f1\"{0}\" /f2\"{1}\" /v/qn", clientInstallResponseFileLocation, setupLogPath);
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = clientDownloadLocation;
			processStartInfo.Arguments = arguments;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.EnableRaisingEvents = true;
			try
			{
				process.Start();
			}
			catch (Win32Exception ex)
			{
				showExceptionError(ex);
				if (process != null)
				{
					process.Dispose();
					process = null;
				}
			}
			return process;
		}

		private Process startMacClientInstallProcess()
		{
			string clientDownloadLocation = LauncherPaths.GetClientDownloadLocation();
			string arguments = "\"" + LauncherPaths.GetClientUpdaterScript() + "\"";
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
			processStartInfo.EnvironmentVariables["CPI_CREATE_SYM_LINKS"] = "false";
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.EnableRaisingEvents = true;
			try
			{
				process.Start();
			}
			catch (Win32Exception ex)
			{
				process.Close();
				showExceptionError(ex);
				return null;
			}
			return process;
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
				Log.LogErrorFormatted(this, "Error installing client: {0}", error);
			}
			canRunNextStep = false;
			canFinish = false;
			Service.Get<ICPSwrveService>().Action("error_prompt", "error_installing_client");
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
