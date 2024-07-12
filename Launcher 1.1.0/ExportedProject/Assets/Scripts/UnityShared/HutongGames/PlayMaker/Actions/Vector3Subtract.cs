namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Subtracts a Vector3 value from a Vector3 variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Subtract : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		[RequiredField]
		public FsmVector3 subtractVector;

		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			subtractVector = new FsmVector3
			{
				UseVariable = true
			};
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector3Variable.Value -= subtractVector.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector3Variable.Value -= subtractVector.Value;
		}
	}
}
