using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class HasFloatSliderAttribute : Attribute
	{
		private readonly float minValue;

		private readonly float maxValue;

		public float MinValue
		{
			get
			{
				return minValue;
			}
		}

		public float MaxValue
		{
			get
			{
				return maxValue;
			}
		}

		public HasFloatSliderAttribute(float minValue, float maxValue)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
		}
	}
}
