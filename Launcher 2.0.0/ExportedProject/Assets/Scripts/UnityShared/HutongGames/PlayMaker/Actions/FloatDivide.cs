namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Divides one Float by another.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatDivide : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The float variable to divide.")]
		public FsmFloat floatVariable;

		[Tooltip("Divide the float variable by this value.")]
		[RequiredField]
		public FsmFloat divideBy;

		[Tooltip("Repeate every frame. Useful if the variables are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			divideBy = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			floatVariable.Value /= divideBy.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			floatVariable.Value /= divideBy.Value;
		}
	}
}
