using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Close a group started with GUILayout Begin ScrollView.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutEndScrollView : FsmStateAction
	{
		public override void OnGUI()
		{
			GUILayout.EndScrollView();
		}
	}
}
