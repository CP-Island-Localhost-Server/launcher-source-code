using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Tinting Color for the GUI. By default only effects GUI rendered by this FSM, check Apply Globally to effect all GUI controls.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUIColor : FsmStateAction
	{
		[RequiredField]
		public FsmColor color;

		public FsmBool applyGlobally;

		public override void Reset()
		{
			color = Color.white;
		}

		public override void OnGUI()
		{
			GUI.color = color.Value;
			if (applyGlobally.Value)
			{
				PlayMakerGUI.GUIColor = GUI.color;
			}
		}
	}
}
