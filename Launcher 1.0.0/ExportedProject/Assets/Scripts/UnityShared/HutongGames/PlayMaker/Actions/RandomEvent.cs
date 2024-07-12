using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends a Random State Event after an optional delay. Use this to transition to a random state from the current state.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class RandomEvent : FsmStateAction
	{
		[Tooltip("Delay before sending the event.")]
		[HasFloatSlider(0f, 10f)]
		public FsmFloat delay;

		[Tooltip("Don't repeat the same event twice in a row.")]
		public FsmBool noRepeat;

		private DelayedEvent delayedEvent;

		private int randomEventIndex;

		private int lastEventIndex = -1;

		public override void Reset()
		{
			delay = null;
		}

		public override void OnEnter()
		{
			if (base.State.Transitions.Length != 0)
			{
				if (lastEventIndex == -1)
				{
					lastEventIndex = Random.Range(0, base.State.Transitions.Length);
				}
				if (delay.Value < 0.001f)
				{
					base.Fsm.Event(GetRandomEvent());
					Finish();
				}
				else
				{
					delayedEvent = base.Fsm.DelayedEvent(GetRandomEvent(), delay.Value);
				}
			}
		}

		public override void OnUpdate()
		{
			if (DelayedEvent.WasSent(delayedEvent))
			{
				Finish();
			}
		}

		private FsmEvent GetRandomEvent()
		{
			do
			{
				randomEventIndex = Random.Range(0, base.State.Transitions.Length);
			}
			while (noRepeat.Value && base.State.Transitions.Length > 1 && randomEventIndex == lastEventIndex);
			lastEventIndex = randomEventIndex;
			return base.State.Transitions[randomEventIndex].FsmEvent;
		}
	}
}
