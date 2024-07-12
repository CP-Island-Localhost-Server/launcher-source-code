namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the XYZ channels of a Vector3 Variable and store them in Float Variables.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class GetVector3XYZ : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeX;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeY;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeZ;

		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			storeX = null;
			storeY = null;
			storeZ = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVector3XYZ();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetVector3XYZ();
		}

		private void DoGetVector3XYZ()
		{
			if (vector3Variable != null)
			{
				if (storeX != null)
				{
					storeX.Value = vector3Variable.Value.x;
				}
				if (storeY != null)
				{
					storeY.Value = vector3Variable.Value.y;
				}
				if (storeZ != null)
				{
					storeZ.Value = vector3Variable.Value.z;
				}
			}
		}
	}
}
