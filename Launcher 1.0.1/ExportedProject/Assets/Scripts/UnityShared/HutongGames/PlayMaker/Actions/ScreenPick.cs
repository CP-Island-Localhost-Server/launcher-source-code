using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Perform a raycast into the scene using screen coordinates and stores the results. Use Ray Distance to set how close the camera must be to pick the object. NOTE: Uses the MainCamera!")]
	[ActionCategory(ActionCategory.Input)]
	public class ScreenPick : FsmStateAction
	{
		[Tooltip("A Vector3 screen position. Commonly stored by other actions.")]
		public FsmVector3 screenVector;

		[Tooltip("X position on screen.")]
		public FsmFloat screenX;

		[Tooltip("Y position on screen.")]
		public FsmFloat screenY;

		[Tooltip("Are the supplied screen coordinates normalized (0-1), or in pixels.")]
		public FsmBool normalized;

		[RequiredField]
		public FsmFloat rayDistance = 100f;

		[UIHint(UIHint.Variable)]
		public FsmBool storeDidPickObject;

		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		[UIHint(UIHint.Variable)]
		public FsmVector3 storePoint;

		[UIHint(UIHint.Variable)]
		public FsmVector3 storeNormal;

		[UIHint(UIHint.Variable)]
		public FsmFloat storeDistance;

		[UIHint(UIHint.Layer)]
		[Tooltip("Pick only from these layers.")]
		public FsmInt[] layerMask;

		[Tooltip("Invert the mask, so you pick from all layers except those defined above.")]
		public FsmBool invertMask;

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
			rayDistance = 100f;
			storeDidPickObject = null;
			storeGameObject = null;
			storePoint = null;
			storeNormal = null;
			storeDistance = null;
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
			Ray ray = Camera.main.ScreenPointToRay(position);
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, rayDistance.Value, ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value));
			bool flag = hitInfo.collider != null;
			storeDidPickObject.Value = flag;
			if (flag)
			{
				storeGameObject.Value = hitInfo.collider.gameObject;
				storeDistance.Value = hitInfo.distance;
				storePoint.Value = hitInfo.point;
				storeNormal.Value = hitInfo.normal;
			}
			else
			{
				storeGameObject.Value = null;
				storeDistance = float.PositiveInfinity;
				storePoint.Value = Vector3.zero;
				storeNormal.Value = Vector3.zero;
			}
		}
	}
}
