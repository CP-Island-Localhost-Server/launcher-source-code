using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Detect collisions between the Owner of this FSM and other Game Objects that have RigidBody2D components.\nNOTE: The system events, COLLISION ENTER 2D, COLLISION STAY 2D, and COLLISION EXIT 2D are sent automatically on collisions with any object. Use this action to filter collisions by Tag.")]
	public class Collision2dEvent : FsmStateAction
	{
		[Tooltip("The type of collision to detect.")]
		public Collision2DType collision;

		[UIHint(UIHint.Tag)]
		[Tooltip("Filter by Tag.")]
		public FsmString collideTag;

		[Tooltip("Event to send if a collision is detected.")]
		public FsmEvent sendEvent;

		[Tooltip("Store the GameObject that collided with the Owner of this FSM.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeCollider;

		[Tooltip("Store the force of the collision. NOTE: Use Get Collision 2D Info to get more info about the collision.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeForce;

		public override void Reset()
		{
			collision = Collision2DType.OnCollisionEnter2D;
			collideTag = "Untagged";
			sendEvent = null;
			storeCollider = null;
			storeForce = null;
		}

		public override void OnPreprocess()
		{
			switch (collision)
			{
			case Collision2DType.OnCollisionEnter2D:
				base.Fsm.HandleCollisionEnter2D = true;
				break;
			case Collision2DType.OnCollisionStay2D:
				base.Fsm.HandleCollisionStay2D = true;
				break;
			case Collision2DType.OnCollisionExit2D:
				base.Fsm.HandleCollisionExit2D = true;
				break;
			case Collision2DType.OnParticleCollision:
				base.Fsm.HandleParticleCollision = true;
				break;
			}
		}

		private void StoreCollisionInfo(Collision2D collisionInfo)
		{
			storeCollider.Value = collisionInfo.gameObject;
			storeForce.Value = collisionInfo.relativeVelocity.magnitude;
		}

		public override void DoCollisionEnter2D(Collision2D collisionInfo)
		{
			if (collision == Collision2DType.OnCollisionEnter2D && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoCollisionStay2D(Collision2D collisionInfo)
		{
			if (collision == Collision2DType.OnCollisionStay2D && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoCollisionExit2D(Collision2D collisionInfo)
		{
			if (collision == Collision2DType.OnCollisionExit2D && collisionInfo.collider.gameObject.tag == collideTag.Value)
			{
				StoreCollisionInfo(collisionInfo);
				base.Fsm.Event(sendEvent);
			}
		}

		public override void DoParticleCollision(GameObject other)
		{
			if (collision == Collision2DType.OnParticleCollision && other.tag == collideTag.Value)
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
			return ActionHelpers.CheckOwnerPhysics2dSetup(base.Owner);
		}
	}
}
