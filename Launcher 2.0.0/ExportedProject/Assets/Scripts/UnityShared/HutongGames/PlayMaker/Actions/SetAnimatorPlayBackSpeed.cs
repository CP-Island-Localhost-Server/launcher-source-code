using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the playback speed of the Animator. 1 is normal playback speed")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorPlayBackSpeed : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		[Tooltip("The Target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("If true, automaticly stabilize feet during transition and blending")]
		public FsmFloat playBackSpeed;

		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			playBackSpeed = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Finish();
				return;
			}
			_animator = ownerDefaultTarget.GetComponent<Animator>();
			if (_animator == null)
			{
				Finish();
				return;
			}
			DoPlayBackSpeed();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoPlayBackSpeed();
		}

		private void DoPlayBackSpeed()
		{
			if (!(_animator == null))
			{
				_animator.speed = playBackSpeed.Value;
			}
		}
	}
}
