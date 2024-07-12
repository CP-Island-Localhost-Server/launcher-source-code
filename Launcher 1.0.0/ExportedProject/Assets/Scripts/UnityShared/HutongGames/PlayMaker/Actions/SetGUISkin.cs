using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the GUISkin used by GUI elements.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUISkin : FsmStateAction
	{
		[RequiredField]
		public GUISkin skin;

		public FsmBool applyGlobally;

		public override void Reset()
		{
			skin = null;
			applyGlobally = true;
		}

		public override void OnGUI()
		{
			if (skin != null)
			{
				GUI.skin = skin;
			}
			if (applyGlobally.Value)
			{
				PlayMakerGUI.GUISkin = skin;
				Finish();
			}
		}
	}
}
