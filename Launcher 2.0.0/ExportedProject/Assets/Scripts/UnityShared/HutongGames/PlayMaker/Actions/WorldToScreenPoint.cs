using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Transforms position from world space into screen space. NOTE: Uses the MainCamera!")]
	[ActionCategory(ActionCategory.Camera)]
	public class WorldToScreenPoint : FsmStateAction
	{
		[Tooltip("World position to transform into screen coordinates.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 worldPosition;

		[Tooltip("World X position.")]
		public FsmFloat worldX;

		[Tooltip("World Y position.")]
		public FsmFloat worldY;

		[Tooltip("World Z position.")]
		public FsmFloat worldZ;

		[Tooltip("Store the screen position in a Vector3 Variable. Z will equal zero.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeScreenPoint;

		[Tooltip("Store the screen X position in a Float Variable.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeScreenX;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the screen Y position in a Float Variable.")]
		public FsmFloat storeScreenY;

		[Tooltip("Normalize screen coordinates (0-1). Otherwise coordinates are in pixels.")]
		public FsmBool normalize;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			worldPosition = null;
			worldX = new FsmFloat
			{
				UseVariable = true
			};
			worldY = new FsmFloat
			{
				UseVariable = true
			};
			worldZ = new FsmFloat
			{
				UseVariable = true
			};
			storeScreenPoint = null;
			storeScreenX = null;
			storeScreenY = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoWorldToScreenPoint();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoWorldToScreenPoint();
		}

		private void DoWorldToScreenPoint()
		{
			if (Camera.main == null)
			{
				LogError("No MainCamera defined!");
				Finish();
				return;
			}
			Vector3 position = Vector3.zero;
			if (!worldPosition.IsNone)
			{
				position = worldPosition.Value;
			}
			if (!worldX.IsNone)
			{
				position.x = worldX.Value;
			}
			if (!worldY.IsNone)
			{
				position.y = worldY.Value;
			}
			if (!worldZ.IsNone)
			{
				position.z = worldZ.Value;
			}
			position = Camera.main.WorldToScreenPoint(position);
			if (normalize.Value)
			{
				position.x /= Screen.width;
				position.y /= Screen.height;
			}
			storeScreenPoint.Value = position;
			storeScreenX.Value = position.x;
			storeScreenY.Value = position.y;
		}
	}
}
