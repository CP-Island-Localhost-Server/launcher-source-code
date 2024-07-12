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
			SettingsDataService settingsDataService = Service.Get<SettingsDataService>();
			LaunchData launchData = new LaunchData();
			launchData.MusicVolume = settingsDataService.MusicVolume;
			launchData.SFXVolume = settingsDataService.SFXVolume;
			launchData.ScreenWidth = settingsDataService.ScreenWidth;
			launchData.ScreenHeight = settingsDataService.ScreenHeight;
			launchData.Language = ((settingsDataService.Language == Language.none) ? null : settingsDataService.Language.ToString());
			string launchDataWritePath = LaunchDataHelper.GetLaunchDataWritePath(clientCurrentlyInstalledLocation);
			JsonPersistenceUtility.WriteJsonData(launchDataWritePath, launchData);
			process = new Process();
			process.EnableRaisingEvents = true;
			StringBuilder stringBuilder = new StringBuilder();
			if (settingsDataService.IsFullscreen > -1)
			{
				stringBuilder.AppendFormat(" -screen-fullscreen {0}", settingsDataService.IsFullscreen);
			}
			if (settingsDataService.ScreenWidth > 0 && settingsDataService.ScreenHeight > 0)
			{
				stringBuilder.AppendFormat(" -screen-width {0}", settingsDataService.ScreenWidth);
				stringBuilder.AppendFormat(" -screen-height {0}", settingsDataService.ScreenHeight);
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
