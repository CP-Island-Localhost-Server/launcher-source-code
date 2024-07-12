using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Connect a joint to a game object.")]
	public class SetJointConnectedBody : FsmStateAction
	{
		[Tooltip("The joint to connect. Requires a Joint component.")]
		[RequiredField]
		[CheckForComponent(typeof(Joint))]
		public FsmOwnerDefault joint;

		[Tooltip("The game object to connect to the Joint. Set to none to connect the Joint to the world.")]
		[CheckForComponent(typeof(Rigidbody))]
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
