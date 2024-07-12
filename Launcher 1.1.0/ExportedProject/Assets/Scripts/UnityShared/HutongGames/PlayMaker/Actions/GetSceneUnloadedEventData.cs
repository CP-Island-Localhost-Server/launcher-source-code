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

		[UIHint(UIHint.Variable)]
		[Tooltip("true if the scene is valid.")]
		public FsmBool isValid;

		[UIHint(UIHint.Variable)]
		[Tooltip("true if the scene is loaded.")]
		public FsmBool isLoaded;

		[UIHint(UIHint.Variable)]
		[Tooltip("true if the scene is modified.")]
		public FsmBool isDirty;

		[UIHint(UIHint.Variable)]
		[Tooltip("The scene RootCount")]
		public FsmInt rootCount;

		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		[UIHint(UIHint.Variable)]
		[Tooltip("The scene Root GameObjects")]
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
