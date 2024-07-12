using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUILayout Label for an Int Variable.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutIntLabel : GUILayoutAction
	{
		[Tooltip("Text to put before the int variable.")]
		public FsmString prefix;

		[Tooltip("Int variable to display.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt intVariable;

		[Tooltip("Optional GUIStyle in the active GUISKin.")]
		public FsmString style;

		public override void Reset()
		{
			base.Reset();
			prefix = "";
			style = "";
			intVariable = null;
		}

		public override void OnGUI()
		{
			if (string.IsNullOrEmpty(style.Value))
			{
				GUILayout.Label(new GUIContent(prefix.Value + intVariable.Value), base.LayoutOptions);
			}
			else
			{
				GUILayout.Label(new GUIContent(prefix.Value + intVariable.Value), style.Value, base.LayoutOptions);
			}
		}
	}
}
