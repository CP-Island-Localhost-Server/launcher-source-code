using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Multiplies a Float by Time.deltaTime to use in frame-rate independent operations. E.g., 10 becomes 10 units per second.")]
	[ActionCategory(ActionCategory.Time)]
	public class PerSecond : FsmStateAction
	{
		[RequiredField]
		public FsmFloat floatValue;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat storeResult;

		public bool everyFrame;

		public override void Reset()
		{
			floatValue = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoPerSecond();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoPerSecond();
		}

		private void DoPerSecond()
		{
			if (storeResult != null)
			{
				storeResult.Value = floatValue.Value * Time.deltaTime;
			}
		}
	}
}
