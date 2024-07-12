namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a scene RootCount, the number of root transforms of this scene.")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetSceneRootCount : GetSceneActionBase
	{
		[Tooltip("The scene RootCount")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		[RequiredField]
		public FsmInt rootCount;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			base.Reset();
			rootCount = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoGetSceneRootCount();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetSceneRootCount();
		}

		private void DoGetSceneRootCount()
		{
			if (_sceneFound)
			{
				if (!rootCount.IsNone)
				{
					rootCount.Value = _scene.rootCount;
				}
				base.Fsm.Event(sceneFoundEvent);
			}
		}
	}
}
