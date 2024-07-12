using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Child of a GameObject by Index.\nE.g., O to get the first child. HINT: Use this with an integer variable to iterate through children.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetChildNum : FsmStateAction
	{
		[Tooltip("The GameObject to search.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The index of the child to find.")]
		[RequiredField]
		public FsmInt childIndex;

		[RequiredField]
		[Tooltip("Store the child in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject store;

		public override void Reset()
		{
			gameObject = null;
			childIndex = 0;
			store = null;
		}

		public override void OnEnter()
		{
			store.Value = DoGetChildNum(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private GameObject DoGetChildNum(GameObject go)
		{
			return (go == null) ? null : go.transform.GetChild(childIndex.Value % go.transform.childCount).gameObject;
		}
	}
}
