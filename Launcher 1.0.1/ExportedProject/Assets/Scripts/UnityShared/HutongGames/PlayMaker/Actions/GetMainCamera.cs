using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionTarget(typeof(Camera), "storeGameObject", false)]
	[Tooltip("Gets the GameObject tagged MainCamera from the scene")]
	[ActionCategory(ActionCategory.Camera)]
	public class GetMainCamera : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
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
