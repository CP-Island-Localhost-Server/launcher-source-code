using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the Arc Cosine. You can get the result in degrees, simply check on the RadToDeg conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetACosine : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The value of the cosine")]
		public FsmFloat Value;

		[UIHint(UIHint.Variable)]
		[Tooltip("The resulting angle. Note:If you want degrees, simply check RadToDeg")]
		[RequiredField]
		public FsmFloat angle;

		[Tooltip("Check on if you want the angle expressed in degrees.")]
		public FsmBool RadToDeg;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			angle = null;
			RadToDeg = true;
			everyFrame = false;
			Value = null;
		}

		public override void OnEnter()
		{
			DoACosine();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoACosine();
		}

		private void DoACosine()
		{
			float num = Mathf.Acos(Value.Value);
			if (RadToDeg.Value)
			{
				num *= 57.29578f;
			}
			angle.Value = num;
		}
	}
}
