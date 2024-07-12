using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("GUI Vertical Slider connected to a Float Variable.")]
	[ActionCategory(ActionCategory.GUI)]
	public class GUIVerticalSlider : GUIAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatVariable;

		[RequiredField]
		public FsmFloat topValue;

		[RequiredField]
		public FsmFloat bottomValue;

		public FsmString sliderStyle;

		public FsmString thumbStyle;

		public override void Reset()
		{
			base.Reset();
			floatVariable = null;
			topValue = 100f;
			bottomValue = 0f;
			sliderStyle = "verticalslider";
			thumbStyle = "verticalsliderthumb";
			width = 0.1f;
		}

		public override void OnGUI()
		{
			base.OnGUI();
			if (floatVariable != null)
			{
				floatVariable.Value = GUI.VerticalSlider(rect, floatVariable.Value, topValue.Value, bottomValue.Value, (sliderStyle.Value != "") ? sliderStyle.Value : "verticalslider", (thumbStyle.Value != "") ? thumbStyle.Value : "verticalsliderthumb");
			}
		}
	}
}
