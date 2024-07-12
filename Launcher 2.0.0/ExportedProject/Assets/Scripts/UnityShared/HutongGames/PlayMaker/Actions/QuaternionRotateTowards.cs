using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rotates a rotation from towards to. This is essentially the same as Quaternion.Slerp but instead the function will ensure that the angular speed never exceeds maxDegreesDelta. Negative values of maxDegreesDelta pushes the rotation away from to.")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionRotateTowards : QuaternionBaseAction
	{
		[Tooltip("From Quaternion.")]
		[RequiredField]
		public FsmQuaternion fromQuaternion;

		[Tooltip("To Quaternion.")]
		[RequiredField]
		public FsmQuaternion toQuaternion;

		[RequiredField]
		[Tooltip("The angular speed never exceeds maxDegreesDelta. Negative values of maxDegreesDelta pushes the rotation away from to.")]
		public FsmFloat maxDegreesDelta;

		[Tooltip("Store the result in this quaternion variable.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion storeResult;

		public override void Reset()
		{
			fromQuaternion = new FsmQuaternion
			{
				UseVariable = true
			};
			toQuaternion = new FsmQuaternion
			{
				UseVariable = true
			};
			maxDegreesDelta = 10f;
			storeResult = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatRotateTowards();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatRotateTowards();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatRotateTowards();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatRotateTowards();
			}
		}

		private void DoQuatRotateTowards()
		{
			storeResult.Value = Quaternion.RotateTowards(fromQuaternion.Value, toQuaternion.Value, maxDegreesDelta.Value);
		}
	}
}
