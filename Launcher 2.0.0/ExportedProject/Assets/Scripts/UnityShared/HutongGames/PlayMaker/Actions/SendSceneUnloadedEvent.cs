using UnityEngine;
using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Scene)]
	[Tooltip("Send an event when a scene was unloaded.")]
	public class SendSceneUnloadedEvent : FsmStateAction
	{
		[Tooltip("The event to send when scene was unloaded")]
		[RequiredField]
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
