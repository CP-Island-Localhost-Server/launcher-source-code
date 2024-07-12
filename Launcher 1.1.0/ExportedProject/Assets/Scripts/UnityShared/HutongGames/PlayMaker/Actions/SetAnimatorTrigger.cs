using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a trigger parameter to active. Triggers are parameters that act mostly like booleans, but get reset to inactive when they are used in a transition.")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorTrigger : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The trigger name")]
		[UIHint(UIHint.AnimatorTrigger)]
		[RequiredField]
		public FsmString trigger;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			trigger = null;
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
			SetTrigger();
			Finish();
		}

		private void SetTrigger()
		{
			if (_animator != null)
			{
				_animator.SetTrigger(trigger.Value);
			}
		}
	}
}
