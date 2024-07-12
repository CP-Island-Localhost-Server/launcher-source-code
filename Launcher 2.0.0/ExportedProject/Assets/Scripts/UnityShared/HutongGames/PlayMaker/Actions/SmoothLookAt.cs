using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Smoothly Rotates a Game Object so its forward vector points at a Target. The target can be defined as a Game Object or a world Position. If you specify both, then the position will be used as a local offset from the object's position.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SmoothLookAt : FsmStateAction
	{
		[Tooltip("The GameObject to rotate to face a target.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("A target GameObject.")]
		public FsmGameObject targetObject;

		[Tooltip("A target position. If a Target Object is defined, this is used as a local offset.")]
		public FsmVector3 targetPosition;

		[Tooltip("Used to keep the game object generally upright. If left undefined the world y axis is used.")]
		public FsmVector3 upVector;

		[Tooltip("Force the game object to remain vertical. Useful for characters.")]
		public FsmBool keepVertical;

		[HasFloatSlider(0.5f, 15f)]
		[Tooltip("How fast the look at moves.")]
		public FsmFloat speed;

		[Tooltip("Draw a line in the Scene View to the look at position.")]
		public FsmBool debug;

		[Tooltip("If the angle to the target is less than this, send the Finish Event below. Measured in degrees.")]
		public FsmFloat finishTolerance;

		[Tooltip("Event to send if the angle to target is less than the Finish Tolerance.")]
		public FsmEvent finishEvent;

		private GameObject previousGo;

		private Quaternion lastRotation;

		private Quaternion desiredRotation;

		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			targetPosition = new FsmVector3
			{
				UseVariable = true
			};
			upVector = new FsmVector3
			{
				UseVariable = true
			};
			keepVertical = true;
			debug = false;
			speed = 5f;
			finishTolerance = 1f;
			finishEvent = null;
		}

		public override void OnEnter()
		{
			previousGo = null;
		}

		public override void OnLateUpdate()
		{
			DoSmoothLookAt();
		}

		private void DoSmoothLookAt()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			GameObject value = targetObject.Value;
			if (value == null && targetPosition.IsNone)
			{
				return;
			}
			if (previousGo != ownerDefaultTarget)
			{
				lastRotation = ownerDefaultTarget.transform.rotation;
				desiredRotation = lastRotation;
				previousGo = ownerDefaultTarget;
			}
			Vector3 vector = ((!(value != null)) ? targetPosition.Value : ((!targetPosition.IsNone) ? value.transform.TransformPoint(targetPosition.Value) : value.transform.position));
			if (keepVertical.Value)
			{
				vector.y = ownerDefaultTarget.transform.position.y;
			}
			Vector3 vector2 = vector - ownerDefaultTarget.transform.position;
			if (vector2 != Vector3.zero && vector2.sqrMagnitude > 0f)
			{
				desiredRotation = Quaternion.LookRotation(vector2, upVector.IsNone ? Vector3.up : upVector.Value);
			}
			lastRotation = Quaternion.Slerp(lastRotation, desiredRotation, speed.Value * Time.deltaTime);
			ownerDefaultTarget.transform.rotation = lastRotation;
			if (debug.Value)
			{
				Debug.DrawLine(ownerDefaultTarget.transform.position, vector, Color.grey);
			}
			if (finishEvent != null)
			{
				Vector3 from = vector - ownerDefaultTarget.transform.position;
				float f = Vector3.Angle(from, ownerDefaultTarget.transform.forward);
				if (Mathf.Abs(f) <= finishTolerance.Value)
				{
					base.Fsm.Event(finishEvent);
				}
			}
		}
	}
}
