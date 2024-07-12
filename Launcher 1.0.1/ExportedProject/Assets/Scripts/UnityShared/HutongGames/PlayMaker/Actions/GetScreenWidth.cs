using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Width of the Screen in pixels.")]
	[ActionCategory(ActionCategory.Application)]
	public class GetScreenWidth : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeScreenWidth;

		public override void Reset()
		{
			storeScreenWidth = null;
		}

		public override void OnEnter()
		{
			storeScreenWidth.Value = Screen.width;
			Finish();
		}
	}
}
