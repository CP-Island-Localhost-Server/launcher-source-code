using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Finds the Child of a GameObject by Name.\nNote, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger. If you need to specify a tag, use GetChild.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class FindChild : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject to search.")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The name of the child. Note, you can specify a path to the child, e.g., LeftShoulder/Arm/Hand/Finger")]
		public FsmString childName;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the child in a GameObject variable.")]
		[RequiredField]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			childName = "";
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoFindChild();
			Finish();
		}

		private void DoFindChild()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				Transform transform = ownerDefaultTarget.transform.Find(childName.Value);
				storeResult.Value = ((transform != null) ? transform.gameObject : null);
			}
		}
	}
}
