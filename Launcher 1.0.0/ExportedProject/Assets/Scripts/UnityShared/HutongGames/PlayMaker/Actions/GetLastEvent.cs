namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the event that caused the transition to the current state, and stores it in a String Variable.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetLastEvent : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmString storeEvent;

		public override void Reset()
		{
			storeEvent = null;
		}

		public override void OnEnter()
		{
			storeEvent.Value = ((base.Fsm.LastTransition == null) ? "START" : base.Fsm.LastTransition.EventName);
			Finish();
		}
	}
}
