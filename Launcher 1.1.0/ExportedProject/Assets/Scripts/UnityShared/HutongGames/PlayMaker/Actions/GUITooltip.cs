using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Tooltip of the control the mouse is currently over and store it in a String Variable.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUITooltip : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		public FsmString storeTooltip;

		public override void Reset()
		{
			storeTooltip = null;
		}

		public override void OnGUI()
		{
			storeTooltip.Value = GUI.tooltip;
		}
	}
}
