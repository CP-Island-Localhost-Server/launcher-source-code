using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Obsolete("This action is obsolete; use Send Event with Event Target instead.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends an Event to another Fsm after an optional delay. Specify an Fsm Name or use the first Fsm on the object.")]
	public class SendEventToFsm : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of Fsm on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[UIHint(UIHint.FsmEvent)]
		[RequiredField]
		public FsmString sendEvent;

		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		private bool requireReceiver;

		private GameObject go;

		private DelayedEvent delayedEvent;

		public override void Reset()
		{
			gameObject = null;
			fsmName = null;
			sendEvent = null;
			delay = null;
			requireReceiver = false;
		}

		public override void OnEnter()
		{
			go = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
				Finish();
				return;
			}
			PlayMakerFSM gameObjectFsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			if (gameObjectFsm == null)
			{
				if (requireReceiver)
				{
					LogError("GameObject doesn't have FsmComponent: " + go.name + " " + fsmName.Value);
				}
			}
			else if ((double)delay.Value < 0.001)
			{
				gameObjectFsm.Fsm.Event(sendEvent.Value);
				Finish();
			}
			else
			{
				delayedEvent = gameObjectFsm.Fsm.DelayedEvent(FsmEvent.GetFsmEvent(sendEvent.Value), delay.Value);
			}
		}

		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(delayedEvent))
			{
				Finish();
			}
		}
	}
}
