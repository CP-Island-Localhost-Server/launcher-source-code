namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts an Integer value to a Float value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertIntToFloat : FsmStateAction
	{
		[Tooltip("The Integer variable to convert to a float.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a Float variable.")]
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
