namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of a Color Variable.")]
	[ActionCategory(ActionCategory.Color)]
	public class SetColorValue : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmColor colorVariable;

		[RequiredField]
		public FsmColor color;

		public bool everyFrame;

		public override void Reset()
		{
			colorVariable = null;
			color = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetColorValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetColorValue();
		}

		private void DoSetColorValue()
		{
			if (colorVariable != null)
			{
				colorVariable.Value = color.Value;
			}
		}
	}
}
