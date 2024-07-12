using System.Collections;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(InitContentSystemAction))]
	[RequireComponent(typeof(InitLocalizerSetupAction))]
	public class InitPopupManagerAction : InitActionComponent
	{
		public GameObject PopupCanvas;

		public LauncherPromptManager LauncherPromptManager;

		public override bool HasSecondPass
		{
			get
			{
				return false;
			}
		}

		public override bool HasCompletedPass
		{
			get
			{
				return false;
			}
		}

		public override IEnumerator PerformFirstPass()
		{
			LauncherPopupManager instance = PopupCanvas.AddComponent<LauncherPopupManager>();
			Service.Set(instance);
			Service.Set(LauncherPromptManager);
			yield break;
		}
	}
}
