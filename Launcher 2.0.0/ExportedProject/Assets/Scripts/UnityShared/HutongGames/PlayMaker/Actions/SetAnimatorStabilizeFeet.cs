using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("If true, automaticaly stabilize feet during transition and blending")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorStabilizeFeet : FsmStateAction
	{
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("If true, automaticaly stabilize feet during transition and blending")]
		public FsmBool stabilizeFeet;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			stabilizeFeet = null;
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
			DoStabilizeFeet();
			Finish();
		}

		private void DoStabilizeFeet()
		{
			if (!(_animator == null))
			{
				_animator.stabilizeFeet = stabilizeFeet.Value;
			}
		}
	}
}
