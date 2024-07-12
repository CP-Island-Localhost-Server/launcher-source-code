using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of a bool parameter")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorBool : FsmStateActionAnimatorBase
	{
		[Tooltip("The target")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.AnimatorBool)]
		[RequiredField]
		[Tooltip("The animator parameter")]
		public FsmString parameter;

		[Tooltip("The Bool value to assign to the animator parameter")]
		public FsmBool Value;

		private Animator _animator;

		private int _paramID;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			parameter = null;
			Value = null;
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
			SetParameter();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			SetParameter();
		}

		private void SetParameter()
		{
			if (_animator != null)
			{
				_animator.SetBool(_paramID, Value.Value);
			}
		}
	}
}
