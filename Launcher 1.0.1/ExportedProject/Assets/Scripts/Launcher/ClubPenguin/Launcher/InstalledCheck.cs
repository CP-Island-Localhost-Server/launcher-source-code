using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Disney.LaunchPadFramework;
using Microsoft.Win32;

namespace ClubPenguin.Launcher
{
	public static class InstalledCheck
	{
		public static bool IsInstalled()
		{
			return !string.IsNullOrEmpty(GetInstalledVersion());
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

		public static string GetInstalledVersion()
		{
			return getRegistryValue("DisplayVersion");
		}

		public static string GetWindowsInstallLocation()
		{
			return getRegistryValue("InstallLocation");
		}

		private static string getMacInstalledVersion(string currentlyInstalledLocation)
		{
			string result = null;
			Process process = new Process();
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.CreateNoWindow = true;
			process.EnableRaisingEvents = true;
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
				Log.LogErrorFormatted(typeof(InstalledCheck), "Error installing client: {0}", ex.Message);
			}
			string arg = Path.Combine(currentlyInstalledLocation, "Contents/Info.plist");
			string value = string.Format("defaults read \"{0}\" | grep CFBundleShortVersionString", arg);
			process.StandardInput.WriteLine("");
			process.StandardInput.WriteLine(value);
			process.StandardInput.WriteLine("exit");
			process.StandardInput.Flush();
			if (process.StartInfo.RedirectStandardOutput)
			{
				string text = process.StandardOutput.ReadLine();
				if (!string.IsNullOrEmpty(text) && text.TrimStart().StartsWith("CFBundleShortVersionString"))
				{
					string[] array = text.Split('"');
					if (array.Length >= 2)
					{
						result = array[1];
					}
				}
			}
			if (process.StartInfo.RedirectStandardError)
			{
				string text2 = process.StandardError.ReadToEnd();
				if (!string.IsNullOrEmpty(text2))
				{
					Log.LogError(typeof(InstalledCheck), text2);
				}
			}
			return result;
		}
	}
}
