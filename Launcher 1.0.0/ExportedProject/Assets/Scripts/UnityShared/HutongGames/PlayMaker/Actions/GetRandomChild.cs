using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets a Random Child of a Game Object.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetRandomChild : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoGetRandomChild();
			Finish();
		}

		private void DoGetRandomChild()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				int childCount = ownerDefaultTarget.transform.childCount;
				if (childCount != 0)
				{
					storeResult.Value = ownerDefaultTarget.transform.GetChild(Random.Range(0, childCount)).gameObject;
				}
			}
		}
	}
}
