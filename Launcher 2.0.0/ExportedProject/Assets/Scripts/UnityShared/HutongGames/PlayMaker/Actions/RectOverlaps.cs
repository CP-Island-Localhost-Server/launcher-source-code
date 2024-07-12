using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Rect)]
	[Tooltip("Tests if 2 Rects overlap.")]
	public class RectOverlaps : FsmStateAction
	{
		[RequiredField]
		[Tooltip("First Rectangle.")]
		public FsmRect rect1;

		[Tooltip("Second Rectangle.")]
		[RequiredField]
		public FsmRect rect2;

		[Tooltip("Event to send if the Rects overlap.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if the Rects do not overlap.")]
		public FsmEvent falseEvent;

		[Tooltip("Store the result in a variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			rect1 = new FsmRect
			{
				UseVariable = true
			};
			rect2 = new FsmRect
			{
				UseVariable = true
			};
			storeResult = null;
			trueEvent = null;
			falseEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoRectOverlap();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoRectOverlap();
		}

		private void DoRectOverlap()
		{
			if (!rect1.IsNone && !rect2.IsNone)
			{
				bool flag = Intersect(rect1.Value, rect2.Value);
				storeResult.Value = flag;
				base.Fsm.Event(flag ? trueEvent : falseEvent);
			}
		}

		public static bool Intersect(Rect a, Rect b)
		{
			FlipNegative(ref a);
			FlipNegative(ref b);
			bool flag = a.xMin < b.xMax;
			bool flag2 = a.xMax > b.xMin;
			bool flag3 = a.yMin < b.yMax;
			bool flag4 = a.yMax > b.yMin;
			return flag && flag2 && flag3 && flag4;
		}

		public static void FlipNegative(ref Rect r)
		{
			if (r.width < 0f)
			{
				r.x -= (r.width *= -1f);
			}
			if (r.height < 0f)
			{
				r.y -= (r.height *= -1f);
			}
		}
	}
}
