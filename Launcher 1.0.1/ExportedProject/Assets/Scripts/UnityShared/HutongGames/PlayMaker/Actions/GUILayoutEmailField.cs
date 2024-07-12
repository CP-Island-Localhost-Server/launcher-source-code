using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUILayout Password Field. Optionally send an event if the text has been edited.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutEmailField : GUILayoutAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The email Text")]
		public FsmString text;

		[Tooltip("The Maximum Length of the field")]
		public FsmInt maxLength;

		[Tooltip("The Style of the Field")]
		public FsmString style;

		[Tooltip("Event sent when field content changed")]
		public FsmEvent changedEvent;

		[Tooltip("Email valid format flag")]
		public FsmBool valid;

		public override void Reset()
		{
			text = null;
			maxLength = 25;
			style = "TextField";
			valid = true;
			changedEvent = null;
		}

		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			text.Value = GUILayout.TextField(text.Value, style.Value, base.LayoutOptions);
			if (GUI.changed)
			{
				base.Fsm.Event(changedEvent);
				GUIUtility.ExitGUI();
			}
			else
			{
				GUI.changed = changed;
			}
		}
	}
}
