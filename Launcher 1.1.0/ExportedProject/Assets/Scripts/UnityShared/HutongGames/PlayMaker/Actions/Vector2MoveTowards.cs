using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Moves a Vector2 towards a Target. Optionally sends an event when successful.")]
	public class Vector2MoveTowards : FsmStateAction
	{
		[Tooltip("The Vector2 to Move")]
		[RequiredField]
		public FsmVector2 source;

		[Tooltip("A target Vector2 to move towards.")]
		public FsmVector2 target;

		[Tooltip("The maximum movement speed. HINT: You can make this a variable to change it over time.")]
		[HasFloatSlider(0f, 20f)]
		public FsmFloat maxSpeed;

		[Tooltip("Distance at which the move is considered finished, and the Finish Event is sent.")]
		[HasFloatSlider(0f, 5f)]
		public FsmFloat finishDistance;

		[Tooltip("Event to send when the Finish Distance is reached.")]
		public FsmEvent finishEvent;

		public override void Reset()
		{
			source = null;
			target = null;
			maxSpeed = 10f;
			finishDistance = 1f;
			finishEvent = null;
		}

		public override void OnUpdate()
		{
			DoMoveTowards();
		}

		private void DoMoveTowards()
		{
			source.Value = Vector2.MoveTowards(source.Value, target.Value, maxSpeed.Value * Time.deltaTime);
			float magnitude = (source.Value - target.Value).magnitude;
			if (magnitude < finishDistance.Value)
			{
				base.Fsm.Event(finishEvent);
				Finish();
			}
		}
	}
}
