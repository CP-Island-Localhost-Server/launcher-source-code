namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts a Bool value to an Integer value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToInt : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Bool variable to test.")]
		[RequiredField]
		public FsmBool boolVariable;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Integer variable to set based on the Bool variable value.")]
		public FsmInt intVariable;

		[Tooltip("Integer value if Bool variable is false.")]
		public FsmInt falseValue;

		[Tooltip("Integer value if Bool variable is false.")]
		public FsmInt trueValue;

		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			intVariable = null;
			falseValue = 0;
			trueValue = 1;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToInt();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertBoolToInt();
		}

		private void DoConvertBoolToInt()
		{
			intVariable.Value = (boolVariable.Value ? trueValue.Value : falseValue.Value);
		}
	}
}
