using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Main Camera.")]
	[ActionCategory(ActionCategory.Camera)]
	public class SetMainCamera : FsmStateAction
	{
		[Tooltip("The GameObject to set as the main camera (should have a Camera component).")]
		[CheckForComponent(typeof(Camera))]
		[RequiredField]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			if (gameObject.Value != null)
			{
				if (Camera.main != null)
				{
					Camera.main.gameObject.tag = "Untagged";
				}
				gameObject.Value.tag = "MainCamera";
			}
			Finish();
		}
	}
}
