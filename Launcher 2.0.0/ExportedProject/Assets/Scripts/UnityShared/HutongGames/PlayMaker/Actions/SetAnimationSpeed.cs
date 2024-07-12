using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Speed of an Animation. Check Every Frame to update the animation time continuosly, e.g., if you're manipulating a variable that controls animation speed.")]
	[ActionCategory(ActionCategory.Animation)]
	public class SetAnimationSpeed : BaseAnimationAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animation))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Animation)]
		public FsmString animName;

		public FsmFloat speed = 1f;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			animName = null;
			speed = 1f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetAnimationSpeed((gameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : gameObject.GameObject.Value);
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetAnimationSpeed((gameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : gameObject.GameObject.Value);
		}

		private void DoSetAnimationSpeed(GameObject go)
		{
			if (UpdateCache(go))
			{
				AnimationState animationState = base.animation[animName.Value];
				if (animationState == null)
				{
					LogWarning("Missing animation: " + animName.Value);
				}
				else
				{
					animationState.speed = speed.Value;
				}
			}
		}
	}
}
