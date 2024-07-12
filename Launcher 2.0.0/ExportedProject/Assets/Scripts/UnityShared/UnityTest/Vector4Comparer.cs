using System;
using UnityEngine;

namespace UnityTest
{
	public class Vector4Comparer : VectorComparerBase<Vector4>
	{
		public enum CompareType
		{
			MagnitudeEquals = 0,
			MagnitudeNotEquals = 1
		}

		public CompareType compareType;

		public double floatingPointError;

		protected override bool Compare(Vector4 a, Vector4 b)
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
