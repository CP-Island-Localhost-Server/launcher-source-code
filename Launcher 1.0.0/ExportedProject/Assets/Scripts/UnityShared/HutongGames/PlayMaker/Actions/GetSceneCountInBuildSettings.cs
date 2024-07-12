using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the number of scenes in Build Settings.")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetSceneCountInBuildSettings : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The number of scenes in Build Settings.")]
		[UIHint(UIHint.Variable)]
		public FsmInt sceneCountInBuildSettings;

		public override void Reset()
		{
			sceneCountInBuildSettings = null;
		}

		public override void OnEnter()
		{
			DoGetSceneCountInBuildSettings();
			Finish();
		}

		private void DoGetSceneCountInBuildSettings()
		{
			sceneCountInBuildSettings.Value = SceneManager.sceneCountInBuildSettings;
		}
	}
}
