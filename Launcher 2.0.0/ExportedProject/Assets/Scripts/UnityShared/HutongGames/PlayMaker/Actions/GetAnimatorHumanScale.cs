using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns the scale of the current Avatar for a humanoid rig, (1 by default if the rig is generic).\n The scale is relative to Unity's Default Avatar")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorHumanScale : FsmStateAction
	{
		[Tooltip("The Target. An Animator component is required")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[ActionSection("Result")]
		[Tooltip("the scale of the current Avatar")]
		[UIHint(UIHint.Variable)]
		public FsmFloat humanScale;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			humanScale = null;
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
			DoGetHumanScale();
			Finish();
		}

		private void DoGetHumanScale()
		{
			if (!(_animator == null))
			{
				humanScale.Value = _animator.humanScale;
			}
		}
	}
}
