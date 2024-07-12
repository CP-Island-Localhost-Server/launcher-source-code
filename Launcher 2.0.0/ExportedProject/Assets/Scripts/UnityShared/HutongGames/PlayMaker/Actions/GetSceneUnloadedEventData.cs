using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Scene)]
	[Tooltip("Get the last Unloaded Scene Event data when event was sent from the action 'SendSceneUnloadedEvent")]
	public class GetSceneUnloadedEventData : FsmStateAction
	{
		[Tooltip("The scene name")]
		[UIHint(UIHint.Variable)]
		public FsmString name;

		[Tooltip("The scene path")]
		[UIHint(UIHint.Variable)]
		public FsmString path;

		[UIHint(UIHint.Variable)]
		[Tooltip("The scene Build Index")]
		public FsmInt buildIndex;

		[Tooltip("true if the scene is valid.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isValid;

		[Tooltip("true if the scene is loaded.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isLoaded;

		[Tooltip("true if the scene is modified.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isDirty;

		[Tooltip("The scene RootCount")]
		[UIHint(UIHint.Variable)]
		public FsmInt rootCount;

		[UIHint(UIHint.Variable)]
		[Tooltip("The scene Root GameObjects")]
		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		public FsmArray rootGameObjects;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		private Scene _scene;

		public override void Reset()
		{
			name = null;
			path = null;
			buildIndex = null;
			isLoaded = null;
			rootCount = null;
			rootGameObjects = null;
			isDirty = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetSceneProperties();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetSceneProperties();
		}

		private void DoGetSceneProperties()
		{
			_scene = SendSceneUnloadedEvent.lastUnLoadedScene;
			if (!name.IsNone)
			{
				name.Value = _scene.name;
			}
			if (!buildIndex.IsNone)
			{
				buildIndex.Value = _scene.buildIndex;
			}
			if (!path.IsNone)
			{
				path.Value = _scene.path;
			}
			if (!isValid.IsNone)
			{
				isValid.Value = _scene.IsValid();
			}
			if (!isDirty.IsNone)
			{
				isDirty.Value = _scene.isDirty;
			}
			if (!isLoaded.IsNone)
			{
				isLoaded.Value = _scene.isLoaded;
			}
			if (!rootCount.IsNone)
			{
				rootCount.Value = _scene.rootCount;
			}
			if (!rootGameObjects.IsNone)
			{
				if (_scene.IsValid())
				{
					rootGameObjects.Values = _scene.GetRootGameObjects();
				}
				else
				{
					rootGameObjects.Resize(0);
				}
			}
		}
	}
}
