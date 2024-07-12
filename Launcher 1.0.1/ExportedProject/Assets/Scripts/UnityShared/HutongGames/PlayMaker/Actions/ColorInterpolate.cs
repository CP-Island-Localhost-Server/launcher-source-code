using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Color)]
	[Tooltip("Interpolate through an array of Colors over a specified amount of Time.")]
	public class ColorInterpolate : FsmStateAction
	{
		[Tooltip("Array of colors to interpolate through.")]
		[RequiredField]
		public FsmColor[] colors;

		[Tooltip("Interpolation time.")]
		[RequiredField]
		public FsmFloat time;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the interpolated color in a Color variable.")]
		public FsmColor storeColor;

		[Tooltip("Event to send when the interpolation finishes.")]
		public FsmEvent finishEvent;

		[Tooltip("Ignore TimeScale")]
		public bool realTime;

		private float startTime;

		private float currentTime;

		public override void Reset()
		{
			colors = new FsmColor[3];
			time = 1f;
			storeColor = null;
			finishEvent = null;
			realTime = false;
		}

		public override void OnEnter()
		{
			startTime = FsmTime.RealtimeSinceStartup;
			currentTime = 0f;
			if (colors.Length < 2)
			{
				if (colors.Length == 1)
				{
					storeColor.Value = colors[0].Value;
				}
				Finish();
			}
			else
			{
				storeColor.Value = colors[0].Value;
			}
		}

		public override void OnUpdate()
		{
			if (realTime)
			{
				currentTime = FsmTime.RealtimeSinceStartup - startTime;
			}
			else
			{
				currentTime += Time.deltaTime;
			}
			if (currentTime > time.Value)
			{
				Finish();
				storeColor.Value = colors[colors.Length - 1].Value;
				if (finishEvent != null)
				{
					base.Fsm.Event(finishEvent);
				}
				return;
			}
			float num = (float)(colors.Length - 1) * currentTime / time.Value;
			Color value;
			if (num.Equals(0f))
			{
				value = colors[0].Value;
			}
			else if (num.Equals(colors.Length - 1))
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

		public override string ErrorCheck()
		{
			return (colors.Length < 2) ? "Define at least 2 colors to make a gradient." : null;
		}
	}
}
