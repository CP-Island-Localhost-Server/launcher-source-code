using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the total number of currently loaded scenes.")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetSceneCount : FsmStateAction
	{
		[Tooltip("The number of currently loaded scenes.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt sceneCount;

		[Tooltip("Repeat every Frame")]
		public bool everyFrame;

		public override void Reset()
		{
			sceneCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetSceneCount();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetSceneCount();
		}

		private void DoGetSceneCount()
		{
			sceneCount.Value = SceneManager.sceneCount;
		}
	}
}
