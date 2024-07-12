namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the target FSM for all subsequent events sent by this state. The default 'Self' sends events to this FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetEventTarget : FsmStateAction
	{
		public FsmEventTarget eventTarget;

		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			base.Fsm.EventTarget = eventTarget;
			Finish();
		}
	}
}
