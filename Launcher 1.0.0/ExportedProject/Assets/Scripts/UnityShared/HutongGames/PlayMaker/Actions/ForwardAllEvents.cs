namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Forwards all event received by this FSM to another target. Optionally specify a list of events to ignore.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class ForwardAllEvents : FsmStateAction
	{
		[Tooltip("Forward to this target.")]
		public FsmEventTarget forwardTo;

		[Tooltip("Don't forward these events.")]
		public FsmEvent[] exceptThese;

		[Tooltip("Should this action eat the events or pass them on.")]
		public bool eatEvents;

		public override void Reset()
		{
			forwardTo = new FsmEventTarget
			{
				target = FsmEventTarget.EventTarget.FSMComponent
			};
			exceptThese = new FsmEvent[1] { FsmEvent.Finished };
			eatEvents = true;
		}

		public override bool Event(FsmEvent fsmEvent)
		{
			if (exceptThese != null)
			{
				FsmEvent[] array = exceptThese;
				foreach (FsmEvent fsmEvent2 in array)
				{
					if (fsmEvent2 == fsmEvent)
					{
						return false;
					}
				}
			}
			base.Fsm.Event(forwardTo, fsmEvent);
			return eatEvents;
		}
	}
}
