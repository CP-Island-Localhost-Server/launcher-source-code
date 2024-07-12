using System.Collections;

namespace ClubPenguin.Launcher
{
	public class DeleteClientInstallerAction : LauncherAction
	{
		public override IEnumerator Run()
		{
			yield break;
		}

		public override float GetProgress()
		{
			return 1f;
		}
	}
}
