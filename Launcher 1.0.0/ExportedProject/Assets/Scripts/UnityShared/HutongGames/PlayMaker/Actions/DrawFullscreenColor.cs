using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Fills the screen with a Color. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
	[ActionCategory(ActionCategory.GUI)]
	public class DrawFullscreenColor : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Color. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
		public FsmColor color;

		public override void Reset()
		{
			color = Color.white;
		}

		public override void OnGUI()
		{
			Color color = GUI.color;
			GUI.color = this.color.Value;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), ActionHelpers.WhiteTexture);
			GUI.color = color;
		}
	}
}
