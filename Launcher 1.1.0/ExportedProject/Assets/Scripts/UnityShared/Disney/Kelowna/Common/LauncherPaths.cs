using System.Collections;
using System.IO;
using Microsoft.Win32;
using UnityEngine;

namespace Disney.Kelowna.Common
{
	public static class LauncherPaths
	{
		public const string CLIENT_DMG_MOUNT_POINT = "/Volumes/Club Penguin Island";

		public const string CLIENT_SOURCE_DMG_APP_NAME = "Club Penguin Island.app";

		public const string LAUNCHER_DMG_MOUNT_POINT = "/Volumes/Club Penguin Island Launcher";

		public const string LAUNCHER_DMG_APP_NAME = "Club Penguin Island Launcher.app";

		public const string LAUNCHER_HASH_CODE_PARAM = "LauncherHash";

		public const string UPDATER_BASE_PATH_PARAM = "UpdaterPath";

		public const string WINDOWS_EXE_PATH = "ClubPenguinIsland.exe";

		public const string WINDOWS_PRODUCT_GUID = "F77CC12D-3096-40F1-8D24-A3EE6AEC72B4";

		public const string WINDOWS_REGISTRY_PATH = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F77CC12D-3096-40F1-8D24-A3EE6AEC72B4}";

		public const string WINDOWS_WOW6432_REGISTRY_PATH = "HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F77CC12D-3096-40F1-8D24-A3EE6AEC72B4}";

		public const string WINDOWS_REGISTRY_VERSION_VALUE_NAME = "DisplayVersion";

		public const string WINDOWS_REGISTRY_INSTALL_LOCATION_NAME = "InstallLocation";

		private const string WINDOWS_LAUNCHER_INSTALLER_LOCATION = "CPILauncherSetup.exe";

		private const string LAUNCHER_INSTALL_RESPONSE_FILE_LOCATION = "InstallLauncher.iss";

		private const string WINDOWS_CLIENT_INSTALLER_LOCATION = "ClubPenguinIslandSetup.exe";

		private const string CLIENT_INSTALL_RESPONSE_FILE_LOCATION = "InstallClient.iss";

		private const string CLIENT_UNINSTALL_RESPONSE_FILE_LOCATION = "UninstallClient.iss";

		private const string MAC_CLIENT_DMG_LOCATION = "ClubPenguinIsland.dmg";

		private const string MAC_CLIENT_APP_NAME = "Client.app";

		private const string MAC_LAUNCHER_DMG_LOCATION = "ClubPenguinIslandLauncher.dmg";

		private const string EDITOR_TEMP_DIRECTORY = "TempLauncher";

		public static string GetLauncherHashCode()
		{
			Hashtable commandLineArgs = CommandLineArgsUtils.GetCommandLineArgs();
			if (commandLineArgs.ContainsKey("LauncherHash"))
			{
				string text = (string)commandLineArgs["LauncherHash"];
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			return null;
		}

		public static string GetUpdaterBasePath()
		{
			Hashtable commandLineArgs = CommandLineArgsUtils.GetCommandLineArgs();
			if (commandLineArgs.ContainsKey("UpdaterPath"))
			{
				string text = (string)commandLineArgs["UpdaterPath"];
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			return Application.persistentDataPath;
		}

		public static string GetLauncherDownloadLocation()
		{
			string updaterBasePath = GetUpdaterBasePath();
			return Path.Combine(updaterBasePath, "CPILauncherSetup.exe");
		}

		public static string GetLauncherInstallLocation()
		{
			return Path.Combine(getWindowsDirectory(), "Launcher");
		}

		public static string GetLauncherCurrentlyInstalledLocation()
		{
			return GetLauncherInstallLocation();
		}

		public static string GetClientInstallLocation()
		{
			return Path.Combine(getWindowsDirectory(), "Client");
		}

		public static string GetClientCurrentlyInstalledLocation()
		{
			return getRegistryValue("InstallLocation");
		}

		private static string getRegistryValue(string valueName)
		{
			string value;
			if (!tryGetRegistryValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F77CC12D-3096-40F1-8D24-A3EE6AEC72B4}", valueName, out value))
			{
				tryGetRegistryValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F77CC12D-3096-40F1-8D24-A3EE6AEC72B4}", valueName, out value);
			}
			return value;
		}

		private static bool tryGetRegistryValue(string keyName, string valueName, out string value)
		{
			value = (string)Registry.GetValue(keyName, valueName, null);
			return value != null;
		}

		public static string GetClientDownloadLocation()
		{
			return Path.Combine(Application.temporaryCachePath, "ClubPenguinIslandSetup.exe");
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

		public static string GetLauncherInstallResponseFileLocation()
		{
			string updaterBasePath = GetUpdaterBasePath();
			return Path.Combine(updaterBasePath, "InstallLauncher.iss");
		}

		public static string GetClientInstallResponseFileLocation()
		{
			return Path.Combine(Application.temporaryCachePath, "InstallClient.iss");
		}

		public static string GetClientUninstallResponseFileLocation()
		{
			return Path.Combine(getWindowsDirectory(), "UninstallClient.iss");
		}

		public static string GetTempLocation(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			return Path.Combine(fileInfo.DirectoryName, "Temp" + fileInfo.Name);
		}

		private static string getWindowsDirectory()
		{
			return Directory.GetCurrentDirectory();
		}

		private static string getMacAppDirectory()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
			return directoryInfo.Parent.FullName;
		}
	}
}
