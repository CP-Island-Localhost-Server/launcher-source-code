namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Join an array of strings into a single string.")]
	public class StringJoin : FsmStateAction
	{
		[ArrayEditor(VariableType.String, "", 0, 0, 65536)]
		[Tooltip("Array of string to join into a single string.")]
		[UIHint(UIHint.Variable)]
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
