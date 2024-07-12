using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets info on the last 2d Raycast or LineCast and store in variables.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetRayCastHit2dInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the GameObject hit by the last Raycast and store it in a variable.")]
		public FsmGameObject gameObjectHit;

		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		[Title("Hit Point")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 point;

		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 normal;

		[UIHint(UIHint.Variable)]
		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		public FsmFloat distance;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObjectHit = null;
			point = null;
			normal = null;
			distance = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			StoreRaycastInfo();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			StoreRaycastInfo();
		}

		private void StoreRaycastInfo()
		{
			RaycastHit2D lastRaycastHit2DInfo = Fsm.GetLastRaycastHit2DInfo(base.Fsm);
			if (lastRaycastHit2DInfo.collider != null)
			{
				gameObjectHit.Value = lastRaycastHit2DInfo.collider.gameObject;
				point.Value = lastRaycastHit2DInfo.point;
				normal.Value = lastRaycastHit2DInfo.normal;
				distance.Value = lastRaycastHit2DInfo.fraction;
			}
		}
	}
}
