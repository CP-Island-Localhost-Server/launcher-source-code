namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Kill all queued delayed events. Normally delayed events are automatically killed when the active state is exited, but you can override this behaviour in FSM settings. If you choose to keep delayed events you can use this action to kill them when needed.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[Note("Kill all queued delayed events.")]
	public class KillDelayedEvents : FsmStateAction
	{
		public override void OnEnter()
		{
			base.Fsm.KillDelayedEvents();
			Finish();
		}
	}
}
