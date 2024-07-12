using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Lights)]
	[Tooltip("Sets the strength of the shadows cast by a Light.")]
	public class SetShadowStrength : ComponentAction<Light>
	{
		[CheckForComponent(typeof(Light))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		public FsmFloat shadowStrength;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			shadowStrength = 0.8f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetShadowStrength();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetShadowStrength();
		}

		private void DoSetShadowStrength()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				base.light.shadowStrength = shadowStrength.Value;
			}
		}
	}
}
