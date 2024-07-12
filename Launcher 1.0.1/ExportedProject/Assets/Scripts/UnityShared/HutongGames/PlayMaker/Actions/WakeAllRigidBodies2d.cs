using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Rigid bodies 2D start sleeping when they come to rest. This action wakes up all rigid bodies 2D in the scene. E.g., if you Set Gravity 2D and want objects at rest to respond.")]
	[ActionCategory(ActionCategory.Physics2D)]
	public class WakeAllRigidBodies2d : FsmStateAction
	{
		[Tooltip("Repeat every frame. Note: This would be very expensive!")]
		public bool everyFrame;

		public override void Reset()
		{
			everyFrame = false;
		}

		public override void OnEnter()
		{
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
			Rigidbody2D[] array = Object.FindObjectsOfType(typeof(Rigidbody2D)) as Rigidbody2D[];
			if (array != null)
			{
				Rigidbody2D[] array2 = array;
				foreach (Rigidbody2D rigidbody2D in array2)
				{
					rigidbody2D.WakeUp();
				}
			}
		}
	}
}
