namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value at an index. Index must be between 0 and the number of items -1. First item is index 0.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArraySet : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The Array Variable to use.")]
		public FsmArray array;

		[Tooltip("The index into the array.")]
		public FsmInt index;

		[Tooltip("Set the value of the array at the specified index.")]
		[RequiredField]
		[MatchElementType("array")]
		public FsmVar value;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		[ActionSection("Events")]
		[Tooltip("The event to trigger if the index is out of range")]
		public FsmEvent indexOutOfRange;

		public override void Reset()
		{
			array = null;
			index = null;
			value = null;
			everyFrame = false;
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
			if (!array.IsNone)
			{
				if (index.Value >= 0 && index.Value < array.Length)
				{
					value.UpdateValue();
					array.Set(index.Value, value.GetValue());
				}
				else
				{
					base.Fsm.Event(indexOutOfRange);
				}
			}
		}
	}
}
