namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a scene path.")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetScenePath : GetSceneActionBase
	{
		[Tooltip("The scene path")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		[RequiredField]
		public FsmString path;

		public override void Reset()
		{
			base.Reset();
			path = null;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoGetScenePath();
			Finish();
		}

		private void DoGetScenePath()
		{
			if (_sceneFound)
			{
				if (!path.IsNone)
				{
					path.Value = _scene.path;
				}
				base.Fsm.Event(sceneFoundEvent);
			}
		}
	}
}
