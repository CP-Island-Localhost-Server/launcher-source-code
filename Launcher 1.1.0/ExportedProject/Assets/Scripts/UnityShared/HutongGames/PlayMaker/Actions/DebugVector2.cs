namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Logs the value of a Vector2 Variable in the PlayMaker Log Window.")]
	[ActionCategory(ActionCategory.Debug)]
	public class DebugVector2 : FsmStateAction
	{
		[Tooltip("Info, Warning, or Error.")]
		public LogLevel logLevel;

		[Tooltip("Prints the value of a Vector2 variable in the PlayMaker log window.")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		public override void Reset()
		{
			logLevel = LogLevel.Info;
			vector2Variable = null;
		}

		public override void OnEnter()
		{
			string text = "None";
			if (!vector2Variable.IsNone)
			{
				text = vector2Variable.Name + ": " + vector2Variable.Value;
			}
			ActionHelpers.DebugLog(base.Fsm, logLevel, text);
			Finish();
		}
	}
}
