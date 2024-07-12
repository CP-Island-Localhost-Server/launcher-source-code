namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if any of the given Bool Variables are True.")]
	[ActionCategory(ActionCategory.Logic)]
	public class BoolAnyTrue : FsmStateAction
	{
		[Tooltip("The Bool variables to check.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool[] boolVariables;

		[Tooltip("Event to send if any of the Bool variables are True.")]
		public FsmEvent sendEvent;

		[Tooltip("Store the result in a Bool variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariables = null;
			sendEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoAnyTrue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoAnyTrue();
		}

		private void DoAnyTrue()
		{
			if (boolVariables.Length == 0)
			{
				return;
			}
			storeResult.Value = false;
			for (int i = 0; i < boolVariables.Length; i++)
			{
				if (boolVariables[i].Value)
				{
					base.Fsm.Event(sendEvent);
					storeResult.Value = true;
					break;
				}
			}
		}
	}
}
