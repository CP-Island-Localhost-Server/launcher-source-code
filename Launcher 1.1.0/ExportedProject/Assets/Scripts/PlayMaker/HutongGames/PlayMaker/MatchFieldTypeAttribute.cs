using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MatchFieldTypeAttribute : Attribute
	{
		private readonly string fieldName;

		public string FieldName
		{
			get
			{
				return fieldName;
			}
		}

		public MatchFieldTypeAttribute(string fieldName)
		{
			this.fieldName = fieldName;
		}
	}
}
