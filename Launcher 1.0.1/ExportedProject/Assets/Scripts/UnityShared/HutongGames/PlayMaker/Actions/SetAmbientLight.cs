using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Ambient Light Color for the scene.")]
	[ActionCategory(ActionCategory.RenderSettings)]
	public class SetAmbientLight : FsmStateAction
	{
		[RequiredField]
		public FsmColor ambientColor;

		public bool everyFrame;

		public override void Reset()
		{
			ambientColor = Color.gray;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetAmbientColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSetAmbientColor();
		}

		private void DoSetAmbientColor()
		{
			RenderSettings.ambientLight = ambientColor.Value;
		}
	}
}
