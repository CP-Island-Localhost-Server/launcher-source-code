using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Performs math operation on 2 Integers: Add, Subtract, Multiply, Divide, Min, Max.")]
	[ActionCategory(ActionCategory.Math)]
	public class IntOperator : FsmStateAction
	{
		public enum Operation
		{
			Add = 0,
			Subtract = 1,
			Multiply = 2,
			Divide = 3,
			Min = 4,
			Max = 5
		}

		[RequiredField]
		public FsmInt integer1;

		[RequiredField]
		public FsmInt integer2;

		public Operation operation = Operation.Add;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			integer1 = null;
			integer2 = null;
			operation = Operation.Add;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIntOperator();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIntOperator();
		}

		private void DoIntOperator()
		{
			int value = integer1.Value;
			int value2 = integer2.Value;
			switch (operation)
			{
			case Operation.Add:
				storeResult.Value = value + value2;
				break;
			case Operation.Subtract:
				storeResult.Value = value - value2;
				break;
			case Operation.Multiply:
				storeResult.Value = value * value2;
				break;
			case Operation.Divide:
				storeResult.Value = value / value2;
				break;
			case Operation.Min:
				storeResult.Value = Mathf.Min(value, value2);
				break;
			case Operation.Max:
				storeResult.Value = Mathf.Max(value, value2);
				break;
			}
		}
	}
}
