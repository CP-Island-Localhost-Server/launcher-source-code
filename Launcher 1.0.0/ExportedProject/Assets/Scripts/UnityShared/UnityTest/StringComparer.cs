using System;

namespace UnityTest
{
	public class StringComparer : ComparerBaseGeneric<string>
	{
		public enum CompareType
		{
			Equal = 0,
			NotEqual = 1,
			Shorter = 2,
			Longer = 3
		}

		public CompareType compareType;

		public StringComparison comparisonType = StringComparison.Ordinal;

		public bool ignoreCase = false;

		protected override bool Compare(string a, string b)
		{
			if (ignoreCase)
			{
				a = a.ToLower();
				b = b.ToLower();
			}
			switch (compareType)
			{
			case CompareType.Equal:
				return string.Compare(a, b, comparisonType) == 0;
			case CompareType.NotEqual:
				return string.Compare(a, b, comparisonType) != 0;
			case CompareType.Longer:
				return string.Compare(a, b, comparisonType) > 0;
			case CompareType.Shorter:
				return string.Compare(a, b, comparisonType) < 0;
			default:
				throw new Exception();
			}
		}
	}
}
