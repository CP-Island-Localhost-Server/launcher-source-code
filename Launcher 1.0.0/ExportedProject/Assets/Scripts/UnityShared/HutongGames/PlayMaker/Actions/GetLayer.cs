namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets a Game Object's Layer and stores it in an Int Variable.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetLayer : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmInt storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetLayer();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetLayer();
		}

		private void DoGetLayer()
		{
			if (!(gameObject.Value == null))
			{
				storeResult.Value = gameObject.Value.layer;
			}
		}
	}
}
