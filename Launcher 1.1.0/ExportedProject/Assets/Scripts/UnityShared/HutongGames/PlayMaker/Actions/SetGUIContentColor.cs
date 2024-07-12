using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Tinting Color for all text rendered by the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUIContentColor : FsmStateAction
	{
		[RequiredField]
		public FsmColor contentColor;

		public FsmBool applyGlobally;

		public override void Reset()
		{
			contentColor = Color.white;
		}

		public override void OnGUI()
		{
			GUI.contentColor = contentColor.Value;
			if (applyGlobally.Value)
			{
				PlayMakerGUI.GUIContentColor = GUI.contentColor;
			}
		}
	}
}
