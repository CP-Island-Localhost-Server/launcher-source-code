using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Subtracts a value from a Float Variable.")]
	public class FloatSubtract : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The float variable to subtract from.")]
		public FsmFloat floatVariable;

		[RequiredField]
		[Tooltip("Value to subtract from the float variable.")]
		public FsmFloat subtract;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		[Tooltip("Used with Every Frame. Adds the value over one second to make the operation frame rate independent.")]
		public bool perSecond;

		public override void Reset()
		{
			floatVariable = null;
			subtract = null;
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoFloatSubtract();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatSubtract();
		}

		private void DoFloatSubtract()
		{
			if (!perSecond)
			{
				floatVariable.Value -= subtract.Value;
			}
			else
			{
				floatVariable.Value -= subtract.Value * Time.deltaTime;
			}
		}
	}
}
