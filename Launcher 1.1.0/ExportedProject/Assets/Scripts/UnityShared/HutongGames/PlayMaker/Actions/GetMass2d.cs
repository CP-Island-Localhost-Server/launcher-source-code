using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Mass of a Game Object's Rigid Body 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetMass2d : ComponentAction<Rigidbody2D>
	{
		[Tooltip("The GameObject with a Rigidbody2D attached.")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the mass of gameObject.")]
		public FsmFloat storeResult;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
		}

		public override void OnEnter()
		{
			DoGetMass();
			Finish();
		}

		private void DoGetMass()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				storeResult.Value = base.rigidbody2d.mass;
			}
		}
	}
}
