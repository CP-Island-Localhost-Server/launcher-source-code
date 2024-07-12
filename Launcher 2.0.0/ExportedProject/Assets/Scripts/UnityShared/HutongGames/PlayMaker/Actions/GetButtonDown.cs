using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when a Button is pressed.")]
	[ActionCategory(ActionCategory.Input)]
	public class GetButtonDown : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The name of the button. Set in the Unity Input Manager.")]
		public FsmString buttonName;

		[Tooltip("Event to send if the button is pressed.")]
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Set to True if the button is pressed.")]
		public FsmBool storeResult;

		public override void Reset()
		{
			buttonName = "Fire1";
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool buttonDown = Input.GetButtonDown(buttonName.Value);
			if (buttonDown)
			{
				base.Fsm.Event(sendEvent);
			}
			storeResult.Value = buttonDown;
		}
	}
}
