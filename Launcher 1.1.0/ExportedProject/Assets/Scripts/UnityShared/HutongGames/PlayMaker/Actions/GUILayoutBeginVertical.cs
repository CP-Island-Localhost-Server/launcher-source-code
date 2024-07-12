using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Begins a vertical control group. The group must be closed with GUILayoutEndVertical action.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutBeginVertical : GUILayoutAction
	{
		public FsmTexture image;

		public FsmString text;

		public FsmString tooltip;

		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			text = "";
			image = null;
			tooltip = "";
			style = "";
		}

		public override void OnGUI()
		{
			GUILayout.BeginVertical(new GUIContent(text.Value, image.Value, tooltip.Value), style.Value, base.LayoutOptions);
		}
	}
}
