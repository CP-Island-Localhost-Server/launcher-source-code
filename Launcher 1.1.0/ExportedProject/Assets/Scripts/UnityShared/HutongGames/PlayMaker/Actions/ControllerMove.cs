using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Moves a Game Object with a Character Controller. See also Controller Simple Move. NOTE: It is recommended that you make only one call to Move or SimpleMove per frame.")]
	[ActionCategory(ActionCategory.Character)]
	public class ControllerMove : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(CharacterController))]
		[Tooltip("The GameObject to move.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The movement vector.")]
		public FsmVector3 moveVector;

		[Tooltip("Move in local or word space.")]
		public Space space;

		[Tooltip("Movement vector is defined in units per second. Makes movement frame rate independent.")]
		public FsmBool perSecond;

		private GameObject previousGo;

		private CharacterController controller;

		public override void Reset()
		{
			gameObject = null;
			moveVector = new FsmVector3
			{
				UseVariable = true
			};
			space = Space.World;
			perSecond = true;
		}

		public override void OnUpdate()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != previousGo)
			{
				controller = ownerDefaultTarget.GetComponent<CharacterController>();
				previousGo = ownerDefaultTarget;
			}
			if (controller != null)
			{
				Vector3 vector = ((space == Space.World) ? moveVector.Value : ownerDefaultTarget.transform.TransformDirection(moveVector.Value));
				if (perSecond.Value)
				{
					controller.Move(vector * Time.deltaTime);
				}
				else
				{
					controller.Move(vector);
				}
			}
		}
	}
}
