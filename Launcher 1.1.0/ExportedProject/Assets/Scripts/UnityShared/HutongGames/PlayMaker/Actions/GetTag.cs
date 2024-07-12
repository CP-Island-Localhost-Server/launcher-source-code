namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets a Game Object's Tag and stores it in a String Variable.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetTag : FsmStateAction
	{
		[RequiredField]
		public FsmGameObject gameObject;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetTag();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetTag();
		}

		private void DoGetTag()
		{
			if (!(gameObject.Value == null))
			{
				storeResult.Value = gameObject.Value.tag;
			}
		}
	}
}
