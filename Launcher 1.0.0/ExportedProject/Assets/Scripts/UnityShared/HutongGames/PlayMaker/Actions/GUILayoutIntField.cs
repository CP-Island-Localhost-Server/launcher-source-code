using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUILayout Text Field to edit an Int Variable. Optionally send an event if the text has been edited.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutIntField : GUILayoutAction
	{
		[Tooltip("Int Variable to show in the edit field.")]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		[Tooltip("Optional event to send when the value changes.")]
		public FsmEvent changedEvent;

		public override void Reset()
		{
			base.Reset();
			intVariable = null;
			style = "";
			changedEvent = null;
		}

		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			if (!string.IsNullOrEmpty(style.Value))
			{
				intVariable.Value = int.Parse(GUILayout.TextField(intVariable.Value.ToString(), style.Value, base.LayoutOptions));
			}
			else
			{
				intVariable.Value = int.Parse(GUILayout.TextField(intVariable.Value.ToString(), base.LayoutOptions));
			}
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
