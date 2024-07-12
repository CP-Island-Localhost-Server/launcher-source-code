namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts an Integer value to a Float value.")]
	public class ConvertIntToFloat : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Integer variable to convert to a float.")]
		[RequiredField]
		public FsmInt intVariable;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a Float variable.")]
		[RequiredField]
		public FsmFloat floatVariable;

		[Tooltip("Repeat every frame. Useful if the Integer variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			intVariable = null;
			floatVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertIntToFloat();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertIntToFloat();
		}

		private void DoConvertIntToFloat()
		{
			floatVariable.Value = intVariable.Value;
		}
	}
}
