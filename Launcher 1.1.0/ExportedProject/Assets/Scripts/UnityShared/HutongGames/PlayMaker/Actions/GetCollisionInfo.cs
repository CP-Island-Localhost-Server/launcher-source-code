namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets info on the last collision event and store in variables. See Unity Physics docs.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetCollisionInfo : FsmStateAction
	{
		[Tooltip("Get the GameObject hit.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the relative velocity of the collision.")]
		public FsmVector3 relativeVelocity;

		[Tooltip("Get the relative speed of the collision. Useful for controlling reactions. E.g., selecting an appropriate sound fx.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat relativeSpeed;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the world position of the collision contact. Useful for spawning effects etc.")]
		public FsmVector3 contactPoint;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the collision normal vector. Useful for aligning spawned effects etc.")]
		public FsmVector3 contactNormal;

		[Tooltip("Get the name of the physics material of the colliding GameObject. Useful for triggering different effects. Audio, particles...")]
		[UIHint(UIHint.Variable)]
		public FsmString physicsMaterialName;

		public override void Reset()
		{
			gameObjectHit = null;
			relativeVelocity = null;
			relativeSpeed = null;
			contactPoint = null;
			contactNormal = null;
			physicsMaterialName = null;
		}

		private void StoreCollisionInfo()
		{
			if (base.Fsm.CollisionInfo != null)
			{
				gameObjectHit.Value = base.Fsm.CollisionInfo.gameObject;
				relativeSpeed.Value = base.Fsm.CollisionInfo.relativeVelocity.magnitude;
				relativeVelocity.Value = base.Fsm.CollisionInfo.relativeVelocity;
				physicsMaterialName.Value = base.Fsm.CollisionInfo.collider.material.name;
				if (base.Fsm.CollisionInfo.contacts != null && base.Fsm.CollisionInfo.contacts.Length > 0)
				{
					contactPoint.Value = base.Fsm.CollisionInfo.contacts[0].point;
					contactNormal.Value = base.Fsm.CollisionInfo.contacts[0].normal;
				}
			}
		}

		public override void OnEnter()
		{
			StoreCollisionInfo();
			Finish();
		}
	}
}
