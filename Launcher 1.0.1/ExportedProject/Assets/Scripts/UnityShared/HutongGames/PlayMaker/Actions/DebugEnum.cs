namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of an Enum Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugEnum : BaseLogAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("The Enum Variable to debug.")]
		[UIHint(UIHint.Variable)]
		public FsmEnum enumVariable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			enumVariable = null;
			base.Reset();
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!enumVariable.IsNone)
			{
				text = enumVariable.Name + ": " + enumVariable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text, sendToUnityLog);
			Finish();
		}
	}
}
