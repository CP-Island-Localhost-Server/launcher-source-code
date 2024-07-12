using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the pressed state of the specified Mouse Button and stores it in a Bool Variable. See Unity Input Manager doc.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButton : FsmStateAction
	{
		[Tooltip("The mouse button to test.")]
		[RequiredField]
		public MouseButton button;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the pressed state in a Bool Variable.")]
		[RequiredField]
		public FsmBool storeResult;

		public override void Reset()
		{
			button = MouseButton.Left;
			storeResult = null;
		}

		public override void OnEnter()
		{
			storeResult.Value = Input.GetMouseButton((int)button);
		}

		public override void OnUpdate()
		{
			storeResult.Value = Input.GetMouseButton((int)button);
		}
	}
}
