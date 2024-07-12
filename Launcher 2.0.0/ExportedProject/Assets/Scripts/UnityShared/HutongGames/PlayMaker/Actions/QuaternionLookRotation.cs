using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Creates a rotation that looks along forward with the the head upwards along upwards.")]
	public class QuaternionLookRotation : QuaternionBaseAction
	{
		[RequiredField]
		[Tooltip("the rotation direction")]
		public FsmVector3 direction;

		[Tooltip("The up direction")]
		public FsmVector3 upVector;

		[Tooltip("Store the inverse of the rotation variable.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;

		public override void Reset()
		{
			direction = null;
			upVector = new FsmVector3
			{
				UseVariable = true
			};
			result = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatLookRotation();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatLookRotation();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatLookRotation();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatLookRotation();
			}
		}

		private void DoQuatLookRotation()
		{
			if (!upVector.IsNone)
			{
				result.Value = Quaternion.LookRotation(direction.Value, upVector.Value);
			}
			else
			{
				result.Value = Quaternion.LookRotation(direction.Value);
			}
		}
	}
}
