namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a scene Root GameObjects.")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetSceneRootGameObjects : GetSceneActionBase
	{
		[ActionSection("Result")]
		[ArrayEditor(VariableType.GameObject, "", 0, 0, 65536)]
		[Tooltip("The scene Root GameObjects")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray rootGameObjects;

		[Tooltip("Repeat every Frame")]
		public bool everyFrame;

		public override void Reset()
		{
			base.Reset();
			rootGameObjects = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoGetSceneRootGameObjects();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetSceneRootGameObjects();
		}

		private void DoGetSceneRootGameObjects()
		{
			if (_sceneFound)
			{
				if (!rootGameObjects.IsNone)
				{
					rootGameObjects.Values = _scene.GetRootGameObjects();
				}
				base.Fsm.Event(sceneFoundEvent);
			}
		}
	}
}
