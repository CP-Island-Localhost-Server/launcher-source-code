namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets info on the last Character Controller collision and store in variables.")]
	[ActionCategory(ActionCategory.Character)]
	public class GetControllerHitInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the GameObject hit in the last collision.")]
		public FsmGameObject gameObjectHit;

		[Tooltip("Store the contact point of the last collision in world coordinates.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactPoint;

		[Tooltip("Store the normal of the last collision.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 contactNormal;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the direction of the last move before the collision.")]
		public FsmVector3 moveDirection;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the distance of the last move before the collision.")]
		public FsmFloat moveLength;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the physics material of the Game Object Hit. Useful for triggering different effects. Audio, particles...")]
		public FsmString physicsMaterialName;

		public override void Reset()
		{
			gameObjectHit = null;
			contactPoint = null;
			contactNormal = null;
			moveDirection = null;
			moveLength = null;
			physicsMaterialName = null;
		}

		public override void OnPreprocess()
		{
			base.Fsm.HandleControllerColliderHit = true;
		}

		private void StoreTriggerInfo()
		{
			if (base.Fsm.ControllerCollider != null)
			{
				gameObjectHit.Value = base.Fsm.ControllerCollider.gameObject;
				contactPoint.Value = base.Fsm.ControllerCollider.point;
				contactNormal.Value = base.Fsm.ControllerCollider.normal;
				moveDirection.Value = base.Fsm.ControllerCollider.moveDirection;
				moveLength.Value = base.Fsm.ControllerCollider.moveLength;
				physicsMaterialName.Value = base.Fsm.ControllerCollider.collider.material.name;
			}
		}

		public override void OnEnter()
		{
			StoreTriggerInfo();
			Finish();
		}

		public override string ErrorCheck()
		{
			return ActionHelpers.CheckOwnerPhysicsSetup(base.Owner);
		}
	}
}
