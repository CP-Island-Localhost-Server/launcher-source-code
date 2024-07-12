using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Sends an event when a swipe is detected.")]
	public class SwipeGestureEvent : FsmStateAction
	{
		[Tooltip("How far a touch has to travel to be considered a swipe. Uses normalized distance (e.g. 1 = 1 screen diagonal distance). Should generally be a very small number.")]
		public FsmFloat minSwipeDistance;

		[Tooltip("Event to send when swipe left detected.")]
		public FsmEvent swipeLeftEvent;

		[Tooltip("Event to send when swipe right detected.")]
		public FsmEvent swipeRightEvent;

		[Tooltip("Event to send when swipe up detected.")]
		public FsmEvent swipeUpEvent;

		[Tooltip("Event to send when swipe down detected.")]
		public FsmEvent swipeDownEvent;

		private float screenDiagonalSize;

		private float minSwipeDistancePixels;

		private bool touchStarted;

		private Vector2 touchStartPos;

		public override void Reset()
		{
			minSwipeDistance = 0.1f;
			swipeLeftEvent = null;
			swipeRightEvent = null;
			swipeUpEvent = null;
			swipeDownEvent = null;
		}

		public override void OnEnter()
		{
			screenDiagonalSize = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
			minSwipeDistancePixels = minSwipeDistance.Value * screenDiagonalSize;
		}

		public override void OnUpdate()
		{
			if (Input.touchCount <= 0)
			{
				return;
			}
			Touch touch = Input.touches[0];
			switch (touch.phase)
			{
			case TouchPhase.Began:
				touchStarted = true;
				touchStartPos = touch.position;
				break;
			case TouchPhase.Ended:
				if (touchStarted)
				{
					TestForSwipeGesture(touch);
					touchStarted = false;
				}
				break;
			case TouchPhase.Canceled:
				touchStarted = false;
				break;
			case TouchPhase.Stationary:
				break;
			case TouchPhase.Moved:
				break;
			}
		}

		private void TestForSwipeGesture(Touch touch)
		{
			Vector2 position = touch.position;
			float num = Vector2.Distance(position, touchStartPos);
			if (num > minSwipeDistancePixels)
			{
				float x = position.y - touchStartPos.y;
				float y = position.x - touchStartPos.x;
				float num2 = 57.29578f * Mathf.Atan2(y, x);
				num2 = (360f + num2 - 45f) % 360f;
				Debug.Log(num2);
				if (num2 < 90f)
				{
					base.Fsm.Event(swipeRightEvent);
				}
				else if (num2 < 180f)
				{
					base.Fsm.Event(swipeDownEvent);
				}
				else if (num2 < 270f)
				{
					base.Fsm.Event(swipeLeftEvent);
				}
				else
				{
					base.Fsm.Event(swipeUpEvent);
				}
			}
		}
	}
}
