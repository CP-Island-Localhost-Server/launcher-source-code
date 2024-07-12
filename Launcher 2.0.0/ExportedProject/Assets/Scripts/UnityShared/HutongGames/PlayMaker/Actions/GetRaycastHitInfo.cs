namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets info on the last Raycast and store in variables.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetRaycastHitInfo : FsmStateAction
	{
		[Tooltip("Get the GameObject hit by the last Raycast and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		[Tooltip("Get the world position of the ray hit point and store it in a variable.")]
		[Title("Hit Point")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 point;

		[Tooltip("Get the normal at the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 normal;

		[Tooltip("Get the distance along the ray to the hit point and store it in a variable.")]
		[UIHint(UIHint.Variable)]
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

		private void StoreRaycastInfo()
		{
			if (base.Fsm.RaycastHitInfo.collider != null)
			{
				gameObjectHit.Value = base.Fsm.RaycastHitInfo.collider.gameObject;
				point.Value = base.Fsm.RaycastHitInfo.point;
				normal.Value = base.Fsm.RaycastHitInfo.normal;
				distance.Value = base.Fsm.RaycastHitInfo.distance;
			}
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
	}
}
