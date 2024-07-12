using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("Gets the GameObject tagged MainCamera from the scene")]
	[ActionTarget(typeof(Camera), "storeGameObject", false)]
	public class GetMainCamera : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;

		public override void Reset()
		{
			storeGameObject = null;
		}

		public override void OnEnter()
		{
			storeGameObject.Value = ((Camera.main != null) ? Camera.main.gameObject : null);
			Finish();
		}
	}
}
