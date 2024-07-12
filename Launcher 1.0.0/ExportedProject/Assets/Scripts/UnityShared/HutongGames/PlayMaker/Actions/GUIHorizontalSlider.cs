using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("GUI Horizontal Slider connected to a Float Variable.")]
	public class GUIHorizontalSlider : GUIAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmFloat leftValue;

		[RequiredField]
		public FsmFloat rightValue;

		public FsmString sliderStyle;

		public FsmString thumbStyle;

		public override void Reset()
		{
			base.Reset();
			floatVariable = null;
			leftValue = 0f;
			rightValue = 100f;
			sliderStyle = "horizontalslider";
			thumbStyle = "horizontalsliderthumb";
		}

		public override void OnGUI()
		{
			base.OnGUI();
			if (floatVariable != null)
			{
				floatVariable.Value = GUI.HorizontalSlider(rect, floatVariable.Value, leftValue.Value, rightValue.Value, (sliderStyle.Value != "") ? sliderStyle.Value : "horizontalslider", (thumbStyle.Value != "") ? thumbStyle.Value : "horizontalsliderthumb");
			}
		}
	}
}
