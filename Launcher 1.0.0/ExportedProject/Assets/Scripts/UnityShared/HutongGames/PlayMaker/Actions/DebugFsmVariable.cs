namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Print the value of any FSM Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugFsmVariable : BaseLogAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[HideTypeFilter]
		[Tooltip("The variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmVar variable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			variable = null;
			base.Reset();
		}

		public override void OnEnter()
		{
			ActionHelpers.DebugLog(base.Fsm, logLevel, variable.DebugString(), sendToUnityLog);
			Finish();
		}
	}
}
