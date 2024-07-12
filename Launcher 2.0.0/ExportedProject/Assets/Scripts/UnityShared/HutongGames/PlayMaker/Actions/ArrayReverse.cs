using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Array)]
	[Tooltip("Reverse the order of items in an Array.")]
	public class ArrayReverse : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The Array to reverse.")]
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
