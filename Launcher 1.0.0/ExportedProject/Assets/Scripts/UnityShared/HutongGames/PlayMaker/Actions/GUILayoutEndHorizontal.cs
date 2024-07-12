using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Close a group started with BeginHorizontal.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutEndHorizontal : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnGUI()
		{
			GUILayout.EndHorizontal();
		}
	}
}
