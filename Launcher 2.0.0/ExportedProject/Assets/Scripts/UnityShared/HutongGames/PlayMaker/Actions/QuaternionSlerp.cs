using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Spherically interpolates between from and to by t.")]
	public class QuaternionSlerp : QuaternionBaseAction
	{
		[Tooltip("From Quaternion.")]
		[RequiredField]
		public FsmQuaternion fromQuaternion;

		[RequiredField]
		[Tooltip("To Quaternion.")]
		public FsmQuaternion toQuaternion;

		[HasFloatSlider(0f, 1f)]
		[RequiredField]
		[Tooltip("Interpolate between fromQuaternion and toQuaternion by this amount. Value is clamped to 0-1 range. 0 = fromQuaternion; 1 = toQuaternion; 0.5 = half way between.")]
		public FsmFloat amount;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in this quaternion variable.")]
		[RequiredField]
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
			amount = 0.1f;
			storeResult = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatSlerp();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatSlerp();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatSlerp();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatSlerp();
			}
		}

		private void DoQuatSlerp()
		{
			storeResult.Value = Quaternion.Slerp(fromQuaternion.Value, toQuaternion.Value, amount.Value);
		}
	}
}
