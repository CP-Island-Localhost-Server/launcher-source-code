using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rotates a Vector3 direction from Current towards Target.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3RotateTowards : FsmStateAction
	{
		[RequiredField]
		public FsmVector3 currentDirection;

		[RequiredField]
		public FsmVector3 targetDirection;

		[RequiredField]
		[Tooltip("Rotation speed in degrees per second")]
		public FsmFloat rotateSpeed;

		[Tooltip("Max Magnitude per second")]
		[RequiredField]
		public FsmFloat maxMagnitude;

		public override void Reset()
		{
			currentDirection = new FsmVector3
			{
				UseVariable = true
			};
			targetDirection = new FsmVector3
			{
				UseVariable = true
			};
			rotateSpeed = 360f;
			maxMagnitude = 1f;
		}

		public override void OnUpdate()
		{
			currentDirection.Value = Vector3.RotateTowards(currentDirection.Value, targetDirection.Value, rotateSpeed.Value * ((float)Math.PI / 180f) * Time.deltaTime, maxMagnitude.Value);
		}
	}
}
