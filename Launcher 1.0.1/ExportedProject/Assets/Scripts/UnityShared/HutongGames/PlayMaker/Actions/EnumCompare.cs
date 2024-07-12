namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Compares 2 Enum values and sends Events based on the result.")]
	[ActionCategory(ActionCategory.Logic)]
	public class EnumCompare : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmEnum enumVariable;

		[MatchFieldType("enumVariable")]
		public FsmEnum compareTo;

		public FsmEvent equalEvent;

		public FsmEvent notEqualEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the true/false result in a bool variable.")]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame. Useful if the enum is changing over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			enumVariable = null;
			compareTo = null;
			equalEvent = null;
			notEqualEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoEnumCompare();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoEnumCompare();
		}

		private void DoEnumCompare()
		{
			if (enumVariable != null && compareTo != null)
			{
				bool flag = object.Equals(enumVariable.Value, compareTo.Value);
				if (storeResult != null)
				{
					storeResult.Value = flag;
				}
				if (flag && equalEvent != null)
				{
					base.Fsm.Event(equalEvent);
				}
				else if (!flag && notEqualEvent != null)
				{
					base.Fsm.Event(notEqualEvent);
				}
			}
		}
	}
}
