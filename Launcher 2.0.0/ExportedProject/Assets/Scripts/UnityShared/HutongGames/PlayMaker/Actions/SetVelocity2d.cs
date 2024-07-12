using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the 2d Velocity of a Game Object. To leave any axis unchanged, set variable to 'None'. NOTE: Game object must have a rigidbody 2D.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class SetVelocity2d : ComponentAction<Rigidbody2D>
	{
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody2D))]
		public FsmOwnerDefault gameObject;

		[Tooltip("A Vector2 value for the velocity")]
		public FsmVector2 vector;

		[Tooltip("The y value of the velocity. Overrides 'Vector' x value if set")]
		public FsmFloat x;

		[Tooltip("The y value of the velocity. Overrides 'Vector' y value if set")]
		public FsmFloat y;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			vector = null;
			x = new FsmFloat
			{
				UseVariable = true
			};
			y = new FsmFloat
			{
				UseVariable = true
			};
			everyFrame = false;
		}

		public override void Awake()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter()
		{
			DoSetVelocity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoSetVelocity();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetVelocity()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				Vector2 velocity = ((!vector.IsNone) ? vector.Value : base.rigidbody2d.velocity);
				if (!x.IsNone)
				{
					velocity.x = x.Value;
				}
				if (!y.IsNone)
				{
					velocity.y = y.Value;
				}
				base.rigidbody2d.velocity = velocity;
			}
		}
	}
}
