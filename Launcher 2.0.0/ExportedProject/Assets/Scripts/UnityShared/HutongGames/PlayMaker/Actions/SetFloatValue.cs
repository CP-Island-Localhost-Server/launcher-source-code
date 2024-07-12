namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Sets the value of a Float Variable.")]
	public class SetFloatValue : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmFloat floatValue;

		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			floatValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value = floatValue.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			floatVariable.Value = floatValue.Value;
		}
	}
}
