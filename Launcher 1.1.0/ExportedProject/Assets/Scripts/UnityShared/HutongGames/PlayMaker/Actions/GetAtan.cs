using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the Arc Tangent. You can get the result in degrees, simply check on the RadToDeg conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetAtan : FsmStateAction
	{
		[Tooltip("The value of the tan")]
		[RequiredField]
		public FsmFloat Value;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		public FsmFloat angle;

		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		public bool everyFrame;

		public override void Reset()
		{
			Value = null;
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
			float num = Mathf.Atan(Value.Value);
			if (RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			angle.Value = num;
		}
	}
}
