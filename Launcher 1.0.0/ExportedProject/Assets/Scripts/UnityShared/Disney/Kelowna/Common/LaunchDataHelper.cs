using System.IO;

namespace Disney.Kelowna.Common
{
	public static class LaunchDataHelper
	{
		public static string GetLaunchDataWritePath(string installLocation)
		{
			string text = null;
			return Path.Combine(installLocation, "launch_data.json");
		}

		public static string GetLaunchDataReadPath()
		{
			string text = null;
			string currentDirectory = Directory.GetCurrentDirectory();
			return Path.Combine(currentDirectory, "launch_data.json");
		}
	}
}
