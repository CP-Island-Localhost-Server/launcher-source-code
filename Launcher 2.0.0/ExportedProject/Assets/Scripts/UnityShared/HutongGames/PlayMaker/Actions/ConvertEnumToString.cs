namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts an Enum value to a String value.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertEnumToString : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Enum variable to convert.")]
		[UIHint(UIHint.Variable)]
		public FsmEnum enumVariable;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The String variable to store the converted value.")]
		public FsmString stringVariable;

		[Tooltip("Repeat every frame. Useful if the Enum variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			enumVariable = null;
			stringVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertEnumToString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertEnumToString();
		}

		private void DoConvertEnumToString()
		{
			stringVariable.Value = ((enumVariable.Value != null) ? enumVariable.Value.ToString() : "");
		}
	}
}
