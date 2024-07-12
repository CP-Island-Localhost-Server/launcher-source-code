using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Height of the Screen in pixels.")]
	[ActionCategory(ActionCategory.Application)]
	public class GetScreenHeight : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
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
