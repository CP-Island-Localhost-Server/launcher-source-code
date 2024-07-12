using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the sine. You can use degrees, simply check on the DegToRad conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetSine : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The angle. Note: You can use degrees, simply check DegtoRad if the angle is expressed in degrees.")]
		public FsmFloat angle;

		[Tooltip("Check on if the angle is expressed in degrees.")]
		public FsmBool DegToRad;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The angle tan")]
		public FsmFloat result;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			angle = null;
			DegToRad = true;
			everyFrame = false;
			result = null;
		}

		public override void OnEnter()
		{
			DoSine();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoSine();
		}

		private void DoSine()
		{
			float num = angle.Value;
			if (DegToRad.Value)
			{
				num *= (float)Math.PI / 180f;
			}
			result.Value = Mathf.Sin(num);
		}
	}
}
