using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Clamps the value of Float Variable to a Min/Max range.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatClamp : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Float variable to clamp.")]
		[RequiredField]
		public FsmFloat floatVariable;

		[Tooltip("The minimum value.")]
		[RequiredField]
		public FsmFloat minValue;

		[RequiredField]
		[Tooltip("The maximum value.")]
		public FsmFloat maxValue;

		[Tooltip("Repeate every frame. Useful if the float variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariable = null;
			minValue = null;
			maxValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoClamp();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoClamp();
		}

		private void DoClamp()
		{
			floatVariable.Value = Mathf.Clamp(floatVariable.Value, minValue.Value, maxValue.Value);
		}
	}
}
