using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets an Integer Variable to a random value between Min/Max.")]
	[ActionCategory(ActionCategory.Math)]
	public class RandomInt : FsmStateAction
	{
		[RequiredField]
		public FsmInt min;

		[RequiredField]
		public FsmInt max;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;

		[Tooltip("Should the Max value be included in the possible results?")]
		public bool inclusiveMax;

		public override void Reset()
		{
			min = 0;
			max = 100;
			storeResult = null;
			inclusiveMax = false;
		}

		public override void OnEnter()
		{
			storeResult.Value = (inclusiveMax ? Random.Range(min.Value, max.Value + 1) : Random.Range(min.Value, max.Value));
			Finish();
		}
	}
}
