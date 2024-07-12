using System.Collections;
using System.IO;

namespace ClubPenguin.Launcher
{
	public class DeleteClientInstallerAction : LauncherAction
	{
		public override IEnumerator Run()
		{
			File.Delete(LauncherPaths.GetClientDownloadLocation());
			yield break;
		}

		public override float GetProgress()
		{
			return 1f;
		}
	}
}
