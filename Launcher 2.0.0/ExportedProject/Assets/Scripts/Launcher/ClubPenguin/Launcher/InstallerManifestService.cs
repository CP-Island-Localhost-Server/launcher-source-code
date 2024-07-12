using System;
using Disney.Kelowna.Common;

namespace ClubPenguin.Launcher
{
	public class InstallerManifestService
	{
		private const string CLIENT_TYPE = "client";

		private const string LAUNCHER_TYPE = "launcher";

		private InstallerManifest installerManifest;

		public bool IsInstallerManifestNull
		{
			get
			{
				return installerManifest == null;
			}
		}

		public event Action OnInstallerManifestUpdated;

		public void UpdateInstallerManifest(InstallerManifest installerManifest)
		{
			this.installerManifest = installerManifest;
			this.OnInstallerManifestUpdated.InvokeSafe();
		}

		public InstallerManifestService(InstallerManifest installerManifest)
		{
			this.installerManifest = installerManifest;
		}

		public InstallerManifestEntry GetLatestClientEntry()
		{
			return getLatestEntry(installerManifest, "client");
		}

		public InstallerManifestEntry GetLatestLauncherEntry()
		{
			return getLatestEntry(installerManifest, "launcher");
		}

		public static InstallerManifestEntry GetLatestClientEntry(InstallerManifest installerManifest)
		{
			return getLatestEntry(installerManifest, "client");
		}

		public static InstallerManifestEntry GetLatestLauncherEntry(InstallerManifest installerManifest)
		{
			return getLatestEntry(installerManifest, "launcher");
		}

		private static InstallerManifestEntry getLatestEntry(InstallerManifest installerManifest, string installerType)
		{
			InstallerManifestEntry installerManifestEntry = null;
			if (installerManifest != null && installerManifest.Entries != null)
			{
				for (int i = 0; i < installerManifest.Entries.Length; i++)
				{
					InstallerManifestEntry installerManifestEntry2 = installerManifest.Entries[i];
					if (installerManifestEntry2.Platform == InstallerManifest.CurrentPlatform && installerManifestEntry2.InstallerId == installerType && (installerManifestEntry == null || installerManifestEntry2.GetVersion() > installerManifestEntry.GetVersion()))
					{
						installerManifestEntry = installerManifestEntry2;
					}
				}
			}
			return installerManifestEntry;
		}
	}
}
