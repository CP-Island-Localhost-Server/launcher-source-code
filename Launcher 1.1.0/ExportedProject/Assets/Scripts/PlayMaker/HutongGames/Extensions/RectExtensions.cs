using UnityEngine;

namespace HutongGames.Extensions
{
	public static class RectExtensions
	{
		public static bool Contains(this Rect rect, float x, float y)
		{
			if (x > rect.xMin && x < rect.xMax && y > rect.yMin)
			{
				return y < rect.yMax;
			}
			return false;
		}

		public static bool Contains(this Rect rect1, Rect rect2)
		{
			if (rect1.xMin <= rect2.xMin && rect1.yMin <= rect2.yMin && rect1.xMax >= rect2.xMax)
			{
				return rect1.yMax >= rect2.yMax;
			}
			return false;
		}

		public static bool IntersectsWith(this Rect rect1, Rect rect2)
		{
			if (rect2.xMin <= rect1.xMax && rect2.xMax >= rect1.xMin && rect2.yMin <= rect1.yMax)
			{
				return rect2.yMax >= rect1.yMin;
			}
			return false;
		}

		public static Rect Union(this Rect rect1, Rect rect2)
		{
			return Rect.MinMaxRect(Mathf.Min(rect1.xMin, rect2.xMin), Mathf.Min(rect1.yMin, rect2.yMin), Mathf.Max(rect1.xMax, rect2.xMax), Mathf.Max(rect1.yMax, rect2.yMax));
		}

		public static Rect Scale(this Rect rect, float scale)
		{
			return new Rect(rect.x * scale, rect.y * scale, rect.width * scale, rect.height * scale);
		}

		public static Rect MinSize(this Rect rect, float minWidth, float minHeight)
		{
			return new Rect(rect.x, rect.y, Mathf.Max(rect.width, minWidth), Mathf.Max(rect.height, minHeight));
		}

		public static Rect MinSize(this Rect rect, Vector2 minSize)
		{
			return new Rect(rect.x, rect.y, Mathf.Max(rect.width, minSize.x), Mathf.Max(rect.height, minSize.y));
		}
	}
}
