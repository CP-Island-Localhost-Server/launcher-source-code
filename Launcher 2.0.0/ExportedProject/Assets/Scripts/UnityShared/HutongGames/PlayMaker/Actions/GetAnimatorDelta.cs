using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the avatar delta position and rotation for the last evaluated frame.")]
	public class GetAnimatorDelta : FsmStateActionAnimatorBase
	{
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The avatar delta position for the last evaluated frame")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 deltaPosition;

		[Tooltip("The avatar delta position for the last evaluated frame")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion deltaRotation;

		private Transform _transform;

		private Animator _animator;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			deltaPosition = null;
			deltaRotation = null;
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
			DoGetDeltaPosition();
			Finish();
		}

		public override void OnActionUpdate()
		{
			DoGetDeltaPosition();
		}

		private void DoGetDeltaPosition()
		{
			if (!(_animator == null))
			{
				deltaPosition.Value = _animator.deltaPosition;
				deltaRotation.Value = _animator.deltaRotation;
			}
		}
	}
}
