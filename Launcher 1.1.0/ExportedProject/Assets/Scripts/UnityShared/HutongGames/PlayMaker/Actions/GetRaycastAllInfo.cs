namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last RaycastAll and store in array variables.")]
	public class GetRaycastAllInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		[Tooltip("Store the GameObjects hit in an array variable.")]
		public FsmArray storeHitObjects;

		[ArrayEditor(VariableType.Vector3, "", 0, 0, 65536)]
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the world position of all ray hit point and store them in an array variable.")]
		public FsmArray points;

		[ArrayEditor(VariableType.Vector3, "", 0, 0, 65536)]
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the normal at all hit points and store them in an array variable.")]
		public FsmArray normals;

		[ArrayEditor(VariableType.Float, "", 0, 0, 65536)]
		[Tooltip("Get the distance along the ray to all hit points and store tjem in an array variable.")]
		[UIHint(UIHint.Variable)]
		public FsmArray distances;

		[Tooltip("Repeat every frame. Warning, this could be affecting performances")]
		public bool everyFrame;

		public override void Reset()
		{
			storeHitObjects = null;
			points = null;
			normals = null;
			distances = null;
			everyFrame = false;
		}

		private void StoreRaycastAllInfo()
		{
			if (RaycastAll.RaycastAllHitInfo != null)
			{
				storeHitObjects.Resize(RaycastAll.RaycastAllHitInfo.Length);
				points.Resize(RaycastAll.RaycastAllHitInfo.Length);
				normals.Resize(RaycastAll.RaycastAllHitInfo.Length);
				distances.Resize(RaycastAll.RaycastAllHitInfo.Length);
				for (int i = 0; i < RaycastAll.RaycastAllHitInfo.Length; i++)
				{
					storeHitObjects.Values[i] = RaycastAll.RaycastAllHitInfo[i].collider.gameObject;
					points.Values[i] = RaycastAll.RaycastAllHitInfo[i].point;
					normals.Values[i] = RaycastAll.RaycastAllHitInfo[i].normal;
					distances.Values[i] = RaycastAll.RaycastAllHitInfo[i].distance;
				}
			}
		}

		public override void OnEnter()
		{
			StoreRaycastAllInfo();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			StoreRaycastAllInfo();
		}
	}
}
