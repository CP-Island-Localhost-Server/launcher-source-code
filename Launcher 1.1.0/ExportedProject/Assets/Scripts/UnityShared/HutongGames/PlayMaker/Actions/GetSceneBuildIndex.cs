namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns the index of a scene in the Build Settings. Always returns -1 if the scene was loaded through an AssetBundle.")]
	[ActionCategory(ActionCategory.Scene)]
	public class GetSceneBuildIndex : GetSceneActionBase
	{
		[Tooltip("The scene Build Index")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		[RequiredField]
		public FsmInt buildIndex;

		public override void Reset()
		{
			base.Reset();
			buildIndex = null;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			DoGetSceneBuildIndex();
			Finish();
		}

		private void DoGetSceneBuildIndex()
		{
			if (_sceneFound)
			{
				if (!buildIndex.IsNone)
				{
					buildIndex.Value = _scene.buildIndex;
				}
				base.Fsm.Event(sceneFoundEvent);
			}
		}
	}
}
