using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Gets the Height of the Screen in pixels.")]
	public class GetScreenHeight : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeScreenHeight;

		public override void Reset()
		{
			storeScreenHeight = null;
		}

		public override void OnEnter()
		{
			storeScreenHeight.Value = Screen.height;
			Finish();
		}
	}
}
