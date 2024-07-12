using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Gets the 2d Velocity of a Game Object and stores it in a Vector2 Variable or each Axis in a Float Variable. NOTE: The Game Object must have a Rigid Body 2D.")]
	public class GetVelocity2d : ComponentAction<Rigidbody2D>
	{
		[RequiredField]
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The velocity")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector;

		[Tooltip("The x value of the velocity")]
		[UIHint(UIHint.Variable)]
		public FsmFloat x;

		[UIHint(UIHint.Variable)]
		[Tooltip("The y value of the velocity")]
		public FsmFloat y;

		[Tooltip("The space reference to express the velocity")]
		public Space space;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			vector = null;
			x = null;
			y = null;
			space = Space.World;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetVelocity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetVelocity();
		}

		private void DoGetVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				Vector2 vector = base.rigidbody2d.velocity;
				if (space == Space.Self)
				{
					vector = base.rigidbody2d.transform.InverseTransformDirection(vector);
				}
				this.vector.Value = vector;
				x.Value = vector.x;
				y.Value = vector.y;
			}
		}
	}
}
