namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of an Enum Variable.")]
	[ActionCategory(ActionCategory.Enum)]
	public class SetEnumValue : FsmStateAction
	{
		[Tooltip("The Enum Variable to set.")]
		[UIHint(UIHint.Variable)]
		public FsmEnum enumVariable;

		[MatchFieldType("enumVariable")]
		[Tooltip("The Enum value to set the variable to.")]
		public FsmEnum enumValue;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			enumVariable = null;
			enumValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetEnumValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetEnumValue();
		}

		private void DoSetEnumValue()
		{
			enumVariable.Value = enumValue.Value;
		}
	}
}
