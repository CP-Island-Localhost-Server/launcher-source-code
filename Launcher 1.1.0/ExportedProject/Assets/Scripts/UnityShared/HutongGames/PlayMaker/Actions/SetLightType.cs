using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set Spot, Directional, or Point Light type.")]
	[ActionCategory(ActionCategory.Lights)]
	public class SetLightType : ComponentAction<Light>
	{
		[RequiredField]
		[CheckForComponent(typeof(Light))]
		public FsmOwnerDefault gameObject;

		[ObjectType(typeof(LightType))]
		public FsmEnum lightType;

		public override void Reset()
		{
			gameObject = null;
			lightType = LightType.Point;
		}

		public override void OnEnter()
		{
			DoSetLightType();
			Finish();
		}

		private void DoSetLightType()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.light.type = (LightType)(object)lightType.Value;
			}
		}
	}
}
