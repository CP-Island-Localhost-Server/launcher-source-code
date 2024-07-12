using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Detect collisions between the Owner of this FSM and other Game Objects that have RigidBody components.\nNOTE: The system events, COLLISION ENTER, COLLISION STAY, and COLLISION EXIT are sent automatically on collisions with any object. Use this action to filter collisions by Tag.")]
	[ActionCategory(ActionCategory.Physics)]
	public class CollisionEvent : FsmStateAction
	{
		[Tooltip("The type of collision to detect.")]
		public CollisionType collision;

		[Tooltip("Filter by Tag.")]
		[UIHint(UIHint.Tag)]
		public FsmString collideTag;

		[Tooltip("Event to send if a collision is detected.")]
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		public FsmGameObject storeCollider;

		[Tooltip("Store the force of the collision. NOTE: Use Get Collision Info to get more info about the collision.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeForce;

		public override void Reset()
		{
			collision = CollisionType.OnCollisionEnter;
			collideTag = "Untagged";
			sendEvent = null;
			storeCollider = null;
			storeForce = null;
		}

		public override void OnPreprocess()
		{
			switch (collision)
			{
			case CollisionType.OnCollisionEnter:
				base.Fsm.HandleCollisionEnter = true;
				break;
			case CollisionType.OnCollisionStay:
				base.Fsm.HandleCollisionStay = true;
				break;
			case CollisionType.OnCollisionExit:
				base.Fsm.HandleCollisionExit = true;
				break;
			case CollisionType.OnControllerColliderHit:
				base.Fsm.HandleControllerColliderHit = true;
				break;
			case CollisionType.OnParticleCollision:
				base.Fsm.HandleParticleCollision = true;
				break;
			}
		}

		private void StoreCollisionInfo(Collision collisionInfo)
		{
			storeCollider.Value = collisionInfo.gameObject;
			storeForce.Value = collisionInfo.relativeVelocity.magnitude;
		}

		public override void DoCollisionEnter(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionEnter && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoCollisionStay(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionStay && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoCollisionExit(Collision collisionInfo)
		{
			if (collision == CollisionType.OnCollisionExit && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoControllerColliderHit(ControllerColliderHit collisionInfo)
		{
			if (collision == CollisionType.OnControllerColliderHit && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				if (storeCollider != null)
				{
					storeCollider.Value = collisionInfo.gameObject;
				}
				storeForce.Value = 0f;
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoParticleCollision(GameObject other)
		{
			if (collision == CollisionType.OnParticleCollision && other.tag == collideTag.Value)
			{
				if (storeCollider != null)
				{
					storeCollider.Value = other;
				}
				storeForce.Value = 0f;
				base.Fsm.Event(sendEvent);
			}
		}

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}
	}
}
