using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Stops all playing Animations on a Game Object. Optionally, specify a single Animation to Stop.")]
	[ActionCategory(ActionCategory.Animation)]
	public class StopAnimation : BaseAnimationAction
	{
		[CheckForComponent(typeof(Animation))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Animation)]
		[Tooltip("Leave empty to stop all playing animations.")]
		public FsmString animName;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
		}

		public override void OnEnter()
		{
			DoStopAnimation();
			Finish();
		}

		private void DoStopAnimation()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				if (FsmString.IsNullOrEmpty(animName))
				{
					base.animation.Stop();
				}
				else
				{
					base.animation.Stop(animName.Value);
				}
			}
		}
	}
}
