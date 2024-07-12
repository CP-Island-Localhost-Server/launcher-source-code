using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the isTrigger option of a Collider2D. Optionally set all collider2D found on the gameobject Target.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class SetCollider2dIsTrigger : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject with the Collider2D attached")]
		[CheckForComponent(typeof(Collider2D))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The flag value")]
		public FsmBool isTrigger;

		[Tooltip("Set all Colliders on the GameObject target")]
		public bool setAllColliders;

		public override void Reset()
		{
			gameObject = null;
			isTrigger = false;
			setAllColliders = false;
		}

		public override void OnEnter()
		{
			DoSetIsTrigger();
			Finish();
		}

		private void DoSetIsTrigger()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (setAllColliders)
			{
				Collider2D[] components = ownerDefaultTarget.GetComponents<Collider2D>();
				Collider2D[] array = components;
				foreach (Collider2D collider2D in array)
				{
					collider2D.isTrigger = isTrigger.Value;
				}
			}
			else if (ownerDefaultTarget.GetComponent<Collider2D>() != null)
			{
				ownerDefaultTarget.GetComponent<Collider2D>().isTrigger = isTrigger.Value;
			}
		}
	}
}
