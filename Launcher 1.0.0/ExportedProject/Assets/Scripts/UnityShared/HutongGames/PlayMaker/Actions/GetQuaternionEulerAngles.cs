namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Gets a quaternion as euler angles.")]
	public class GetQuaternionEulerAngles : QuaternionBaseAction
	{
		[Tooltip("The rotation")]
		[RequiredField]
		public FsmQuaternion quaternion;

		[UIHint(UIHint.Variable)]
		[Tooltip("The euler angles of the quaternion.")]
		[RequiredField]
		public FsmVector3 eulerAngles;

		public override void Reset()
		{
			quaternion = null;
			eulerAngles = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			GetQuatEuler();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				GetQuatEuler();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				GetQuatEuler();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				GetQuatEuler();
			}
		}

		private void GetQuatEuler()
		{
			eulerAngles.Value = quaternion.Value.eulerAngles;
		}
	}
}
