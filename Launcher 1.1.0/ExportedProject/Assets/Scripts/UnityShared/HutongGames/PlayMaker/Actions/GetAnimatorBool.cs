using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the value of a bool parameter")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorBool : FsmStateActionAnimatorBase
	{
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.AnimatorBool)]
		[Tooltip("The animator parameter")]
		[RequiredField]
		public FsmString parameter;

		[ActionSection("Results")]
		[RequiredField]
		[Tooltip("The bool value of the animator parameter")]
		[UIHint(UIHint.Variable)]
		public FsmBool result;

		private Animator _animator;

		private int _paramID;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			parameter = null;
			result = null;
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
			_paramID = Animator.StringToHash(parameter.Value);
			GetParameter();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			GetParameter();
		}

		private void GetParameter()
		{
			if (_animator != null)
			{
				result.Value = _animator.GetBool(_paramID);
			}
		}
	}
}
