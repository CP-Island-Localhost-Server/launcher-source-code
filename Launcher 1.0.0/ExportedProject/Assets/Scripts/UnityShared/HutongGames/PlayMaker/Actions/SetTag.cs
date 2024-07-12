using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets a Game Object's Tag.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class SetTag : FsmStateAction
	{
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Tag)]
		public FsmString tag;

		public override void Reset()
		{
			gameObject = null;
			tag = "Untagged";
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				ownerDefaultTarget.tag = tag.Value;
			}
			Finish();
		}
	}
}
