namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts a Bool value to a Float value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToFloat : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Bool variable to test.")]
		[UIHint(UIHint.Variable)]
		public FsmBool boolVariable;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The Float variable to set based on the Bool variable value.")]
		public FsmFloat floatVariable;

		[Tooltip("Float value if Bool variable is false.")]
		public FsmFloat falseValue;

		[Tooltip("Float value if Bool variable is true.")]
		public FsmFloat trueValue;

		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			floatVariable = null;
			falseValue = 0f;
			trueValue = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToFloat();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertBoolToFloat();
		}

		private void DoConvertBoolToFloat()
		{
			floatVariable.Value = (boolVariable.Value ? trueValue.Value : falseValue.Value);
		}
	}
}
