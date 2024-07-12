using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Position of a Game Object. To leave any axis unchanged, set variable to 'None'.")]
	[ActionCategory(ActionCategory.Transform)]
	public class SetPosition : FsmStateAction
	{
		[Tooltip("The GameObject to position.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Use a stored Vector3 position, and/or set individual axis below.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		public FsmFloat x;

		public FsmFloat y;

		public FsmFloat z;

		[Tooltip("Use local or world space.")]
		public Space space;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		[Tooltip("Perform in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;

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
			z = new FsmFloat
			{
				UseVariable = true
			};
			space = Space.Self;
			everyFrame = false;
			lateUpdate = false;
		}

		public override void OnEnter()
		{
			if (!everyFrame && !lateUpdate)
			{
				DoSetPosition();
				Finish();
			}
		}

		public override void OnUpdate()
		{
			if (!lateUpdate)
			{
				DoSetPosition();
			}
		}

		public override void OnLateUpdate()
		{
			if (lateUpdate)
			{
				DoSetPosition();
			}
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetPosition()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				Vector3 vector = ((!this.vector.IsNone) ? this.vector.Value : ((space == Space.World) ? ownerDefaultTarget.transform.position : ownerDefaultTarget.transform.localPosition));
				if (!x.IsNone)
				{
					vector.x = x.Value;
				}
				if (!y.IsNone)
				{
					vector.y = y.Value;
				}
				if (!z.IsNone)
				{
					vector.z = z.Value;
				}
				if (space == Space.World)
				{
					ownerDefaultTarget.transform.position = vector;
				}
				else
				{
					ownerDefaultTarget.transform.localPosition = vector;
				}
			}
		}
	}
}
