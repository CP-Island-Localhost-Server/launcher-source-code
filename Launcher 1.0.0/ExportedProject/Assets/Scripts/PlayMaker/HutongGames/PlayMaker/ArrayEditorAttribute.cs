using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ArrayEditorAttribute : Attribute
	{
		private readonly VariableType variableType;

		private readonly Type objectType;

		private readonly string elementName;

		private readonly int fixedSize;

		private readonly int maxSize;

		private readonly int minSize;

		public VariableType VariableType
		{
			get
			{
				return variableType;
			}
		}

		public Type ObjectType
		{
			get
			{
				return objectType;
			}
		}

		public string ElementName
		{
			get
			{
				return elementName;
			}
		}

		public int FixedSize
		{
			get
			{
				return fixedSize;
			}
		}

		public bool Resizable
		{
			get
			{
				return fixedSize == 0;
			}
		}

		public int MinSize
		{
			get
			{
				return minSize;
			}
		}

		public int MaxSize
		{
			get
			{
				return maxSize;
			}
		}

		public ArrayEditorAttribute(VariableType variableType, string elementName = "", int fixedSize = 0, int minSize = 0, int maxSize = 65536)
		{
			this.variableType = variableType;
			this.elementName = elementName;
			this.fixedSize = fixedSize;
			this.minSize = minSize;
			this.maxSize = maxSize;
		}

		public ArrayEditorAttribute(Type objectType, string elementName = "", int fixedSize = 0, int minSize = 0, int maxSize = 65536)
		{
			variableType = (objectType.IsEnum ? VariableType.Enum : VariableType.Object);
			this.objectType = objectType;
			this.elementName = elementName;
			this.fixedSize = fixedSize;
			this.minSize = minSize;
			this.maxSize = maxSize;
		}
	}
}
