using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Interpolates between 2 Vector3 values over a specified Time.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Interpolate : FsmStateAction
	{
		public InterpolationType mode;

		[RequiredField]
		public FsmVector3 fromVector;

		[RequiredField]
		public FsmVector3 toVector;

		[RequiredField]
		public FsmFloat time;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 storeResult;

		public FsmEvent finishEvent;

		[Tooltip("Ignore TimeScale")]
		public bool realTime;

		private float startTime;

		private float currentTime;

		public override void Reset()
		{
			mode = InterpolationType.Linear;
			fromVector = new FsmVector3
			{
				UseVariable = true
			};
			toVector = new FsmVector3
			{
				UseVariable = true
			};
			time = 1f;
			storeResult = null;
			finishEvent = null;
			realTime = false;
		}

		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			currentTime = 0f;
			if (storeResult == null)
			{
				Finish();
			}
			else
			{
				storeResult.Value = fromVector.Value;
			}
		}

		public override void OnUpdate()
		{
			if (realTime)
			{
				currentTime = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				currentTime += Time.deltaTime;
			}
			float num = currentTime / time.Value;
			switch (mode)
			{
			case InterpolationType.EaseInOut:
				num = Mathf.SmoothStep(0f, 1f, num);
				break;
			}
			storeResult.Value = Vector3.Lerp(fromVector.Value, toVector.Value, num);
			if (num >= 1f)
			{
				if (finishEvent != null)
				{
					base.Fsm.Event(finishEvent);
				}
				Finish();
			}
		}
	}
}
