using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Samples a Color on a continuous Colors gradient.")]
	[ActionCategory(ActionCategory.Color)]
	public class ColorRamp : FsmStateAction
	{
		[Tooltip("Array of colors to defining the gradient.")]
		[RequiredField]
		public FsmColor[] colors;

		[RequiredField]
		[Tooltip("Point on the gradient to sample. Should be between 0 and the number of colors in the gradient.")]
		public FsmFloat sampleAt;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the sampled color in a Color variable.")]
		public FsmColor storeColor;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			colors = new FsmColor[3];
			sampleAt = 0f;
			storeColor = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoColorRamp();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoColorRamp();
		}

		private void DoColorRamp()
		{
			if (colors != null && colors.Length != 0 && sampleAt != null && storeColor != null)
			{
				float num = Mathf.Clamp(sampleAt.Value, 0f, colors.Length - 1);
				Color value;
				if (num == 0f)
				{
					value = colors[0].Value;
				}
				else if (num == (float)colors.Length)
				{
					value = colors[colors.Length - 1].Value;
				}
				else
				{
					Color value2 = colors[Mathf.FloorToInt(num)].Value;
					Color value3 = colors[Mathf.CeilToInt(num)].Value;
					num -= Mathf.Floor(num);
					value = Color.Lerp(value2, value3, num);
				}
				storeColor.Value = value;
			}
		}

		public override string ErrorCheck()
		{
			if (colors.Length < 2)
			{
				return "Define at least 2 colors to make a gradient.";
			}
			return null;
		}
	}
}
