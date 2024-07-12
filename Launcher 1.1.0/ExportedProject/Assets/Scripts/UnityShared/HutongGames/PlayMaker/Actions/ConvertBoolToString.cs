namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts a Bool value to a String value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToString : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Bool variable to test.")]
		[RequiredField]
		public FsmBool boolVariable;

		[Tooltip("The String variable to set based on the Bool variable value.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		[Tooltip("String value if Bool variable is false.")]
		public FsmString falseString;

		[Tooltip("String value if Bool variable is true.")]
		public FsmString trueString;

		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			stringVariable = null;
			falseString = "False";
			trueString = "True";
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertBoolToString();
		}

		private void DoConvertBoolToString()
		{
			stringVariable.Value = (boolVariable.Value ? trueString.Value : falseString.Value);
		}
	}
}
