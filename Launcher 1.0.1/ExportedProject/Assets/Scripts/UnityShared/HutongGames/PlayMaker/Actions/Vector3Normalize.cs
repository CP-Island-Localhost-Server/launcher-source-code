namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Normalizes a Vector3 Variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Normalize : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			vector3Variable.Value = vector3Variable.Value.normalized;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector3Variable.Value = vector3Variable.Value.normalized;
		}
	}
}
