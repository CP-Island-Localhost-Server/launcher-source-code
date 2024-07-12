namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Forward an event received by this FSM to another target.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class ForwardEvent : FsmStateAction
	{
		[Tooltip("Forward to this target.")]
		public FsmEventTarget forwardTo;

		[Tooltip("The events to forward.")]
		public FsmEvent[] eventsToForward;

		[Tooltip("Should this action eat the events or pass them on.")]
		public bool eatEvents;

		public override void Reset()
		{
			forwardTo = new FsmEventTarget
			{
				target = FsmEventTarget.EventTarget.FSMComponent
			};
			eventsToForward = null;
			eatEvents = true;
		}

		public override bool Event(FsmEvent fsmEvent)
		{
			if (eventsToForward != null)
			{
				FsmEvent[] array = eventsToForward;
				foreach (FsmEvent fsmEvent2 in array)
				{
					if (fsmEvent2 == fsmEvent)
					{
						base.Fsm.Event(forwardTo, fsmEvent);
						return eatEvents;
					}
				}
			}
			return false;
		}
	}
}
