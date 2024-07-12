using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rigid bodies start sleeping when they come to rest. This action wakes up all rigid bodies in the scene. E.g., if you Set Gravity and want objects at rest to respond.")]
	[ActionCategory(ActionCategory.Physics)]
	public class WakeAllRigidBodies : FsmStateAction
	{
		public bool everyFrame;

		private Rigidbody[] bodies;

		public override void Reset()
		{
			everyFrame = false;
		}

		public override void OnEnter()
		{
			bodies = Object.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
			DoWakeAll();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoWakeAll();
		}

		private void DoWakeAll()
		{
			bodies = Object.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
			if (bodies != null)
			{
				Rigidbody[] array = bodies;
				foreach (Rigidbody rigidbody in array)
				{
					rigidbody.WakeUp();
				}
			}
		}
	}
}
