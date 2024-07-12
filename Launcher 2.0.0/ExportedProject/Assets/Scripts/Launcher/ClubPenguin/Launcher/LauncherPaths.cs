using System;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Win32;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public static class LauncherPaths
	{
		public const string WINDOWS_EXE_FILENAME = "ClubPenguinIsland.exe";

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

		private const string MAC_CLIENT_DMG_NAME = "ClubPenguinIsland.dmg";

		private const string MAC_LAUNCHER_DMG_NAME = "ClubPenguinIslandLauncher.dmg";

		private const string USER_FACING_APP_NAME = "Club Penguin Island.app";

		private const string MAC_BUILT_LAUNCHER_APP_NAME = "Club Penguin Island Launcher.app";

		private const string MAC_PACKAGED_LAUNCHER_APP_NAME = "Club Penguin Island.app";

		private const string MAC_INSTALLED_LAUNCHER_APP_NAME = "Club Penguin Island.app";

		private const string MAC_BUILT_CLIENT_APP_NAME = "Club Penguin Island.app";

		private const string MAC_PACKAGED_CLIENT_APP_NAME = "Club Penguin Island.app";

		private const string MAC_INSTALLED_CLIENT_APP_NAME = "Client.app";

		private const string EDITOR_TEMP_DIRECTORY = "TempLauncher";

		public static string GetLauncherDownloadLocation()
		{
			string updaterBasePath = getUpdaterBasePath();
			return Path.Combine(updaterBasePath, "CPILauncherSetup.exe");
		}

		public static string GetLauncherSelfUpdaterScript()
		{
			throw new Exception("Can't get launcher self-updater script location: unsupported platform");
		}

		public static string GetClientUpdaterScript()
		{
			throw new Exception("Can't get client updater script location: unsupported platform");
		}

		public static string GetRunAppInFrontScript()
		{
			throw new Exception("Can't get client updater script location: unsupported platform");
		}

		public static string GetLauncherInstallDirectory()
		{
			return getWindowsDirectory();
		}

		public static string GetClientInstallDirectory()
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

		public static string GetLauncherInstallResponseFileLocation()
		{
			string updaterBasePath = getUpdaterBasePath();
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

		public static void SetBashEnvironmentVariables(StringDictionary environmentVariables)
		{
			environmentVariables["CPI_LAUNCHER_INSTALL_DIRECTORY"] = GetLauncherInstallDirectory();
			environmentVariables["CPI_CLIENT_INSTALL_DIRECTORY"] = GetClientInstallDirectory();
			environmentVariables["CPI_LAUNCHER_DMG_FILENAME"] = GetLauncherDownloadLocation();
			environmentVariables["CPI_CLIENT_DMG_FILENAME"] = GetClientDownloadLocation();
			environmentVariables["CPI_PACKAGED_LAUNCHER_APP_NAME"] = "Club Penguin Island.app";
			environmentVariables["CPI_PACKAGED_CLIENT_APP_NAME"] = "Club Penguin Island.app";
		}

		private static string getUpdaterBasePath()
		{
			return Application.persistentDataPath;
		}

		private static string getWindowsDirectory()
		{
			return Directory.GetCurrentDirectory();
		}

		private static string getMacLauncherInstalledDirectory()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
			return directoryInfo.Parent.FullName;
		}
	}
}
