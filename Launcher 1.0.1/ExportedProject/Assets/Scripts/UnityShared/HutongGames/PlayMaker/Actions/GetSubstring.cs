namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets a sub-string from a String Variable.")]
	[ActionCategory(ActionCategory.String)]
	public class GetSubstring : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		[RequiredField]
		public FsmInt startIndex;

		[RequiredField]
		public FsmInt length;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			startIndex = 0;
			length = 1;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetSubstring();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetSubstring();
		}

		private void DoGetSubstring()
		{
			if (stringVariable != null && storeResult != null)
			{
				storeResult.Value = stringVariable.Value.Substring(startIndex.Value, length.Value);
			}
		}
	}
}
