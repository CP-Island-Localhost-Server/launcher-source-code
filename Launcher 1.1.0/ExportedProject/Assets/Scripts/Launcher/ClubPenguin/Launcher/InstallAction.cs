using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
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
			Process process = new Process
			{
				StartInfo = 
				{
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true
				},
				EnableRaisingEvents = true
			};
			string clientDownloadLocation = LauncherPaths.GetClientDownloadLocation();
			if (startWindowsClientInstallProcess(process, clientDownloadLocation))
			{
				Stopwatch timer = new Stopwatch();
				timer.Start();
				while (!process.HasExited)
				{
					yield return null;
				}
				timer.Stop();
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

		private bool startWindowsClientInstallProcess(Process process, string clientDownloadLocation)
		{
			setupLogPath = Path.Combine(Application.temporaryCachePath, "setup.log");
			string clientInstallResponseFileLocation = LauncherPaths.GetClientInstallResponseFileLocation();
			clientInstallResponseFileLocation = clientInstallResponseFileLocation.Replace('/', '\\');
			string arguments = string.Format("/s /f1\"{0}\" /f2\"{1}\" /v/qn", clientInstallResponseFileLocation, setupLogPath);
			process.StartInfo.FileName = clientDownloadLocation;
			process.StartInfo.Arguments = arguments;
			try
			{
				process.Start();
			}
			catch (Win32Exception ex)
			{
				showExceptionError(ex);
				return false;
			}
			return true;
		}

		private bool startMacClientInstallProcess(Process process, string clientDownloadLocation)
		{
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.FileName = "/bin/bash";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.RedirectStandardOutput = true;
			try
			{
				process.Start();
			}
			catch (Win32Exception ex)
			{
				showExceptionError(ex);
				return false;
			}
			string text = string.Format("rm -Rf \"{0}\"", LauncherPaths.GetClientCurrentlyInstalledLocation());
			string text2 = string.Format("rm -Rf \"{0}\"", LauncherPaths.GetClientInstallLocation());
			string value = string.Format("hdiutil mount \"{0}\" -mountpoint \"{1}\" -nobrowse", clientDownloadLocation, "/Volumes/Club Penguin Island");
			string value2 = string.Format("cp -R \"{0}/{1}/\" \"{2}\"", "/Volumes/Club Penguin Island", "Club Penguin Island.app", LauncherPaths.GetClientInstallLocation());
			string value3 = string.Format("hdiutil unmount \"{0}\"", "/Volumes/Club Penguin Island");
			process.StandardInput.WriteLine("");
			process.StandardInput.Flush();
			process.StandardInput.WriteLine(text);
			process.StandardInput.Flush();
			if (!string.Equals(text, text2, StringComparison.Ordinal))
			{
				process.StandardInput.WriteLine(text2);
				process.StandardInput.Flush();
			}
			process.StandardInput.WriteLine(value);
			process.StandardInput.Flush();
			process.StandardInput.WriteLine(value2);
			process.StandardInput.Flush();
			process.StandardInput.WriteLine(value3);
			process.StandardInput.Flush();
			ClientSetupUtil.CreateMacSymLinks(false, process);
			return true;
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
