using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Perform a raycast into the 2d scene using screen coordinates and stores the results. Use Ray Distance to set how close the camera must be to pick the 2d object. NOTE: Uses the MainCamera!")]
	[ActionCategory(ActionCategory.Input)]
	public class ScreenPick2d : FsmStateAction
	{
		[Tooltip("A Vector3 screen position. Commonly stored by other actions.")]
		public FsmVector3 screenVector;

		[Tooltip("X position on screen.")]
		public FsmFloat screenX;

		[Tooltip("Y position on screen.")]
		public FsmFloat screenY;

		[Tooltip("Are the supplied screen coordinates normalized (0-1), or in pixels.")]
		public FsmBool normalized;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store whether the Screen pick did pick a GameObject")]
		public FsmBool storeDidPickObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the picked GameObject")]
		public FsmGameObject storeGameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the picked position in world Space")]
		public FsmVector3 storePoint;

		[Tooltip("Pick only from these layers.")]
		[UIHint(UIHint.Layer)]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			screenVector = new FsmVector3
			{
				UseVariable = true
			};
			screenX = new FsmFloat
			{
				UseVariable = true
			};
			screenY = new FsmFloat
			{
				UseVariable = true
			};
			normalized = false;
			storeDidPickObject = null;
			storeGameObject = null;
			storePoint = null;
			layerMask = new FsmInt[0];
			invertMask = false;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoScreenPick();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoScreenPick();
		}

		private void DoScreenPick()
		{
			if (Camera.main == null)
			{
				LogError("No MainCamera defined!");
				Finish();
				return;
			}
			Vector3 position = Vector3.zero;
			if (!screenVector.IsNone)
			{
				position = screenVector.Value;
			}
			if (!screenX.IsNone)
			{
				position.x = screenX.Value;
			}
			if (!screenY.IsNone)
			{
				position.y = screenY.Value;
			}
			if (normalized.Value)
			{
				position.x *= Screen.width;
				position.y *= Screen.height;
			}
			RaycastHit2D rayIntersection = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position), float.PositiveInfinity, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
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
