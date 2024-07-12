using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rewinds the named animation.")]
	[ActionCategory(ActionCategory.Animation)]
	public class RewindAnimation : BaseAnimationAction
	{
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Animation)]
		public FsmString animName;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
		}

		public override void OnEnter()
		{
			DoRewindAnimation();
			Finish();
		}

		private void DoRewindAnimation()
		{
			if (!string.IsNullOrEmpty(animName.Value))
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
				if (UpdateCache(ownerDefaultTarget))
				{
					base.animation.Rewind(animName.Value);
				}
			}
		}
	}
}
