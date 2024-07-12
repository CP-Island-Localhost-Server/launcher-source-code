using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends an Event when the mobile device is shaken.")]
	[ActionCategory(ActionCategory.Device)]
	public class DeviceShakeEvent : FsmStateAction
	{
		[Tooltip("Amount of acceleration required to trigger the event. Higher numbers require a harder shake.")]
		[RequiredField]
		public FsmFloat shakeThreshold;

		[RequiredField]
		[Tooltip("Event to send when Shake Threshold is exceded.")]
		public FsmEvent sendEvent;

		public override void Reset()
		{
			shakeThreshold = 3f;
			sendEvent = null;
		}

		public override void OnUpdate()
		{
			if (Input.acceleration.sqrMagnitude > shakeThreshold.Value * shakeThreshold.Value)
			{
				base.Fsm.Event(sendEvent);
			}
		}
	}
}
