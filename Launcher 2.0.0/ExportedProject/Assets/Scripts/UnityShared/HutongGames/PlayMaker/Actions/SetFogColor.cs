using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the color of the Fog in the scene.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetFogColor : FsmStateAction
	{
		[RequiredField]
		public FsmColor fogColor;

		public bool everyFrame;

		public override void Reset()
		{
			fogColor = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFogColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetFogColor();
		}

		private void DoSetFogColor()
		{
			RenderSettings.fogColor = fogColor.Value;
		}
	}
}
