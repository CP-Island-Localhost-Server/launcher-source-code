using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the global Skybox.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetSkybox : FsmStateAction
	{
		public FsmMaterial skybox;

		[Tooltip("Repeat every frame. Useful if the Skybox is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			skybox = null;
		}

		public override void OnEnter()
		{
			RenderSettings.skybox = skybox.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			RenderSettings.skybox = skybox.Value;
		}
	}
}
