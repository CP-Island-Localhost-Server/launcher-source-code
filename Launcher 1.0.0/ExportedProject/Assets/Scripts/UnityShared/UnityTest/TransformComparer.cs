using System;
using UnityEngine;

namespace UnityTest
{
	public class TransformComparer : ComparerBaseGeneric<Transform>
	{
		public enum CompareType
		{
			Equals = 0,
			NotEquals = 1
		}

		public CompareType compareType;

		protected override bool Compare(Transform a, Transform b)
		{
			if (compareType == CompareType.Equals)
			{
				return a.position == b.position;
			}
			if (compareType == CompareType.NotEquals)
			{
				return a.position != b.position;
			}
			throw new Exception();
		}
	}
}
