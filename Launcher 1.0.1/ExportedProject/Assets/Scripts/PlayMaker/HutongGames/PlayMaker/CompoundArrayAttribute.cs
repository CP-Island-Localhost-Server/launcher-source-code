using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class CompoundArrayAttribute : Attribute
	{
		private readonly string name;

		private readonly string firstArrayName;

		private readonly string secondArrayName;

		public string Name
		{
			get
			{
				return name;
			}
		}

		public string FirstArrayName
		{
			get
			{
				return firstArrayName;
			}
		}

		public string SecondArrayName
		{
			get
			{
				return secondArrayName;
			}
		}

		public CompoundArrayAttribute(string name, string firstArrayName, string secondArrayName)
		{
			this.name = name;
			this.firstArrayName = firstArrayName;
			this.secondArrayName = secondArrayName;
		}
	}
}
