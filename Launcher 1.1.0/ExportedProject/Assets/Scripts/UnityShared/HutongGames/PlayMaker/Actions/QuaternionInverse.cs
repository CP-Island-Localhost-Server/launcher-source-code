using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Inverse a quaternion")]
	[ActionCategory(ActionCategory.Quaternion)]
	public class QuaternionInverse : QuaternionBaseAction
	{
		[Tooltip("the rotation")]
		[RequiredField]
		public FsmQuaternion rotation;

		[RequiredField]
		[Tooltip("Store the inverse of the rotation variable.")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion result;

		public override void Reset()
		{
			rotation = null;
			result = null;
			everyFrame = true;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatInverse();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatInverse();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatInverse();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatInverse();
			}
		}

		private void DoQuatInverse()
		{
			result.Value = Quaternion.Inverse(rotation.Value);
		}
	}
}
