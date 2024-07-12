using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class OpenClientCommand
	{
		private Process process;

		public string LauncherContentHash { get; set; }

		public bool IsClientRunning
		{
			get
			{
				return !process.HasExited;
			}
		}

		public void Execute()
		{
			try
			{
				ClientSetupUtil.CreateMacSymLinks(true);
			}
			catch (Exception ex)
			{
				Log.LogException(this, ex);
				Log.LogError(this, "There was an exception thrown while attempting to create mac symbolic links. This could cause crashes at runtime in the client.");
			}
			string clientCurrentlyInstalledLocation = LauncherPaths.GetClientCurrentlyInstalledLocation();
			string text = null;
			SettingsDataService settingsDataService = Service.Get<SettingsDataService>();
			if (settingsDataService.HasChanges)
			{
				LaunchData launchData = new LaunchData();
				launchData.MusicVolume = settingsDataService.MusicVolume;
				launchData.SFXVolume = settingsDataService.SFXVolume;
				launchData.ScreenWidth = settingsDataService.ScreenWidth;
				launchData.ScreenHeight = settingsDataService.ScreenHeight;
				launchData.Language = ((settingsDataService.Language == Language.none) ? null : settingsDataService.Language.ToString());
				text = LaunchDataHelper.GetLaunchDataWritePath();
				JsonPersistenceUtility.WriteJsonData(text, launchData);
			}
			process = new Process();
			process.EnableRaisingEvents = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (settingsDataService.IsFullscreen > -1)
			{
				CommandLineArgsUtils.AppendArg(stringBuilder, "screen-fullscreen", settingsDataService.IsFullscreen);
			}
			if (settingsDataService.ScreenWidth > 0 && settingsDataService.ScreenHeight > 0)
			{
				CommandLineArgsUtils.AppendArg(stringBuilder, "screen-width", settingsDataService.ScreenWidth);
				CommandLineArgsUtils.AppendArg(stringBuilder, "screen-height", settingsDataService.ScreenHeight);
			}
			if (!string.IsNullOrEmpty(text))
			{
				CommandLineArgsUtils.AppendArg(stringBuilder, "launchDataPath", text);
			}
			CommandLineArgsUtils.AppendArg(stringBuilder, "UpdaterPath", LauncherPaths.GetUpdaterBasePath());
			if (!string.IsNullOrEmpty(LauncherContentHash))
			{
				CommandLineArgsUtils.AppendArg(stringBuilder, "LauncherHash", LauncherContentHash);
			}
			setupWindowsProcess(clientCurrentlyInstalledLocation, stringBuilder.ToString());
			try
			{
				process.Start();
				Service.Get<ICPSwrveService>().Action("launch_cpi");
			}
			catch (Win32Exception ex2)
			{
				Log.LogError(this, "Error starting client. Please see the exception below.");
				Log.LogException(this, ex2);
			}
		}

		private void setupWindowsProcess(string installLocation, string args)
		{
			process.StartInfo.FileName = Path.Combine(installLocation, "ClubPenguinIsland.exe").Replace('/', '\\');
			process.StartInfo.Arguments = args;
		}

		private void setupMacProcess(string installLocation, string args)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(installLocation, "Contents/MacOS/"));
			FileInfo[] files = directoryInfo.GetFiles();
			string fileName = string.Empty;
			if (files.Length == 1)
			{
				fileName = files[0].FullName;
			}
			else
			{
				Log.LogError(this, "Could not find Club Penguin executable");
			}
			process.StartInfo.FileName = fileName;
			process.StartInfo.Arguments = args;
		}
	}
}
