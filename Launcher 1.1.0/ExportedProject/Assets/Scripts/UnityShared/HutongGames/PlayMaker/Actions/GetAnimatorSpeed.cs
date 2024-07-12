using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the playback speed of the Animator. 1 is normal playback speed")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorSpeed : FsmStateActionAnimatorBase
	{
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The playBack speed of the animator. 1 is normal playback speed")]
		[UIHint(UIHint.Variable)]
		public FsmFloat speed;

		private Animator _animator;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			speed = null;
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
			GetPlaybackSpeed();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			GetPlaybackSpeed();
		}

		private void GetPlaybackSpeed()
		{
			if (_animator != null)
			{
				speed.Value = _animator.speed;
			}
		}
	}
}
