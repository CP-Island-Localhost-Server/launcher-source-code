using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class MatchElementTypeAttribute : Attribute
	{
		private readonly string fieldName;

		public string FieldName
		{
			get
			{
				return fieldName;
			}
		}

		public MatchElementTypeAttribute(string fieldName)
		{
			this.fieldName = fieldName;
		}
	}
}
