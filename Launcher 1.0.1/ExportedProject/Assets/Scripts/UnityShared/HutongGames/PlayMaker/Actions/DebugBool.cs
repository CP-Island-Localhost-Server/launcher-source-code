namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of a Bool Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugBool : BaseLogAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("The Bool variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			boolVariable = null;
			base.Reset();
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!boolVariable.IsNone)
			{
				text = boolVariable.Name + ": " + boolVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text, sendToUnityLog);
			Finish();
		}
	}
}
