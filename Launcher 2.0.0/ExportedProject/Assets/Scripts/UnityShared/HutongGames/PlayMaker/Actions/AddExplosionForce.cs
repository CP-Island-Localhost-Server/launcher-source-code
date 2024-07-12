using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Applies a force to a Game Object that simulates explosion effects. The explosion force will fall off linearly with distance. Hint: Use the Explosion Action instead to apply an explosion force to all objects in a blast radius.")]
	[ActionCategory(ActionCategory.Physics)]
	public class AddExplosionForce : ComponentAction<Rigidbody>
	{
		[Tooltip("The GameObject to add the explosion force to.")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The center of the explosion. Hint: this is often the position returned from a GetCollisionInfo action.")]
		public FsmVector3 center;

		[Tooltip("The strength of the explosion.")]
		[RequiredField]
		public FsmFloat force;

		[Tooltip("The radius of the explosion. Force falls off linearly with distance.")]
		[RequiredField]
		public FsmFloat radius;

		[Tooltip("Applies the force as if it was applied from beneath the object. This is useful since explosions that throw things up instead of pushing things to the side look cooler. A value of 2 will apply a force as if it is applied from 2 meters below while not changing the actual explosion position.")]
		public FsmFloat upwardsModifier;

		[Tooltip("The type of force to apply. See Unity Physics docs.")]
		public ForceMode forceMode;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			center = new FsmVector3
			{
				UseVariable = true
			};
			upwardsModifier = 0f;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter()
		{
			DoAddExplosionForce();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoAddExplosionForce();
		}

		private void DoAddExplosionForce()
		{
			GameObject go = ((gameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : gameObject.GameObject.Value);
			if (center != null && UpdateCache(go))
			{
				base.rigidbody.AddExplosionForce(force.Value, center.Value, radius.Value, upwardsModifier.Value, forceMode);
			}
		}
	}
}
