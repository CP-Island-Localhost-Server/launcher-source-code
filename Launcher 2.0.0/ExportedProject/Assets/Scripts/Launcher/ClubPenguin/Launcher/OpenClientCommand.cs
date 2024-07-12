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

		public bool IsClientRunning
		{
			get
			{
				return !process.HasExited;
			}
		}

		public void Execute()
		{
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
			setupWindowsProcess(clientCurrentlyInstalledLocation, stringBuilder.ToString());
			try
			{
				process.Start();
				Service.Get<ICPSwrveService>().Action("launch_cpi");
			}
			catch (Win32Exception ex)
			{
				Log.LogError(this, "Error starting client. Please see the exception below.");
				Log.LogException(this, ex);
			}
		}

		private void setupWindowsProcess(string installLocation, string args)
		{
			process.StartInfo.FileName = Path.Combine(installLocation, "ClubPenguinIsland.exe").Replace('/', '\\');
			process.StartInfo.Arguments = args;
		}

		private void setupMacProcess(string installLocation, StringBuilder args)
		{
			string value = string.Format("'{0}' '{1}' ", LauncherPaths.GetRunAppInFrontScript(), installLocation);
			args.Insert(0, value);
			process.StartInfo.FileName = "bash";
			process.StartInfo.Arguments = args.ToString();
		}
	}
}
