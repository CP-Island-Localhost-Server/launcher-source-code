using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Tests if a Game Object's Rigid Body 2D is Kinematic.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class IsKinematic2d : ComponentAction<Rigidbody2D>
	{
		[Tooltip("the GameObject with a Rigidbody2D attached")]
		[CheckForComponent(typeof(Rigidbody2D))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[Tooltip("Event Sent if Kinematic")]
		public FsmEvent trueEvent;

		[Tooltip("Event sent if not Kinematic")]
		public FsmEvent falseEvent;

		[Tooltip("Store the Kinematic state")]
		[UIHint(UIHint.Variable)]
		public FsmBool store;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			store = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsKinematic();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsKinematic();
		}

		private void DoIsKinematic()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (UpdateCache(ownerDefaultTarget))
			{
				bool isKinematic = base.rigidbody2d.isKinematic;
				store.Value = isKinematic;
				base.Fsm.Event(isKinematic ? trueEvent : falseEvent);
			}
		}
	}
}
