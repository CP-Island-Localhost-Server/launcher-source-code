using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Speed of a Game Object and stores it in a Float Variable. NOTE: The Game Object must have a rigid body.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetSpeed : ComponentAction<Rigidbody>
	{
		[Tooltip("The GameObject with a Rigidbody.")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Store the speed in a float variable.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetSpeed();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetSpeed();
		}

		private void DoGetSpeed()
		{
			if (storeResult != null)
			{
				GameObject go = ((gameObject.OwnerOption == OwnerDefaultOption.UseOwner) ? base.Owner : gameObject.GameObject.Value);
				if (UpdateCache(go))
				{
					Vector3 velocity = base.rigidbody.velocity;
					storeResult.Value = velocity.magnitude;
				}
			}
		}
	}
}
