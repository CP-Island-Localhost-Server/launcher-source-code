namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of an Object Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugObject : BaseLogAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[UIHint(UIHint.Variable)]
		[Tooltip("The Object variable to debug.")]
		public FsmObject fsmObject;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			fsmObject = null;
			base.Reset();
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!fsmObject.IsNone)
			{
				text = fsmObject.Name + ": " + fsmObject;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text, sendToUnityLog);
			Finish();
		}
	}
}
