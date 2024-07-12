namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Add an item to the end of an Array.")]
	public class ArrayAdd : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The Array Variable to use.")]
		public FsmArray array;

		[RequiredField]
		[MatchElementType("array")]
		[Tooltip("Item to add.")]
		public FsmVar value;

		public override void Reset()
		{
			array = null;
			value = null;
		}

		public override void OnEnter()
		{
			DoAddValue();
			Finish();
		}

		private void DoAddValue()
		{
			array.Resize(array.Length + 1);
			value.UpdateValue();
			array.Set(array.Length - 1, value.GetValue());
		}
	}
}
