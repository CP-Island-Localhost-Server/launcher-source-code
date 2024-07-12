using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Instantly changes a GameObject's Euler angles in degrees then returns it to it's starting rotation over time.")]
	[ActionCategory("iTween")]
	public class iTweenRotateFrom : iTweenFsmAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("iTween ID. If set you can use iTween Stop action to stop it by its id.")]
		public FsmString id;

		[Tooltip("A rotation from a GameObject.")]
		public FsmGameObject transformRotation;

		[Tooltip("A rotation vector the GameObject will animate from.")]
		public FsmVector3 vectorRotation;

		[Tooltip("The time in seconds the animation will take to complete.")]
		public FsmFloat time;

		[Tooltip("The time in seconds the animation will wait before beginning.")]
		public FsmFloat delay;

		[Tooltip("Can be used instead of time to allow animation based on speed. When you define speed the time variable is ignored.")]
		public FsmFloat speed;

		[Tooltip("The shape of the easing curve applied to the animation.")]
		public iTween.EaseType easeType = iTween.EaseType.linear;

		[Tooltip("The type of loop to apply once the animation has completed.")]
		public iTween.LoopType loopType = iTween.LoopType.none;

		[Tooltip("Whether to animate in local or world space.")]
		public Space space = Space.World;

		public override void Reset()
		{
			base.Reset();
			id = new FsmString
			{
				UseVariable = true
			};
			transformRotation = new FsmGameObject
			{
				UseVariable = true
			};
			vectorRotation = new FsmVector3
			{
				UseVariable = true
			};
			time = 1f;
			delay = 0f;
			loopType = iTween.LoopType.none;
			speed = new FsmFloat
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
				Vector3 vector = (vectorRotation.IsNone ? Vector3.zero : vectorRotation.Value);
				if (!transformRotation.IsNone && (bool)transformRotation.Value)
				{
					vector = ((space == Space.World) ? (transformRotation.Value.transform.eulerAngles + vector) : (transformRotation.Value.transform.localEulerAngles + vector));
				}
				itweenType = "rotate";
				iTween.RotateFrom(ownerDefaultTarget, iTween.Hash("rotation", vector, "name", id.IsNone ? "" : id.Value, speed.IsNone ? "time" : "speed", (!speed.IsNone) ? speed.Value : (time.IsNone ? 1f : time.Value), "delay", delay.IsNone ? 0f : delay.Value, "easetype", easeType, "looptype", loopType, "oncomplete", "iTweenOnComplete", "oncompleteparams", itweenID, "onstart", "iTweenOnStart", "onstartparams", itweenID, "ignoretimescale", !realTime.IsNone && realTime.Value, "islocal", space == Space.Self));
			}
		}
	}
}
