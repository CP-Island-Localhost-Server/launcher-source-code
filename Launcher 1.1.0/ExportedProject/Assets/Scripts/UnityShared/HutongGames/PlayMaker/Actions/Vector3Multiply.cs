namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Multiplies a Vector3 variable by a Float.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Multiply : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		[RequiredField]
		public FsmFloat multiplyBy;

		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			multiplyBy = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector3Variable.Value *= multiplyBy.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector3Variable.Value *= multiplyBy.Value;
		}
	}
}
