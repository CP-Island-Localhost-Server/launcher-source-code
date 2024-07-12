namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Scene)]
	[Tooltip("Get a scene isDirty flag. true if the scene is modified. ")]
	public class GetSceneProperties : GetSceneActionBase
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The scene name")]
		[ActionSection("Result")]
		public FsmString name;

		[Tooltip("The scene path")]
		[UIHint(UIHint.Variable)]
		public FsmString path;

		[Tooltip("The scene Build Index")]
		[UIHint(UIHint.Variable)]
		public FsmInt buildIndex;

		[UIHint(UIHint.Variable)]
		[Tooltip("true if the scene is valid.")]
		public FsmBool isValid;

		[Tooltip("true if the scene is loaded.")]
		[UIHint(UIHint.Variable)]
		public FsmBool isLoaded;

		[UIHint(UIHint.Variable)]
		[Tooltip("true if the scene is modified.")]
		public FsmBool isDirty;

		[Tooltip("The scene RootCount")]
		[UIHint(UIHint.Variable)]
		public FsmInt rootCount;

		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		[Tooltip("The scene Root GameObjects")]
		[UIHint(UIHint.Variable)]
		public FsmArray rootGameObjects;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			base.Reset();
			name = null;
			path = null;
			buildIndex = null;
			isValid = null;
			isLoaded = null;
			rootCount = null;
			rootGameObjects = null;
			isDirty = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoGetSceneProperties();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoGetSceneProperties()
		{
			if (!_sceneFound)
			{
				return;
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
			base.Fsm.Event(sceneFoundEvent);
		}
	}
}
