using System;
using UnityEngine;

namespace UnityTest
{
	public class Vector2Comparer : VectorComparerBase<Vector2>
	{
		public enum CompareType
		{
			MagnitudeEquals = 0,
			MagnitudeNotEquals = 1
		}

		public CompareType compareType;

		public float floatingPointError = 0.0001f;

		protected override bool Compare(Vector2 a, Vector2 b)
		{
			switch (compareType)
			{
			case CompareType.MagnitudeEquals:
				return AreVectorMagnitudeEqual(a.magnitude, b.magnitude, floatingPointError);
			case CompareType.MagnitudeNotEquals:
				return !AreVectorMagnitudeEqual(a.magnitude, b.magnitude, floatingPointError);
			default:
				throw new Exception();
			}
		}

		public override int GetDepthOfSearch()
		{
			return 3;
		}
	}
}
