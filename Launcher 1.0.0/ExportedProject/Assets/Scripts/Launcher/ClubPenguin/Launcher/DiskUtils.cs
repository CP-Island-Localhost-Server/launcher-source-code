using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ClubPenguin.Launcher
{
	public static class DiskUtils
	{
		private const string DEFAULT_DRIVE = "C:\\";

		[DllImport("DiskUtilsWinAPI")]
		private static extern int getAvailableDiskSpace([MarshalAs(UnmanagedType.LPWStr)] string drive);

		[DllImport("DiskUtilsWinAPI")]
		private static extern int getTotalDiskSpace([MarshalAs(UnmanagedType.LPWStr)] string drive);

		[DllImport("DiskUtilsWinAPI")]
		private static extern int getTotalFreeDiskSpace([MarshalAs(UnmanagedType.LPWStr)] string drive);

		public static bool IsDefaultDrive(string drive)
		{
			if (!string.IsNullOrEmpty(drive) && 0 == string.Compare("C:\\", 0, drive, 0, "C:\\".Length - 1, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			return false;
		}

		public static int GetAvailableSpace(string drive = "C:\\")
		{
			return getAvailableDiskSpace(drive);
		}

		public static int GetTotalSpace(string drive = "C:\\")
		{
			return getTotalDiskSpace(drive);
		}

		public static int GetBusySpace(string drive = "C:\\")
		{
			return getTotalFreeDiskSpace(drive);
		}

		public static string[] GetDriveNames()
		{
			return Directory.GetLogicalDrives();
		}
	}
}
