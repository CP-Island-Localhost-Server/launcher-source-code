using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmObject : NamedVariable
	{
		[SerializeField]
		private string typeName;

		[SerializeField]
		private UnityEngine.Object value;

		private Type objectType;

		public override Type ObjectType
		{
			get
			{
				if (object.ReferenceEquals(objectType, null))
				{
					if (string.IsNullOrEmpty(typeName))
					{
						typeName = typeof(UnityEngine.Object).FullName;
					}
					objectType = ReflectionUtils.GetGlobalType(typeName);
				}
				return objectType;
			}
			set
			{
				objectType = value;
				if (object.ReferenceEquals(objectType, null))
				{
					objectType = typeof(UnityEngine.Object);
				}
				if (!object.ReferenceEquals(this.value, null))
				{
					Type type = this.value.GetType();
					if (!type.IsAssignableFrom(objectType) && !type.IsSubclassOf(objectType))
					{
						this.value = null;
					}
				}
				typeName = objectType.FullName;
			}
		}

		public string TypeName
		{
			get
			{
				return typeName;
			}
		}

		public UnityEngine.Object Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}

		public override object RawValue
		{
			get
			{
				return value;
			}
			set
			{
				this.value = (UnityEngine.Object)value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Object;
			}
		}

		public FsmObject()
		{
		}

		public FsmObject(string name)
			: base(name)
		{
			typeName = typeof(UnityEngine.Object).FullName;
			objectType = typeof(UnityEngine.Object);
		}

		public FsmObject(FsmObject source)
			: base(source)
		{
			value = source.value;
			typeName = source.typeName;
			objectType = source.objectType;
		}

		public override NamedVariable Clone()
		{
			return new FsmObject(this);
		}

		public override string ToString()
		{
			if (!(value == null))
			{
				return value.ToString();
			}
			return "None";
		}

		public static implicit operator FsmObject(UnityEngine.Object value)
		{
			FsmObject fsmObject = new FsmObject();
			fsmObject.value = value;
			return fsmObject;
		}

		public override bool TestTypeConstraint(VariableType variableType, Type _objectType = null)
		{
			if (variableType == VariableType.Unknown)
			{
				return true;
			}
			if (base.TestTypeConstraint(variableType, objectType))
			{
				if (!object.ReferenceEquals(_objectType, null) && !object.ReferenceEquals(_objectType, typeof(UnityEngine.Object)) && !object.ReferenceEquals(ObjectType, _objectType))
				{
					return _objectType.IsAssignableFrom(ObjectType);
				}
				return true;
			}
			return false;
		}
	}
}
