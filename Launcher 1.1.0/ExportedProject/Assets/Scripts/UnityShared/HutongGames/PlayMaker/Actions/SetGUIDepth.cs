using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the sorting depth of subsequent GUI elements.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetGUIDepth : FsmStateAction
	{
		[RequiredField]
		public FsmInt depth;

		public override void Reset()
		{
			depth = 0;
		}

		public override void OnPreprocess()
		{
			base.Fsm.HandleOnGUI = true;
		}

		public override void OnGUI()
		{
			GUI.depth = depth.Value;
		}
	}
}
