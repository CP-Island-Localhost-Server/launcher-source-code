namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the XY channels of a Vector2 Variable and store them in Float Variables.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class GetVector2XY : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The vector2 source")]
		[RequiredField]
		public FsmVector2 vector2Variable;

		[Tooltip("The x component")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		[Tooltip("The y component")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			vector2Variable = null;
			storeX = null;
			storeY = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVector2XYZ();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetVector2XYZ();
		}

		private void DoGetVector2XYZ()
		{
			if (vector2Variable != null)
			{
				if (storeX != null)
				{
					storeX.Value = vector2Variable.Value.x;
				}
				if (storeY != null)
				{
					storeY.Value = vector2Variable.Value.y;
				}
			}
		}
	}
}
