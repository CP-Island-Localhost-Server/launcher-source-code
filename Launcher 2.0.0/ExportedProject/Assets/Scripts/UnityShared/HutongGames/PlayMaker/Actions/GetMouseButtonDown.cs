using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when the specified Mouse Button is pressed. Optionally store the button state in a bool variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButtonDown : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The mouse button to test.")]
		public MouseButton button;

		[Tooltip("Event to send if the mouse button is down.")]
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the button state in a Bool Variable.")]
		public FsmBool storeResult;

		[Tooltip("Uncheck to run when entering the state.")]
		public bool inUpdateOnly;

		public override void Reset()
		{
			button = MouseButton.Left;
			sendEvent = null;
			storeResult = null;
			inUpdateOnly = true;
		}

		public override void OnEnter()
		{
			if (!inUpdateOnly)
			{
				DoGetMouseButtonDown();
			}
		}

		public override void OnUpdate()
		{
			DoGetMouseButtonDown();
		}

		private void DoGetMouseButtonDown()
		{
			bool mouseButtonDown = Input.GetMouseButtonDown((int)button);
			if (mouseButtonDown)
			{
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = mouseButtonDown;
		}
	}
}
