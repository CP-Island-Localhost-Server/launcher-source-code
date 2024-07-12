namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Multiplies a Vector2 variable by a Float.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Multiply : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The vector to Multiply")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		[Tooltip("The multiplication factor")]
		[RequiredField]
		public FsmFloat multiplyBy;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			vector2Variable = null;
			multiplyBy = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector2Variable.Value *= multiplyBy.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector2Variable.Value *= multiplyBy.Value;
		}
	}
}