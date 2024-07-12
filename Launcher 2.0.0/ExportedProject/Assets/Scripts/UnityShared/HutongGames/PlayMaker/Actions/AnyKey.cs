using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when the user hits any Key or Mouse Button.")]
	[ActionCategory(ActionCategory.Input)]
	public class AnyKey : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Event to send when any Key or Mouse Button is pressed.")]
		public FsmEvent sendEvent;

		public override void Reset()
		{
			sendEvent = null;
		}

		public override void OnUpdate()
		{
			if (Input.anyKeyDown)
			{
				base.Fsm.Event(sendEvent);
			}
		}
	}
}
