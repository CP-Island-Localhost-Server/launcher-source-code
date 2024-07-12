using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Connect a joint to a game object.")]
	[ActionCategory(ActionCategory.Physics)]
	public class SetJointConnectedBody : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Joint))]
		[Tooltip("The joint to connect. Requires a Joint component.")]
		public FsmOwnerDefault joint;

		[CheckForComponent(typeof(Rigidbody))]
		[Tooltip("The game object to connect to the Joint. Set to none to connect the Joint to the world.")]
		public FsmGameObject rigidBody;

		public override void Reset()
		{
			joint = null;
			rigidBody = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(joint);
			if (ownerDefaultTarget != null)
			{
				Joint component = ownerDefaultTarget.GetComponent<Joint>();
				if (component != null)
				{
					component.connectedBody = ((rigidBody.Value == null) ? null : rigidBody.Value.GetComponent<Rigidbody>());
				}
			}
			Finish();
		}
	}
}
