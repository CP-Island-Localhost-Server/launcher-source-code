using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Quaternion)]
	[Tooltip("Check if two quaternions are equals or not. Takes in account inversed representations of quaternions")]
	public class QuaternionCompare : QuaternionBaseAction
	{
		[Tooltip("First Quaternion")]
		[RequiredField]
		public FsmQuaternion Quaternion1;

		[RequiredField]
		[Tooltip("Second Quaternion")]
		public FsmQuaternion Quaternion2;

		[Tooltip("true if Quaternions are equal")]
		public FsmBool equal;

		[Tooltip("Event sent if Quaternions are equal")]
		public FsmEvent equalEvent;

		[Tooltip("Event sent if Quaternions are not equal")]
		public FsmEvent notEqualEvent;

		public override void Reset()
		{
			Quaternion1 = new FsmQuaternion
			{
				UseVariable = true
			};
			Quaternion2 = new FsmQuaternion
			{
				UseVariable = true
			};
			equal = null;
			equalEvent = null;
			notEqualEvent = null;
			everyFrameOption = everyFrameOptions.Update;
		}

		public override void OnEnter()
		{
			DoQuatCompare();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (everyFrameOption == everyFrameOptions.Update)
			{
				DoQuatCompare();
			}
		}

		public override void OnLateUpdate()
		{
			if (everyFrameOption == everyFrameOptions.LateUpdate)
			{
				DoQuatCompare();
			}
		}

		public override void OnFixedUpdate()
		{
			if (everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				DoQuatCompare();
			}
		}

		private void DoQuatCompare()
		{
			bool flag = Mathf.Abs(Quaternion.Dot(Quaternion1.Value, Quaternion2.Value)) > 0.999999f;
			equal.Value = flag;
			if (flag)
			{
				base.Fsm.Event(equalEvent);
			}
			else
			{
				base.Fsm.Event(notEqualEvent);
			}
		}
	}
}
