using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the value of a float parameter")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorFloat : FsmStateActionAnimatorBase
	{
		[Tooltip("The target. An Animator component is required")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The animator parameter")]
		[RequiredField]
		[UIHint(UIHint.AnimatorFloat)]
		public FsmString parameter;

		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		[RequiredField]
		[Tooltip("The float value of the animator parameter")]
		public FsmFloat result;

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
				result.Value = _animator.GetFloat(_paramID);
			}
		}
	}
}
