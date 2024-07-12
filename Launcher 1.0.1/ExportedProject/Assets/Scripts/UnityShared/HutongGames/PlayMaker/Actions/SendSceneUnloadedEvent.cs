using UnityEngine;
using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Send an event when a scene was unloaded.")]
	[ActionCategory(ActionCategory.Scene)]
	public class SendSceneUnloadedEvent : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The event to send when scene was unloaded")]
		public FsmEvent sceneUnloaded;

		public static Scene lastUnLoadedScene;

		public override void Reset()
		{
			sceneUnloaded = null;
		}

		public override void OnEnter()
		{
			SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
			Finish();
		}

		private void SceneManager_sceneUnloaded(Scene scene)
		{
			Debug.Log(scene.name);
			lastUnLoadedScene = scene;
			base.Fsm.Event(sceneUnloaded);
			Finish();
		}

		public override void OnExit()
		{
			SceneManager.sceneUnloaded -= SceneManager_sceneUnloaded;
		}
	}
}
