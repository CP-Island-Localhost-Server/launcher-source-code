namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get Vector3 Length.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class GetVectorLength : FsmStateAction
	{
		public FsmVector3 vector3;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeLength;

		public override void Reset()
		{
			vector3 = null;
			storeLength = null;
		}

		public override void OnEnter()
		{
			DoVectorLength();
			Finish();
		}

		private void DoVectorLength()
		{
			if (vector3 != null && storeLength != null)
			{
				storeLength.Value = vector3.Value.magnitude;
			}
		}
	}
}
