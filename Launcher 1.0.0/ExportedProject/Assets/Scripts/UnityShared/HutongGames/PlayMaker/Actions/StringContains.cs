namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a String contains another String.")]
	[ActionCategory(ActionCategory.Logic)]
	public class StringContains : FsmStateAction
	{
		[Tooltip("The String variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		[RequiredField]
		[Tooltip("Test if the String variable contains this string.")]
		public FsmString containsString;

		[Tooltip("Event to send if true.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if false.")]
		public FsmEvent falseEvent;

		[Tooltip("Store the true/false result in a bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame. Useful if any of the strings are changing over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			containsString = "";
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoStringContains();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoStringContains();
		}

		private void DoStringContains()
		{
			if (!stringVariable.IsNone && !containsString.IsNone)
			{
				bool flag = stringVariable.Value.Contains(containsString.Value);
				if (storeResult != null)
				{
					storeResult.Value = flag;
				}
				if (flag && trueEvent != null)
				{
					base.Fsm.Event(trueEvent);
				}
				else if (!flag && falseEvent != null)
				{
					base.Fsm.Event(falseEvent);
				}
			}
		}
	}
}
