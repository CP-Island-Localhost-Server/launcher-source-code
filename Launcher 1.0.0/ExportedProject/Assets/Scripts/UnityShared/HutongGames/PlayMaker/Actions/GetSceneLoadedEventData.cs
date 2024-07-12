using UnityEngine.SceneManagement;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the last Loaded Scene Event data when event was sent from the action 'SendSceneLoadedEvent")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetSceneLoadedEventData : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(LoadSceneMode))]
		[Tooltip("The scene loaded mode")]
		public FsmEnum loadedMode;

		[Tooltip("The scene name")]
		[UIHint(UIHint.Variable)]
		public FsmString name;

		[UIHint(UIHint.Variable)]
		[Tooltip("The scene path")]
		public FsmString path;

		[Tooltip("true if the scene is valid.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isValid;

		[UIHint(UIHint.Variable)]
		[Tooltip("The scene Build Index")]
		public FsmInt buildIndex;

		[Tooltip("true if the scene is loaded.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isLoaded;

		[UIHint(UIHint.Variable)]
		[Tooltip("true if the scene is modified.")]
		public FsmBool isDirty;

		[Tooltip("The scene RootCount")]
		[UIHint(UIHint.Variable)]
		public FsmInt rootCount;

		[UIHint(UIHint.Variable)]
		[Tooltip("The scene Root GameObjects")]
		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		public FsmArray rootGameObjects;

		private Scene _scene;

		public override void Reset()
		{
			loadedMode = null;
			name = null;
			path = null;
			isValid = null;
			buildIndex = null;
			isLoaded = null;
			rootCount = null;
			rootGameObjects = null;
			isDirty = null;
		}

		public override void OnEnter()
		{
			DoGetSceneProperties();
			Finish();
		}

		private void DoGetSceneProperties()
		{
			_scene = SendSceneLoadedEvent.lastLoadedScene;
			if (!name.IsNone)
			{
				loadedMode.Value = SendSceneLoadedEvent.lastLoadedMode;
			}
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
