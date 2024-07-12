using System.Diagnostics;
using System.IO;

namespace ClubPenguin.Launcher
{
	public static class ClientSetupUtil
	{
		public static void CreateMacSymLinks(bool waitForExit, Process shell = null)
		{
			if (shell == null)
			{
				shell = ProcessHelper.ExecuteShellCommand(onStandardOutput, onStandardError, true, false, "/bin/bash", "");
				shell.StandardInput.WriteLine("");
			}
			string arg = Path.Combine(LauncherPaths.GetClientFrameworksRelativeLocation(), "ZFGameBrowser.app");
			string text = "ZFGameBrowser.app";
			string arg2 = Path.Combine(LauncherPaths.GetClientFrameworksRelativeLocation(), "Chromium Embedded Framework.framework");
			string text2 = "Chromium Embedded Framework.framework";
			string value = string.Format("ln -s \"{0}\" \"{1}\"", arg, text);
			string value2 = string.Format("ln -s \"{0}\" \"{1}\"", arg2, text2);
			string value3 = string.Format("rm \"{0}\"", text);
			string value4 = string.Format("rm \"{0}\"", text2);
			string launcherFrameworksLocation = LauncherPaths.GetLauncherFrameworksLocation();
			string value5 = string.Format("pushd \"{0}\"", launcherFrameworksLocation);
			shell.StandardInput.WriteLine(value5);
			if (symLinkExists(launcherFrameworksLocation, text))
			{
				shell.StandardInput.WriteLine(value3);
			}
			if (symLinkExists(launcherFrameworksLocation, text2))
			{
				shell.StandardInput.WriteLine(value4);
			}
			shell.StandardInput.WriteLine(value);
			shell.StandardInput.WriteLine(value2);
			shell.StandardInput.WriteLine("popd");
			shell.StandardInput.WriteLine("exit");
			shell.StandardInput.Flush();
			if (waitForExit)
			{
				shell.WaitForExit(15);
			}
		}

		private static void onStandardError(object sender, DataReceivedEventArgs args)
		{
			if (string.IsNullOrEmpty(args.Data))
			{
			}
		}

		private static void onStandardOutput(object sender, DataReceivedEventArgs args)
		{
			if (string.IsNullOrEmpty(args.Data))
			{
			}
		}

		private static bool symLinkExists(string directory, string link)
		{
			string path = Path.Combine(directory, link);
			return File.Exists(path) || Directory.Exists(path);
		}
	}
}
