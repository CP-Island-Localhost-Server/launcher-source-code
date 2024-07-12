using System;

namespace UnityTest
{
	public class IntComparer : ComparerBaseGeneric<int>
	{
		public enum CompareType
		{
			Equal = 0,
			NotEqual = 1,
			Greater = 2,
			GreaterOrEqual = 3,
			Less = 4,
			LessOrEqual = 5
		}

		public CompareType compareType;

		protected override bool Compare(int a, int b)
		{
			switch (compareType)
			{
			case CompareType.Equal:
				return a == b;
			case CompareType.NotEqual:
				return a != b;
			case CompareType.Greater:
				return a > b;
			case CompareType.GreaterOrEqual:
				return a >= b;
			case CompareType.Less:
				return a < b;
			case CompareType.LessOrEqual:
				return a <= b;
			default:
				throw new Exception();
			}
		}
	}
}