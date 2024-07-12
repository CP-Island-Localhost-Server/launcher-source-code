using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Get the cosine. You can use degrees, simply check on the DegToRad conversion")]
	[ActionCategory(ActionCategory.Trigonometry)]
	public class GetCosine : FsmStateAction
	{
		[Tooltip("The angle. Note: You can use degrees, simply check DegtoRad if the angle is expressed in degrees.")]
		[RequiredField]
		public FsmFloat angle;

		[Tooltip("Check on if the angle is expressed in degrees.")]
		public FsmBool DegToRad;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The angle cosinus")]
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
			DoCosine();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoCosine();
		}

		private void DoCosine()
		{
			float num = angle.Value;
			if (DegToRad.Value)
			{
				num *= (float)Math.PI / 180f;
			}
			result.Value = Mathf.Cos(num);
		}
	}
}
