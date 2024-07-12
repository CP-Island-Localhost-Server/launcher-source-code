using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns The current gravity weight based on current animations that are played")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorGravityWeight : FsmStateActionAnimatorBase
	{
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The current gravity weight based on current animations that are played")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmFloat gravityWeight;

		private Animator _animator;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			gravityWeight = null;
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
			DoGetGravityWeight();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			DoGetGravityWeight();
		}

		private void DoGetGravityWeight()
		{
			if (!(_animator == null))
			{
				gravityWeight.Value = _animator.gravityWeight;
			}
		}
	}
}
