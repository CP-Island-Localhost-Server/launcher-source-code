namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sends Events based on the value of a Boolean Variable.")]
	[ActionCategory(ActionCategory.Logic)]
	public class BoolTest : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Bool variable to test.")]
		public FsmBool boolVariable;

		[Tooltip("Event to send if the Bool variable is True.")]
		public FsmEvent isTrue;

		[Tooltip("Event to send if the Bool variable is False.")]
		public FsmEvent isFalse;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			isTrue = null;
			isFalse = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			base.Fsm.Event(boolVariable.Value ? isTrue : isFalse);
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			base.Fsm.Event(boolVariable.Value ? isTrue : isFalse);
		}
	}
}
