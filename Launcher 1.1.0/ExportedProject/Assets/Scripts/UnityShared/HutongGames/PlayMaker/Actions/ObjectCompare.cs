namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compare 2 Object Variables and send events based on the result.")]
	public class ObjectCompare : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmObject objectVariable;

		[RequiredField]
		public FsmObject compareTo;

		[Tooltip("Event to send if the 2 object values are equal.")]
		public FsmEvent equalEvent;

		[Tooltip("Event to send if the 2 object values are not equal.")]
		public FsmEvent notEqualEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a variable.")]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			objectVariable = null;
			compareTo = null;
			storeResult = null;
			equalEvent = null;
			notEqualEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoObjectCompare();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoObjectCompare();
		}

		private void DoObjectCompare()
		{
			bool flag = objectVariable.Value == compareTo.Value;
			storeResult.Value = flag;
			base.Fsm.Event(flag ? equalEvent : notEqualEvent);
		}
	}
}
