using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Converts a Bool value to a Color.")]
	[ActionCategory(ActionCategory.Convert)]
	public class ConvertBoolToColor : FsmStateAction
	{
		[Tooltip("The Bool variable to test.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmBool boolVariable;

		[Tooltip("The Color variable to set based on the bool variable value.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmColor colorVariable;

		[Tooltip("Color if Bool variable is false.")]
		public FsmColor falseColor;

		[Tooltip("Color if Bool variable is true.")]
		public FsmColor trueColor;

		[Tooltip("Repeat every frame. Useful if the Bool variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			boolVariable = null;
			colorVariable = null;
			falseColor = Color.black;
			trueColor = Color.white;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertBoolToColor();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertBoolToColor();
		}

		private void DoConvertBoolToColor()
		{
			colorVariable.Value = (boolVariable.Value ? trueColor.Value : falseColor.Value);
		}
	}
}
