namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the number of items in an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayLength : FsmStateAction
	{
		[Tooltip("The Array Variable.")]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		[Tooltip("Store the length in an Int Variable.")]
		[UIHint(UIHint.Variable)]
		public FsmInt length;

		[Tooltip("Repeat every frame. Useful if the array is changing and you're waiting for a particular length.")]
		public bool everyFrame;

		public override void Reset()
		{
			array = null;
			length = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			length.Value = array.Length;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			length.Value = array.Length;
		}
	}
}
