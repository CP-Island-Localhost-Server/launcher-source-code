using System.Collections;
using System.IO;
using Disney.Kelowna.Common;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class CleanUpInstallShieldResponseFilesAction : LauncherAction
	{
		public override IEnumerator Run()
		{
			string clientUninstallResponseFileLocation = LauncherPaths.GetClientUninstallResponseFileLocation();
			string tempLocation = LauncherPaths.GetTempLocation(clientUninstallResponseFileLocation);
			if (File.Exists(tempLocation))
			{
				string text = File.ReadAllText(tempLocation);
				InstallerManifestEntry latestClientEntry = Service.Get<InstallerManifestService>().GetLatestClientEntry();
				string contents = text.Replace("{{VERSION}}", latestClientEntry.Version);
				File.WriteAllText(clientUninstallResponseFileLocation, contents);
				File.Delete(tempLocation);
			}
			yield break;
		}

		public override float GetProgress()
		{
			return 1f;
		}
	}
}
