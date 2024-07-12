using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get a Random item from an Array.")]
	[ActionCategory(ActionCategory.Array)]
	public class ArrayGetRandom : FsmStateAction
	{
		[Tooltip("The Array to use.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray array;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Store the value in a variable.")]
		[MatchElementType("array")]
		public FsmVar storeValue;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			array = null;
			storeValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetRandomValue();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetRandomValue();
		}

		private void DoGetRandomValue()
		{
			if (!storeValue.IsNone)
			{
				storeValue.SetValue(array.Get(Random.Range(0, array.Length)));
			}
		}
	}
}
