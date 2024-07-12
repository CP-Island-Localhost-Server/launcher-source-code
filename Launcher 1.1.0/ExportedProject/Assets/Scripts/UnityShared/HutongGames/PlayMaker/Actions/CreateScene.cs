using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Create an empty new scene with the given name additively. The path of the new scene will be empty")]
	[ActionCategory(ActionCategory.Scene)]
	public class CreateScene : FsmStateAction
	{
		[Tooltip("The name of the new scene. It cannot be empty or null, or same as the name of the existing scenes.")]
		[RequiredField]
		public FsmString sceneName;

		public override void Reset()
		{
			sceneName = null;
		}

		public override void OnEnter()
		{
			SceneManager.CreateScene(sceneName.Value);
			Finish();
		}
	}
}
