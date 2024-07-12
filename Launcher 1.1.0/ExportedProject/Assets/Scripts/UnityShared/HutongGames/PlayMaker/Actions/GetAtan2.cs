using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Trigonometry)]
	[Tooltip("Get the Arc Tangent 2 as in atan2(y,x). You can get the result in degrees, simply check on the RadToDeg conversion")]
	public class GetAtan2 : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The x value of the tan")]
		public FsmFloat xValue;

		[Tooltip("The y value of the tan")]
		[RequiredField]
		public FsmFloat yValue;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		public FsmFloat angle;

		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			xValue = null;
			yValue = null;
			RadToDeg = true;
			everyFrame = false;
			angle = null;
		}

		public override void OnEnter()
		{
			DoATan();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoATan();
		}

		private void DoATan()
		{
			float num = Mathf.Atan2(yValue.Value, xValue.Value);
			if (RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			angle.Value = num;
		}
	}
}
