namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Reverses the direction of a Vector2 Variable. Same as multiplying by -1.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Invert : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The vector to invert")]
		[RequiredField]
		public FsmVector2 vector2Variable;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			vector2Variable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector2Variable.Value *= -1f;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector2Variable.Value *= -1f;
		}
	}
}
