namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a scene name.")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetSceneName : GetSceneActionBase
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The scene name")]
		[ActionSection("Result")]
		public FsmString name;

		public override void Reset()
		{
			base.Reset();
			name = null;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoGetSceneName();
			Finish();
		}

		private void DoGetSceneName()
		{
			if (_sceneFound)
			{
				if (!name.IsNone)
				{
					name.Value = _scene.name;
				}
				base.Fsm.Event(sceneFoundEvent);
			}
		}
	}
}
