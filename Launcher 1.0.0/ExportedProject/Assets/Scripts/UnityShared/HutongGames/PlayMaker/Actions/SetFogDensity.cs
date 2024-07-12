using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the density of the Fog in the scene.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetFogDensity : FsmStateAction
	{
		[RequiredField]
		public FsmFloat fogDensity;

		public bool everyFrame;

		public override void Reset()
		{
			fogDensity = 0.5f;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFogDensity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetFogDensity();
		}

		private void DoSetFogDensity()
		{
			RenderSettings.fogDensity = fogDensity.Value;
		}
	}
}
