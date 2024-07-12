using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUILayout Label.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutLabel : GUILayoutAction
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
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Label(new GUIContent(text.Value, image.Value, tooltip.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(text.Value, image.Value, tooltip.Value), style.Value, base.LayoutOptions);
			}
		}
	}
}
