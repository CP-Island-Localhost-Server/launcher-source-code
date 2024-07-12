using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the top most parent of the Game Object.\nIf the game object has no parent, returns itself.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetRoot : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject storeRoot;

		public override void Reset()
		{
			gameObject = null;
			storeRoot = null;
		}

		public override void OnEnter()
		{
			DoGetRoot();
			Finish();
		}

		private void DoGetRoot()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				storeRoot.Value = ownerDefaultTarget.transform.root.gameObject;
			}
		}
	}
}
