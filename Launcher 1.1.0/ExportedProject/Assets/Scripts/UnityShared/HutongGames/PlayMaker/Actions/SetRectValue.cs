namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of a Rect Variable.")]
	[ActionCategory(ActionCategory.Rect)]
	public class SetRectValue : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmRect rectVariable;

		[RequiredField]
		public FsmRect rectValue;

		public bool everyFrame;

		public override void Reset()
		{
			rectVariable = null;
			rectValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			rectVariable.Value = rectValue.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			rectVariable.Value = rectValue.Value;
		}
	}
}
