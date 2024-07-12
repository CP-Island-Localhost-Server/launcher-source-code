namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Reverses the direction of a Vector3 Variable. Same as multiplying by -1.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Invert : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector3Variable.Value *= -1f;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector3Variable.Value *= -1f;
		}
	}
}
