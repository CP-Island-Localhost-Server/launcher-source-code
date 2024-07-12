namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a value at an index. Index must be between 0 and the number of items -1. First item is index 0.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayGet : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The Array Variable to use.")]
		public FsmArray array;

		[Tooltip("The index into the array.")]
		public FsmInt index;

		[Tooltip("Store the value in a variable.")]
		[RequiredField]
		[MatchElementType("array")]
		[UIHint(UIHint.Variable)]
		public FsmVar storeValue;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		[ActionSection("Events")]
		[Tooltip("The event to trigger if the index is out of range")]
		public FsmEvent indexOutOfRange;

		public override void Reset()
		{
			array = null;
			index = null;
			everyFrame = false;
			storeValue = null;
			indexOutOfRange = null;
		}

		public override void OnEnter()
		{
			DoGetValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetValue();
		}

		private void DoGetValue()
		{
			if (!array.IsNone && !storeValue.IsNone)
			{
				if (index.Value >= 0 && index.Value < array.Length)
				{
					storeValue.SetValue(array.Get(index.Value));
				}
				else
				{
					base.Fsm.Event(indexOutOfRange);
				}
			}
		}
	}
}
