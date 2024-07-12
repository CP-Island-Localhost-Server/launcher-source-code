using Disney.Kelowna.Common;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class LauncherStartStateSetter : MonoBehaviour
	{
		private void Awake()
		{
			Application.runInBackground = true;
			Screen.SetResolution(1024, 768, false);
			AppWindowUtil.StartCustomWindowManager();
		}
	}
}
