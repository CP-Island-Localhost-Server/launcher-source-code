using System.Diagnostics;
using System.IO;

namespace ClubPenguin.Launcher
{
	public static class ClientSetupUtil
	{
		public static void CreateMacSymLinks(bool waitForExit, Process shell = null)
		{
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
