using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Enables/Disables an Animation on a GameObject.\nAnimation time is paused while disabled. Animation must also have a non zero weight to play.")]
	[ActionCategory(ActionCategory.Animation)]
	public class EnableAnimation : BaseAnimationAction
	{
		[Tooltip("The GameObject playing the animation.")]
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Animation)]
		[RequiredField]
		[Tooltip("The name of the animation to enable/disable.")]
		public FsmString animName;

		[Tooltip("Set to True to enable, False to disable.")]
		[RequiredField]
		public FsmBool enable;

		[Tooltip("Reset the initial enabled state when exiting the state.")]
		public FsmBool resetOnExit;

		private AnimationState anim;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
			enable = true;
			resetOnExit = false;
		}

		public override void OnEnter()
		{
			DoEnableAnimation(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private void DoEnableAnimation(GameObject go)
		{
			if (UpdateCache(go))
			{
				anim = base.animation[animName.Value];
				if (anim != null)
				{
					anim.enabled = enable.Value;
				}
			}
		}

		public override void OnExit()
		{
			if (resetOnExit.Value && anim != null)
			{
				anim.enabled = !enable.Value;
			}
		}
	}
}
