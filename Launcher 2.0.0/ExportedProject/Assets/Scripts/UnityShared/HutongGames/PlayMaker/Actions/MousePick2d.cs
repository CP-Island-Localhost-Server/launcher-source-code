using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Perform a Mouse Pick on a 2d scene and stores the results. Use Ray Distance to set how close the camera must be to pick the 2d object.")]
	[ActionCategory(ActionCategory.Input)]
	public class MousePick2d : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Store if a GameObject was picked in a Bool variable. True if a GameObject was picked, otherwise false.")]
		public FsmBool storeDidPickObject;

		[Tooltip("Store the picked GameObject in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the picked point in a variable.")]
		public FsmVector2 storePoint;

		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			storeDidPickObject = null;
			storeGameObject = null;
			storePoint = null;
			layerMask = new FsmInt[0];
			invertMask = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoMousePick2d();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoMousePick2d();
		}

		private void DoMousePick2d()
		{
			RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition), float.PositiveInfinity, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
			bool flag = rayIntersection.collider != null;
			storeDidPickObject.Value = flag;
			if (flag)
			{
				storeGameObject.Value = rayIntersection.collider.gameObject;
				storePoint.Value = rayIntersection.point;
			}
			else
			{
				storeGameObject.Value = null;
				storePoint.Value = Vector3.zero;
			}
		}
	}
}
