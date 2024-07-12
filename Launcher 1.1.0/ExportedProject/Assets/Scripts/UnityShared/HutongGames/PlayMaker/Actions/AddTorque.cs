using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds torque (rotational force) to a Game Object.")]
	[ActionCategory(ActionCategory.Physics)]
	public class AddTorque : ComponentAction<Rigidbody>
	{
		[Tooltip("The GameObject to add torque to.")]
		[CheckForComponent(typeof(Rigidbody))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("A Vector3 torque. Optionally override any axis with the X, Y, Z parameters.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		[Tooltip("Torque around the X axis. To leave unchanged, set to 'None'.")]
		public FsmFloat x;

		[Tooltip("Torque around the Y axis. To leave unchanged, set to 'None'.")]
		public FsmFloat y;

		[Tooltip("Torque around the Z axis. To leave unchanged, set to 'None'.")]
		public FsmFloat z;

		[Tooltip("Apply the force in world or local space.")]
		public Space space;

		[Tooltip("The type of force to apply. See Unity Physics docs.")]
		public ForceMode forceMode;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			x = new FsmFloat
			{
				UseVariable = true
			};
			y = new FsmFloat
			{
				UseVariable = true
			};
			z = new FsmFloat
			{
				UseVariable = true
			};
			space = Space.World;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter()
		{
			DoAddTorque();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnFixedUpdate()
		{
			DoAddTorque();
		}

		private void DoAddTorque()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				Vector3 torque = (vector.IsNone ? new Vector3(x.Value, y.Value, z.Value) : vector.Value);
				if (!x.IsNone)
				{
					torque.x = x.Value;
				}
				if (!y.IsNone)
				{
					torque.y = y.Value;
				}
				if (!z.IsNone)
				{
					torque.z = z.Value;
				}
				if (space == Space.World)
				{
					base.rigidbody.AddTorque(torque, forceMode);
				}
				else
				{
					base.rigidbody.AddRelativeTorque(torque, forceMode);
				}
			}
		}
	}
}
