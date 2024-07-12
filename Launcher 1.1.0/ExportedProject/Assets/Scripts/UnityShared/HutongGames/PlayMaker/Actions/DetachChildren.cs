using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Unparents all children from the Game Object.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class DetachChildren : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject to unparent children from.")]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			DoDetachChildren(base.Fsm.GetOwnerDefaultTarget(gameObject));
			Finish();
		}

		private static void DoDetachChildren(GameObject go)
		{
			if (go != null)
			{
				go.transform.DetachChildren();
			}
		}
	}
}
