using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class BaseAnimationAction : ComponentAction<Animation>
	{
		public override void OnActionTargetInvoked(object targetObject)
		{
			AnimationClip animationClip = targetObject as AnimationClip;
			if (!(animationClip == null))
			{
				Animation animation = base.Owner.GetComponent<Animation>();
				if (animation != null)
				{
					animation.AddClip(animationClip, animationClip.name);
				}
			}
		}
	}
}
