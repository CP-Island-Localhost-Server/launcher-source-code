using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class VariableTypeAttribute : Attribute
	{
		private readonly VariableType type;

		public VariableType Type
		{
			get
			{
				return type;
			}
		}

		public VariableTypeAttribute(VariableType type)
		{
			this.type = type;
		}
	}
}
