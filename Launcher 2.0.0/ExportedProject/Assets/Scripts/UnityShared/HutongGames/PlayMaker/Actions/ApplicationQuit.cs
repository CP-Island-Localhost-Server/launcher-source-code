using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Quits the player application.")]
	[ActionCategory(ActionCategory.Application)]
	public class ApplicationQuit : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			Application.Quit();
			Finish();
		}
	}
}
