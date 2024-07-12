using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns the feet pivot. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorFeetPivotActive : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The Target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The feet pivot Blending. At 0% blending point is body mass center. At 100% blending point is feet pivot")]
		[RequiredField]
		public FsmFloat feetPivotActive;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			feetPivotActive = null;
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
			DoGetFeetPivotActive();
			Finish();
		}

		private void DoGetFeetPivotActive()
		{
			if (!(_animator == null))
			{
				feetPivotActive.Value = _animator.feetPivotActive;
			}
		}
	}
}
