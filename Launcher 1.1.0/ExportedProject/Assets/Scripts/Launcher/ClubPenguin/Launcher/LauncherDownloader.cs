using System;
using System.IO;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class LauncherDownloader : IDisposable
	{
		private readonly InstallerManifestEntry latestLauncherEntry;

		private bool isDownloadIssCompleted = false;

		private bool isDownloadIssSuccessful = false;

		private bool isDownloadInstallerCompleted = false;

		private bool isDownloadInstallerSuccessful = false;

		private CdnGet2 cdnGetIss;

		private CdnGet2 cdnGetInstaller;

		private bool disposed = false;

		public LauncherDownloader(InstallerManifestEntry latestLauncherEntry)
		{
			this.latestLauncherEntry = latestLauncherEntry;
		}

		public void Run()
		{
			isDownloadIssCompleted = false;
			isDownloadIssSuccessful = false;
			isDownloadInstallerCompleted = false;
			isDownloadInstallerSuccessful = false;
			Service.Get<EventDispatcher>().AddListener<AppWillQuitEvent>(onAppWillQuit);
			if (latestLauncherEntry == null)
			{
				Log.LogError(this, "LauncherDownloader.Run() - GetLatestLauncherEntry() returned null!");
				isDownloadIssCompleted = true;
				isDownloadInstallerCompleted = true;
				return;
			}
			if (string.IsNullOrEmpty(latestLauncherEntry.InstallResponseFileUrl))
			{
				isDownloadIssCompleted = true;
			}
			else
			{
				string launcherInstallResponseFileLocation = LauncherPaths.GetLauncherInstallResponseFileLocation();
				cdnGetIss = new CdnGet2(latestLauncherEntry.InstallResponseFileUrl, launcherInstallResponseFileLocation, downloadIssCompleted);
				cdnGetIss.TimeoutSeconds = 0;
				cdnGetIss.Execute();
			}
			string launcherDownloadLocation = LauncherPaths.GetLauncherDownloadLocation();
			if (File.Exists(launcherDownloadLocation) && !string.IsNullOrEmpty(latestLauncherEntry.ContentHash))
			{
				string a = ContentHash.CalculateHashForFile(false, launcherDownloadLocation);
				if (string.Equals(a, latestLauncherEntry.ContentHash, StringComparison.Ordinal))
				{
					isDownloadInstallerCompleted = true;
				}
			}
			if (!isDownloadInstallerCompleted)
			{
				cdnGetInstaller = new CdnGet2(latestLauncherEntry.InstallerUrl, launcherDownloadLocation, downloadInstallerCompleted);
				cdnGetInstaller.TimeoutSeconds = 0;
				cdnGetInstaller.Execute();
			}
		}

		public bool AreDownloadsCompleted()
		{
			return isDownloadIssCompleted && isDownloadInstallerCompleted;
		}

		public bool AreDownloadsSuccessful()
		{
			return isDownloadIssSuccessful && isDownloadInstallerSuccessful;
		}

		private void downloadIssCompleted(bool success, string filename, string errorMessage)
		{
			try
			{
				if (success)
				{
					if (File.Exists(filename))
					{
						isDownloadIssSuccessful = true;
						return;
					}
					Log.LogErrorFormatted(this, "Error downloading Launcher Response file: Downloaded file not found at path: {0}", filename);
				}
				else
				{
					Log.LogErrorFormatted(this, "Error downloading Launcher Response file: {0}", errorMessage);
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				isDownloadIssCompleted = true;
			}
		}

		private void downloadInstallerCompleted(bool success, string filename, string errorMessage)
		{
			try
			{
				if (!success)
				{
					Log.LogErrorFormatted(this, "Error downloading Launcher Installer: {0}", errorMessage);
				}
				else if (File.Exists(filename))
				{
					if (string.IsNullOrEmpty(latestLauncherEntry.ContentHash))
					{
						isDownloadInstallerSuccessful = true;
						return;
					}
					string text = ContentHash.CalculateHashForFile(false, filename);
					if (string.Equals(text, latestLauncherEntry.ContentHash, StringComparison.Ordinal))
					{
						isDownloadInstallerSuccessful = true;
						return;
					}
					Log.LogErrorFormatted(this, "Error downloading Launcher: File content hash does not match.\n Manifest entry hash: {0}, '{1}' hash: {2}, ", latestLauncherEntry.ContentHash, filename, text);
				}
				else
				{
					Log.LogErrorFormatted(this, "Error downloading Launcher: Downloaded file not found at path: {0}", filename);
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				isDownloadInstallerCompleted = true;
			}
		}

		private bool onAppWillQuit(AppWillQuitEvent evt)
		{
			Dispose();
			return false;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}
			if (disposing)
			{
				if (cdnGetIss != null)
				{
					cdnGetIss.Dispose();
					cdnGetIss = null;
				}
				if (cdnGetInstaller != null)
				{
					cdnGetInstaller.Dispose();
					cdnGetInstaller = null;
				}
			}
			Service.Get<EventDispatcher>().RemoveListener<AppWillQuitEvent>(onAppWillQuit);
			disposed = true;
		}
	}
}
