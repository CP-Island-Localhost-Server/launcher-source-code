using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets if the Application should play in the background. Useful for servers or testing network games on one machine.")]
	[ActionCategory(ActionCategory.Application)]
	public class ApplicationRunInBackground : FsmStateAction
	{
		public FsmBool runInBackground;

		public override void Reset()
		{
			runInBackground = true;
		}

		public override void OnEnter()
		{
			Application.runInBackground = runInBackground.Value;
			Finish();
		}
	}
}
