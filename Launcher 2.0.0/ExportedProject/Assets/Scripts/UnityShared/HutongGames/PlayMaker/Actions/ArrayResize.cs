namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Resize an array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayResize : FsmStateAction
	{
		[Tooltip("The Array Variable to resize")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		[Tooltip("The new size of the array.")]
		public FsmInt newSize;

		[Tooltip("The event to trigger if the new size is out of range")]
		public FsmEvent sizeOutOfRangeEvent;

		public override void OnEnter()
		{
			if (newSize.Value >= 0)
			{
				array.Resize(newSize.Value);
			}
			else
			{
				LogError("Size out of range: " + newSize.Value);
				base.Fsm.Event(sizeOutOfRangeEvent);
			}
			Finish();
		}
	}
}
