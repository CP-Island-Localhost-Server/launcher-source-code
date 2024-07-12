using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a Character Controller on a Game Object was touching the ground during the last move.")]
	[ActionCategory(ActionCategory.Character)]
	public class ControllerIsGrounded : FsmStateAction
	{
		[CheckForComponent(typeof(CharacterController))]
		[RequiredField]
		[Tooltip("The GameObject to check.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Event to send if touching the ground.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if not touching the ground.")]
		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a bool variable.")]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		private GameObject previousGo;

		private CharacterController controller;

		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoControllerIsGrounded();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoControllerIsGrounded();
		}

		private void DoControllerIsGrounded()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				if (ownerDefaultTarget != previousGo)
				{
					controller = ownerDefaultTarget.GetComponent<CharacterController>();
					previousGo = ownerDefaultTarget;
				}
				if (!(controller == null))
				{
					bool isGrounded = controller.isGrounded;
					storeResult.Value = isGrounded;
					base.Fsm.Event(isGrounded ? trueEvent : falseEvent);
				}
			}
		}
	}
}
