using System;

namespace UnityTest
{
	public class FloatComparer : ComparerBaseGeneric<float>
	{
		public enum CompareTypes
		{
			Equal = 0,
			NotEqual = 1,
			Greater = 2,
			Less = 3
		}

		public CompareTypes compareTypes;

		public double floatingPointError = 9.999999747378752E-05;

		protected override bool Compare(float a, float b)
		{
			switch (compareTypes)
			{
			case CompareTypes.Equal:
				return (double)Math.Abs(a - b) < floatingPointError;
			case CompareTypes.NotEqual:
				return (double)Math.Abs(a - b) > floatingPointError;
			case CompareTypes.Greater:
				return a > b;
			case CompareTypes.Less:
				return a < b;
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
