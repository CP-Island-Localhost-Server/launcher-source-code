namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Subtracts a Vector2 value from a Vector2 variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Subtract : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Vector2 operand")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		[Tooltip("The vector2 to substract with")]
		[RequiredField]
		public FsmVector2 subtractVector;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			vector2Variable = null;
			subtractVector = new FsmVector2
			{
				UseVariable = true
			};
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector2Variable.Value -= subtractVector.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector2Variable.Value -= subtractVector.Value;
		}
	}
}
