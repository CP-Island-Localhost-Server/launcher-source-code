using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Iterate through a list of all colliders that overlap a point in space.The colliders iterated are sorted in order of increasing Z coordinate. No iteration will take place if there are no colliders overlap this point.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetNextOverlapPoint2d : FsmStateAction
	{
		[Tooltip("Point using the gameObject position. \nOr use From Position parameter.")]
		[ActionSection("Setup")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Point as a world position. \nOr use gameObject parameter. If both define, will add position to the gameObject position")]
		public FsmVector2 position;

		[Tooltip("Only include objects with a Z coordinate (depth) greater than this value. leave to none for no effect")]
		public FsmInt minDepth;

		[Tooltip("Only include objects with a Z coordinate (depth) less than this value. leave to none")]
		public FsmInt maxDepth;

		[UIHint(UIHint.Layer)]
		[ActionSection("Filter")]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		[Tooltip("Store the number of colliders found for this overlap.")]
		public FsmInt collidersCount;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the next collider in a GameObject variable.")]
		[RequiredField]
		public FsmGameObject storeNextCollider;

		[Tooltip("Event to send to get the next collider.")]
		public FsmEvent loopEvent;

		[Tooltip("Event to send when there are no more colliders to iterate.")]
		public FsmEvent finishedEvent;

		private Collider2D[] colliders;

		private int colliderCount;

		private int nextColliderIndex;

		public override void Reset()
		{
			gameObject = null;
			position = new FsmVector2
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
			loopEvent = null;
			finishedEvent = null;
		}

		public override void OnEnter()
		{
			if (colliders == null)
			{
				colliders = GetOverlapPointAll();
				colliderCount = colliders.Length;
				collidersCount.Value = colliderCount;
			}
			DoGetNextCollider();
			Finish();
		}

		private void DoGetNextCollider()
		{
			if (nextColliderIndex >= colliderCount)
			{
				nextColliderIndex = 0;
				base.Fsm.Event(finishedEvent);
				return;
			}
			storeNextCollider.Value = colliders[nextColliderIndex].gameObject;
			if (nextColliderIndex >= colliderCount)
			{
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

		private Collider2D[] GetOverlapPointAll()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			Vector2 value = position.Value;
			if (ownerDefaultTarget != null)
			{
				value.x += ownerDefaultTarget.transform.position.x;
				value.y += ownerDefaultTarget.transform.position.y;
			}
			if (minDepth.IsNone && maxDepth.IsNone)
			{
				return Physics2D.OverlapPointAll(value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
			}
			float num = (minDepth.IsNone ? float.NegativeInfinity : ((float)minDepth.Value));
			float num2 = (maxDepth.IsNone ? float.PositiveInfinity : ((float)maxDepth.Value));
			return Physics2D.OverlapPointAll(value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value), num, num2);
		}
	}
}
