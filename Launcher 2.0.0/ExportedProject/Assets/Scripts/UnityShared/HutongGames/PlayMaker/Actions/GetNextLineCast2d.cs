using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Iterate through a list of all colliders detected by a LineCastThe colliders iterated are sorted in order of increasing Z coordinate. No iteration will take place if there are no colliders within the area.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetNextLineCast2d : FsmStateAction
	{
		[ActionSection("Setup")]
		[Tooltip("Start ray at game object position. \nOr use From Position parameter.")]
		public FsmOwnerDefault fromGameObject;

		[Tooltip("Start ray at a vector2 world position. \nOr use fromGameObject parameter. If both define, will add fromPosition to the fromGameObject position")]
		public FsmVector2 fromPosition;

		[Tooltip("End ray at game object position. \nOr use From Position parameter.")]
		public FsmGameObject toGameObject;

		[Tooltip("End ray at a vector2 world position. \nOr use fromGameObject parameter. If both define, will add toPosition to the ToGameObject position")]
		public FsmVector2 toPosition;

		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		[ActionSection("Filter")]
		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("Store the number of colliders found for this overlap.")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmInt collidersCount;

		[Tooltip("Store the next collider in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeNextCollider;

		[Tooltip("Get the 2d position of the next ray hit point and store it in a variable.")]
		public FsmVector2 storeNextHitPoint;

		[Tooltip("Get the 2d normal at the next hit point and store it in a variable.")]
		public FsmVector2 storeNextHitNormal;

		[Tooltip("Get the distance along the ray to the next hit point and store it in a variable.")]
		public FsmFloat storeNextHitDistance;

		[Tooltip("Event to send to get the next collider.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send when there are no more colliders to iterate.")]
		public FsmEvent finishedEvent;

		private RaycastHit2D[] hits;

		private int colliderCount;

		private int nextColliderIndex;

		public override void Reset()
		{
			fromGameObject = null;
			fromPosition = new FsmVector2
			{
				UseVariable = true
			};
			toGameObject = null;
			toPosition = new FsmVector2
			{
				UseVariable = true
			};
			minDepth = new FsmInt
			{
				UseVariable = true
			};
			maxDepth = new FsmInt
			{
				UseVariable = true
			};
			layerMask = new FsmInt[0];
			invertMask = false;
			collidersCount = null;
			storeNextCollider = null;
			storeNextHitPoint = null;
			storeNextHitNormal = null;
			storeNextHitDistance = null;
			loopEvent = null;
			finishedEvent = null;
		}

		public override void OnEnter()
		{
			if (hits == null)
			{
				hits = GetLineCastAll();
				colliderCount = hits.Length;
				collidersCount.Value = colliderCount;
			}
			DoGetNextCollider();
			Finish();
		}

		private void DoGetNextCollider()
		{
			if (nextColliderIndex >= colliderCount)
			{
				hits = new RaycastHit2D[0];
				nextColliderIndex = 0;
				base.Fsm.Event(finishedEvent);
				return;
			}
			Fsm.RecordLastRaycastHit2DInfo(base.Fsm, hits[nextColliderIndex]);
			storeNextCollider.Value = hits[nextColliderIndex].collider.gameObject;
			storeNextHitPoint.Value = hits[nextColliderIndex].point;
			storeNextHitNormal.Value = hits[nextColliderIndex].normal;
			storeNextHitDistance.Value = hits[nextColliderIndex].fraction;
			if (nextColliderIndex >= colliderCount)
			{
				hits = new RaycastHit2D[0];
				nextColliderIndex = 0;
				base.Fsm.Event(finishedEvent);
				return;
			}
			nextColliderIndex++;
			if (loopEvent != null)
			{
				base.Fsm.Event(loopEvent);
			}
		}

		private RaycastHit2D[] GetLineCastAll()
		{
			Vector2 value = fromPosition.Value;
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(fromGameObject);
			if (ownerDefaultTarget != null)
			{
				value.x += ownerDefaultTarget.transform.position.x;
				value.y += ownerDefaultTarget.transform.position.y;
			}
			Vector2 value2 = toPosition.Value;
			GameObject value3 = toGameObject.Value;
			if (value3 != null)
			{
				value2.x += value3.transform.position.x;
				value2.y += value3.transform.position.y;
			}
			if (minDepth.IsNone && maxDepth.IsNone)
			{
				return Physics2D.LinecastAll(value, value2, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
			}
			float num = (minDepth.IsNone ? float.NegativeInfinity : ((float)minDepth.Value));
			float num2 = (maxDepth.IsNone ? float.PositiveInfinity : ((float)maxDepth.Value));
			return Physics2D.LinecastAll(value, value2, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value), num, num2);
		}
	}
}
