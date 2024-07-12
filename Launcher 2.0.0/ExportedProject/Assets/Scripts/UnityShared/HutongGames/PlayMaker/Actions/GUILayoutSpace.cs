using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Inserts a space in the current layout group.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class GUILayoutSpace : FsmStateAction
	{
		public FsmFloat space;

		public override void Reset()
		{
			space = 10f;
		}

		public override void OnGUI()
		{
			GUILayout.Space(space.Value);
		}
	}
}
