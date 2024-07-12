using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets the Mass of a Game Object's Rigid Body.")]
	public class GetMass : ComponentAction<Rigidbody>
	{
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		[Tooltip("The GameObject that owns the Rigidbody")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Store the mass in a float variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
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
				storeResult.Value = base.rigidbody.mass;
			}
		}
	}
}
