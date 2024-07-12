using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Reverse the order of items in an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayReverse : FsmStateAction
	{
		[Tooltip("The Array to reverse.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmArray array;

		public override void Reset()
		{
			array = null;
		}

		public override void OnEnter()
		{
			List<object> list = new List<object>(array.Values);
			list.Reverse();
			array.Values = list.ToArray();
			Finish();
		}
	}
}
