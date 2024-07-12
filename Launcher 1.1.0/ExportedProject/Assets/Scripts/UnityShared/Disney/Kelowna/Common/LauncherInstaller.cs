using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Disney.LaunchPadFramework;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	public class LauncherInstaller
	{
		private string setupLogPath = null;

		private bool canFinish = true;

		[DllImport("shell32.dll", EntryPoint = "SHChangeNotify")]
		private static extern bool __SHChangeNotify(long wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

		public IEnumerator Run()
		{
			yield break;
		}

		public IEnumerator xRun()
		{
			while (isLauncherRunning())
			{
				yield return new WaitForSeconds(1f);
			}
			string launcherDownloadLocation = getDownloadLocationAndValidate();
			if (string.IsNullOrEmpty(launcherDownloadLocation))
			{
				yield break;
			}
			Process process = new Process
			{
				StartInfo = 
				{
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true
				},
				EnableRaisingEvents = true
			};
			if (startWindowsLauncherInstallProcess(process, launcherDownloadLocation))
			{
				Stopwatch timer = new Stopwatch();
				timer.Start();
				while (!process.HasExited)
				{
					yield return null;
				}
				timer.Stop();
				if (process.StartInfo.RedirectStandardOutput && process.StandardOutput != null)
				{
					string value = process.StandardOutput.ReadToEnd();
					if (string.IsNullOrEmpty(value))
					{
					}
				}
				if (process.StartInfo.RedirectStandardError && process.StandardError != null)
				{
					string text = process.StandardError.ReadToEnd();
					if (!string.IsNullOrEmpty(text))
					{
						Log.LogErrorFormatted(this, "Error installing Launcher:\n{0}", text);
					}
				}
				yield return new WaitForSeconds(1f);
				__SHChangeNotify(134217728L, 0u, IntPtr.Zero, IntPtr.Zero);
				if (File.Exists(setupLogPath))
				{
				}
				if (!string.IsNullOrEmpty(launcherDownloadLocation) && File.Exists(launcherDownloadLocation))
				{
					File.Delete(launcherDownloadLocation);
				}
			}
			while (!canFinish)
			{
				yield return null;
			}
		}

		private string getDownloadLocationAndValidate()
		{
			string launcherDownloadLocation = LauncherPaths.GetLauncherDownloadLocation();
			if (string.IsNullOrEmpty(launcherDownloadLocation) || !File.Exists(launcherDownloadLocation))
			{
				return null;
			}
			string launcherHashCode = LauncherPaths.GetLauncherHashCode();
			if (!string.IsNullOrEmpty(launcherHashCode))
			{
				string text = ContentHash.CalculateHashForFile(false, launcherDownloadLocation);
				if (!string.Equals(text, launcherHashCode, StringComparison.Ordinal))
				{
					Log.LogErrorFormatted(this, "getDownloadLocationAndValidate() - INVALID Hash for file '{0}'\n Expected '{1}', calculated '{2}'!", launcherDownloadLocation, launcherHashCode, text);
					return null;
				}
			}
			return launcherDownloadLocation;
		}

		private bool isLauncherRunning()
		{
			bool result = false;
			using (SingleInstanceApplication singleInstanceApplication = SingleInstanceApplication.CreateClientInstance("MUTEX_620CCF48-01A3-453C-A5ED-C18A8D1724E6"))
			{
				result = singleInstanceApplication.IsAnotherProcessRunning();
			}
			return result;
		}

		private bool startWindowsLauncherInstallProcess(Process process, string launcherDownloadLocation)
		{
			setupLogPath = Path.Combine(Application.temporaryCachePath, "setup.log");
			string launcherInstallResponseFileLocation = LauncherPaths.GetLauncherInstallResponseFileLocation();
			launcherInstallResponseFileLocation = launcherInstallResponseFileLocation.Replace('/', '\\');
			string arguments = string.Format("/s /f1\"{0}\" /f2\"{1}\" /v/qn", launcherInstallResponseFileLocation, setupLogPath);
			process.StartInfo.FileName = launcherDownloadLocation;
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

		private bool startMacLauncherInstallProcess(Process process, string launcherDownloadLocation)
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
			string text = string.Format("rm -Rf \"{0}\"", LauncherPaths.GetLauncherCurrentlyInstalledLocation());
			string text2 = string.Format("rm -Rf \"{0}\"", LauncherPaths.GetLauncherInstallLocation());
			string value = string.Format("hdiutil mount \"{0}\" -mountpoint \"{1}\" -nobrowse", launcherDownloadLocation, "/Volumes/Club Penguin Island Launcher");
			string value2 = string.Format("cp -R \"{0}/{1}/\" \"{2}\"", "/Volumes/Club Penguin Island Launcher", "Club Penguin Island Launcher.app", LauncherPaths.GetLauncherInstallLocation());
			string value3 = string.Format("hdiutil unmount \"{0}\"", "/Volumes/Club Penguin Island Launcher");
			string value4 = string.Format("rm -f \"{0}\"", launcherDownloadLocation);
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
			process.StandardInput.WriteLine(value4);
			process.StandardInput.Flush();
			process.StandardInput.WriteLine("exit");
			process.StandardInput.Flush();
			return true;
		}

		private void showExceptionError(Exception ex)
		{
			Log.LogException(this, ex);
		}
	}
}
