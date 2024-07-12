using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Gets the Width of the Screen in pixels.")]
	public class GetScreenWidth : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
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
