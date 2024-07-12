using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Creates a rotation which rotates from fromDirection to toDirection. Usually you use this to rotate a transform so that one of its axes eg. the y-axis - follows a target direction toDirection in world space.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class GetQuaternionFromRotation : QuaternionBaseAction
	{
		[RequiredField]
		[Tooltip("the 'from' direction")]
		public FsmVector3 fromDirection;

		[Tooltip("the 'to' direction")]
		[RequiredField]
		public FsmVector3 toDirection;

		[Tooltip("the resulting quaternion")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;

		public override void Reset()
		{
			fromDirection = null;
			toDirection = null;
			result = null;
			everyFrame = false;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatFromRotation();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatFromRotation();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatFromRotation();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatFromRotation();
			}
		}

		private void DoQuatFromRotation()
		{
			result.Value = Quaternion.FromToRotation(fromDirection.Value, toDirection.Value);
		}
	}
}
