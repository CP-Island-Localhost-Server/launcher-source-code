using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rotates a Vector2 direction from Current towards Target.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2RotateTowards : FsmStateAction
	{
		[Tooltip("The current direction. This will be the result of the rotation as well.")]
		[RequiredField]
		public FsmVector2 currentDirection;

		[RequiredField]
		[Tooltip("The direction to reach")]
		public FsmVector2 targetDirection;

		[Tooltip("Rotation speed in degrees per second")]
		[RequiredField]
		public FsmFloat rotateSpeed;

		private Vector3 current;

		private Vector3 target;

		public override void Reset()
		{
			currentDirection = new FsmVector2
			{
				UseVariable = true
			};
			targetDirection = new FsmVector2
			{
				UseVariable = true
			};
			rotateSpeed = 360f;
		}

		public override void OnEnter()
		{
			current = new Vector3(currentDirection.Value.x, currentDirection.Value.y, 0f);
			target = new Vector3(targetDirection.Value.x, targetDirection.Value.y, 0f);
		}

		public override void OnUpdate()
		{
			current.x = currentDirection.Value.x;
			current.y = currentDirection.Value.y;
			current = Vector3.RotateTowards(current, target, rotateSpeed.Value * ((float)Math.PI / 180f) * Time.deltaTime, 1000f);
			currentDirection.Value = new Vector2(current.x, current.y);
		}
	}
}
