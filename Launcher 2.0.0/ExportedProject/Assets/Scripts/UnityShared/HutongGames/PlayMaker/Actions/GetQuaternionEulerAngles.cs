namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets a quaternion as euler angles.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class GetQuaternionEulerAngles : QuaternionBaseAction
	{
		[Tooltip("The rotation")]
		[RequiredField]
		public FsmQuaternion quaternion;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The euler angles of the quaternion.")]
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
