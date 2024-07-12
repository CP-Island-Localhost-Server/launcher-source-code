using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("GUI button. Sends an Event when pressed. Optionally store the button state in a Bool Variable.")]
	public class GUIButton : GUIContentAction
	{
		public FsmEvent sendEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool storeButtonState;

		public override void Reset()
		{
			base.Reset();
			sendEvent = null;
			storeButtonState = null;
			style = "Button";
		}

		public override void OnGUI()
		{
			base.OnGUI();
			bool value = false;
			if (GUI.Button(rect, content, style.Value))
			{
				base.Fsm.Event(sendEvent);
				value = true;
			}
			if (storeButtonState != null)
			{
				storeButtonState.Value = value;
			}
		}
	}
}
