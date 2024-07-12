using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when the specified Mouse Button is released. Optionally store the button state in a bool variable.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetMouseButtonUp : FsmStateAction
	{
		[Tooltip("The mouse button to test.")]
		[RequiredField]
		public MouseButton button;

		[Tooltip("Event to send if the mouse button is down.")]
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the pressed state in a Bool Variable.")]
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
				DoGetMouseButtonUp();
			}
		}

		public override void OnUpdate()
		{
			DoGetMouseButtonUp();
		}

		public void DoGetMouseButtonUp()
		{
			bool mouseButtonUp = Input.GetMouseButtonUp((int)button);
			if (mouseButtonUp)
			{
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = mouseButtonUp;
		}
	}
}
