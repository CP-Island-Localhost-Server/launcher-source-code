using System.IO;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public static class LauncherPaths
	{
		private const string WINDOWS_INSTALLER_LOCATION = "ClubPenguinIslandSetup.exe";

		private const string MAC_DMG_LOCATION = "ClubPenguinIsland.dmg";

		private const string MAC_CLIENT_APP_NAME = "Client.app";

		public static string GetClientInstallLocation()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), "Client");
		}

		public static string GetClientCurrentlyInstalledLocation()
		{
			return InstalledCheck.GetWindowsInstallLocation();
		}

		public static string GetClientDownloadLocation()
		{
			return "ClubPenguinIslandSetup.exe";
		}

		public static string GetClientFrameworksLocation()
		{
			return null;
		}

		public static string GetLauncherFrameworksLocation()
		{
			return null;
		}

		public static string GetClientFrameworksRelativeLocation()
		{
			return null;
		}

		private static string getMacDirectory()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
			return directoryInfo.Parent.FullName;
		}
	}
}
