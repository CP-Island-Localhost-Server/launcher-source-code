using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns true if the specified layer is in a transition. Can also send events")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorIsLayerInTransition : FsmStateActionAnimatorBase
	{
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		[Tooltip("True if automatic matching is active")]
		public FsmBool isInTransition;

		[Tooltip("Event send if automatic matching is active")]
		public FsmEvent isInTransitionEvent;

		[Tooltip("Event send if automatic matching is not active")]
		public FsmEvent isNotInTransitionEvent;

		private Animator _animator;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			isInTransition = null;
			isInTransitionEvent = null;
			isNotInTransitionEvent = null;
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
			DoCheckIsInTransition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			DoCheckIsInTransition();
		}

		private void DoCheckIsInTransition()
		{
			if (!(_animator == null))
			{
				bool flag = _animator.IsInTransition(layerIndex.Value);
				if (!isInTransition.IsNone)
				{
					isInTransition.Value = flag;
				}
				if (flag)
				{
					base.Fsm.Event(isInTransitionEvent);
				}
				else
				{
					base.Fsm.Event(isNotInTransitionEvent);
				}
			}
		}
	}
}
