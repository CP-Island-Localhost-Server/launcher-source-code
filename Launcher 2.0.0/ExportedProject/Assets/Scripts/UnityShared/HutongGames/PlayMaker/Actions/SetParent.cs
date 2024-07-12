using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Parent of a Game Object.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class SetParent : FsmStateAction
	{
		[Tooltip("The Game Object to parent.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The new parent for the Game Object.")]
		public FsmGameObject parent;

		[Tooltip("Set the local position to 0,0,0 after parenting.")]
		public FsmBool resetLocalPosition;

		[Tooltip("Set the local rotation to 0,0,0 after parenting.")]
		public FsmBool resetLocalRotation;

		public override void Reset()
		{
			gameObject = null;
			parent = null;
			resetLocalPosition = null;
			resetLocalRotation = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				ownerDefaultTarget.transform.parent = ((parent.Value == null) ? null : parent.Value.transform);
				if (resetLocalPosition.Value)
				{
					ownerDefaultTarget.transform.localPosition = Vector3.zero;
				}
				if (resetLocalRotation.Value)
				{
					ownerDefaultTarget.transform.localRotation = Quaternion.identity;
				}
			}
			Finish();
		}
	}
}
