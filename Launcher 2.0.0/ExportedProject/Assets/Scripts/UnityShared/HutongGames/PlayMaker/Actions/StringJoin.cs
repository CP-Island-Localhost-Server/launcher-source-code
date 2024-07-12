namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Join an array of strings into a single string.")]
	[ActionCategory(ActionCategory.String)]
	public class StringJoin : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Array of string to join into a single string.")]
		[ArrayEditor(VariableType.String, "", 0, 0, 65536)]
		public FsmArray stringArray;

		[Tooltip("Seperator to add between each string.")]
		public FsmString separator;

		[Tooltip("Store the joined string in string variable.")]
		[UIHint(UIHint.Variable)]
		public FsmString storeResult;

		public override void OnEnter()
		{
			if (!stringArray.IsNone && !storeResult.IsNone)
			{
				storeResult.Value = string.Join(separator.Value, stringArray.stringValues);
			}
			Finish();
		}
	}
}
