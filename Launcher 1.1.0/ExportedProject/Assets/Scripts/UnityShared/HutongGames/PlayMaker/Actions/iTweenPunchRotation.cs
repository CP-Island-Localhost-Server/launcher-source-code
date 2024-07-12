using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("iTween")]
	[Tooltip("Applies a jolt of force to a GameObject's rotation and wobbles it back to its initial rotation. NOTE: Due to the way iTween utilizes the Transform.Rotate method, PunchRotation works best with single axis usage rather than punching with a Vector3.")]
	public class iTweenPunchRotation : iTweenFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		[Tooltip("A vector punch range.")]
		[RequiredField]
		public FsmVector3 vector;

		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType = iTween.LoopType.none;

		public Space space = Space.World;

		public override void Reset()
		{
			base.Reset();
			id = new FsmString
			{
				UseVariable = true
			};
			time = 1f;
			delay = 0f;
			loopType = iTween.LoopType.none;
			vector = new FsmVector3
			{
				UseVariable = true
			};
			space = Space.World;
		}

		public override void OnEnter()
		{
			OnEnteriTween(gameObject);
			if (loopType != 0)
			{
				IsLoop(true);
			}
			DoiTween();
		}

		public override void OnExit()
		{
			OnExitiTween(gameObject);
		}

		private void DoiTween()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				Vector3 vector = Vector3.zero;
				if (!this.vector.IsNone)
				{
					vector = this.vector.Value;
				}
				itweenType = "punch";
				iTween.PunchRotation(ownerDefaultTarget, iTween.Hash("amount", vector, "name", id.IsNone ? "" : id.Value, "time", time.IsNone ? 1f : time.Value, "delay", delay.IsNone ? 0f : delay.Value, "looptype", loopType, "oncomplete", "iTweenOnComplete", "oncompleteparams", itweenID, "onstart", "iTweenOnStart", "onstartparams", itweenID, "ignoretimescale", !realTime.IsNone && realTime.Value, "space", space));
			}
		}
	}
}