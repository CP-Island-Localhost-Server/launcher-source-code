using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ActionTarget : Attribute
	{
		private readonly Type objectType;

		private readonly string fieldName;

		private readonly bool allowPrefabs;

		public Type ObjectType
		{
			get
			{
				return objectType;
			}
		}

		public string FieldName
		{
			get
			{
				return fieldName;
			}
		}

		public bool AllowPrefabs
		{
			get
			{
				return allowPrefabs;
			}
		}

		public ActionTarget(Type objectType, string fieldName = "", bool allowPrefabs = false)
		{
			this.objectType = objectType;
			this.fieldName = fieldName;
			this.allowPrefabs = allowPrefabs;
		}

		public bool IsSameAs(ActionTarget actionTarget)
		{
			if (object.ReferenceEquals(objectType, actionTarget.objectType))
			{
				return fieldName == actionTarget.fieldName;
			}
			return false;
		}

		public override string ToString()
		{
			return "ActionTarget: " + ((!object.ReferenceEquals(objectType, null)) ? objectType.FullName : "null") + " , " + ((!string.IsNullOrEmpty(fieldName)) ? fieldName : "none");
		}
	}
}
