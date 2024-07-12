namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Get the vector3 from a quaternion multiplied by a vector.")]
	public class GetQuaternionMultipliedByVector : QuaternionBaseAction
	{
		[RequiredField]
		[Tooltip("The quaternion to multiply")]
		public FsmQuaternion quaternion;

		[RequiredField]
		[Tooltip("The vector3 to multiply")]
		public FsmVector3 vector3;

		[RequiredField]
		[Tooltip("The resulting vector3")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 result;

		public override void Reset()
		{
			quaternion = null;
			vector3 = null;
			result = null;
			everyFrame = false;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatMult();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatMult();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatMult();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatMult();
			}
		}

		private void DoQuatMult()
		{
			result.Value = quaternion.Value * vector3.Value;
		}
	}
}
