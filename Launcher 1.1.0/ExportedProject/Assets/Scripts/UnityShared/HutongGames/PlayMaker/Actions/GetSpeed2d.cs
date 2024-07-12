using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the 2d Speed of a Game Object and stores it in a Float Variable. NOTE: The Game Object must have a rigid body 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class GetSpeed2d : ComponentAction<Rigidbody2D>
	{
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("The speed, or in technical terms: velocity magnitude")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
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
			if (!storeResult.IsNone)
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
				if (UpdateCache(ownerDefaultTarget))
				{
					storeResult.Value = base.rigidbody2d.velocity.magnitude;
				}
			}
		}
	}
}
