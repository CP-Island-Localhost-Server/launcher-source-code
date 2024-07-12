using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Spot Angle of a Light.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightSpotAngle : ComponentAction<Light>
	{
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmFloat lightSpotAngle;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			lightSpotAngle = 20f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetLightRange();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetLightRange();
		}

		private void DoSetLightRange()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.light.spotAngle = lightSpotAngle.Value;
			}
		}
	}
}
