using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Time)]
	[Tooltip("Delays a State from finishing by a random time. NOTE: Other actions continue, but FINISHED can't happen before Time.")]
	public class RandomWait : FsmStateAction
	{
		[Tooltip("Minimum amount of time to wait.")]
		[RequiredField]
		public FsmFloat min;

		[Tooltip("Maximum amount of time to wait.")]
		[RequiredField]
		public FsmFloat max;

		[Tooltip("Event to send when timer is finished.")]
		public FsmEvent finishEvent;

		[Tooltip("Ignore time scale.")]
		public bool realTime;

		private float startTime;

		private float timer;

		private float time;

		public override void Reset()
		{
			min = 0f;
			max = 1f;
			finishEvent = null;
			realTime = false;
		}

		public override void OnEnter()
		{
			time = Random.Range(min.Value, max.Value);
			if (time <= 0f)
			{
				base.Fsm.Event(finishEvent);
				Finish();
			}
			else
			{
				startTime = FsmTime.RealtimeSinceStartup;
				timer = 0f;
			}
		}

		public override void OnUpdate()
		{
			if (realTime)
			{
				timer = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				timer += Time.deltaTime;
			}
			if (timer >= time)
			{
				Finish();
				if (finishEvent != null)
				{
					base.Fsm.Event(finishEvent);
				}
			}
		}
	}
}
