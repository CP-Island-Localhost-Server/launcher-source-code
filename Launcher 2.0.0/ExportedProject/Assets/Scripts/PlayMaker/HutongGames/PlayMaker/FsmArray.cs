using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmArray : NamedVariable
	{
		[SerializeField]
		private VariableType type = VariableType.Unknown;

		[SerializeField]
		private string objectTypeName;

		private Type objectType;

		public float[] floatValues;

		public int[] intValues;

		public bool[] boolValues;

		public string[] stringValues;

		public Vector4[] vector4Values;

		public UnityEngine.Object[] objectReferences;

		[NonSerialized]
		private Array sourceArray;

		[NonSerialized]
		private object[] values;

		public override object RawValue
		{
			get
			{
				return values;
			}
			set
			{
				values = (object[])value;
			}
		}

		public override Type ObjectType
		{
			get
			{
				if (object.ReferenceEquals(objectType, null))
				{
					if (string.IsNullOrEmpty(objectTypeName))
					{
						if (ElementType == VariableType.Enum)
						{
							objectTypeName = typeof(None).FullName;
						}
						else
						{
							objectTypeName = typeof(UnityEngine.Object).FullName;
						}
					}
					objectType = ReflectionUtils.GetGlobalType(objectTypeName);
				}
				return objectType;
			}
			set
			{
				if (object.ReferenceEquals(objectType, value))
				{
					return;
				}
				Reset();
				if (ElementType == VariableType.Enum)
				{
					if (object.ReferenceEquals(value, null))
					{
						value = typeof(None);
					}
					else if (!value.IsEnum)
					{
						value = typeof(None);
					}
				}
				else if (ElementType == VariableType.Object)
				{
					if (object.ReferenceEquals(value, null))
					{
						value = typeof(UnityEngine.Object);
					}
					else if (!typeof(UnityEngine.Object).IsAssignableFrom(value))
					{
						value = typeof(None);
					}
				}
				else if (object.ReferenceEquals(value, null))
				{
					value = typeof(UnityEngine.Object);
				}
				objectType = value;
				objectTypeName = objectType.FullName;
			}
		}

		public string ObjectTypeName
		{
			get
			{
				return objectTypeName;
			}
		}

		public object[] Values
		{
			get
			{
				if (values == null)
				{
					InitArray();
				}
				return values;
			}
			set
			{
				values = value;
			}
		}

		public int Length
		{
			get
			{
				return Values.Length;
			}
		}

		public override VariableType TypeConstraint
		{
			get
			{
				return type;
			}
		}

		public VariableType ElementType
		{
			get
			{
				return type;
			}
			set
			{
				SetType(value);
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Array;
			}
		}

		private void InitArray()
		{
			sourceArray = GetSourceArray();
			if (sourceArray != null)
			{
				values = new object[sourceArray.Length];
				for (int i = 0; i < values.Length; i++)
				{
					values[i] = Load(i);
				}
			}
			else
			{
				values = new object[0];
			}
		}

		public object Get(int index)
		{
			return Values[index];
		}

		public void Set(int index, object value)
		{
			Values[index] = value;
		}

		private object Load(int index)
		{
			switch (type)
			{
			case VariableType.Float:
				return floatValues[index];
			case VariableType.Int:
				return intValues[index];
			case VariableType.Bool:
				return boolValues[index];
			case VariableType.GameObject:
				return objectReferences[index] as GameObject;
			case VariableType.String:
				return stringValues[index];
			case VariableType.Vector2:
			{
				Vector4 vector = vector4Values[index];
				return new Vector2(vector.x, vector.y);
			}
			case VariableType.Vector3:
			{
				Vector4 vector = vector4Values[index];
				return new Vector3(vector.x, vector.y, vector.z);
			}
			case VariableType.Color:
			{
				Vector4 vector = vector4Values[index];
				return new Color(vector.x, vector.y, vector.z, vector.w);
			}
			case VariableType.Rect:
			{
				Vector4 vector = vector4Values[index];
				return new Rect(vector.x, vector.y, vector.z, vector.w);
			}
			case VariableType.Material:
				return objectReferences[index] as Material;
			case VariableType.Texture:
				return objectReferences[index] as Texture;
			case VariableType.Quaternion:
			{
				Vector4 vector = vector4Values[index];
				return new Quaternion(vector.x, vector.y, vector.z, vector.w);
			}
			case VariableType.Object:
				return objectReferences[index];
			case VariableType.Enum:
				return Enum.ToObject(ObjectType, (object)intValues[index]);
			case VariableType.Unknown:
				return null;
			case VariableType.Array:
				Debug.LogError("Nested arrays are not supported yet!");
				return null;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void Save(int index, object value)
		{
			switch (type)
			{
			case VariableType.Float:
				floatValues[index] = ((value != null) ? ((float)value) : 0f);
				break;
			case VariableType.Int:
				intValues[index] = ((value != null) ? ((int)value) : 0);
				break;
			case VariableType.Bool:
				boolValues[index] = value != null && (bool)value;
				break;
			case VariableType.GameObject:
				objectReferences[index] = value as GameObject;
				break;
			case VariableType.String:
				stringValues[index] = value as string;
				break;
			case VariableType.Vector2:
				vector4Values[index] = ((value != null) ? ((Vector2)value) : Vector2.zero);
				break;
			case VariableType.Vector3:
				vector4Values[index] = ((value != null) ? ((Vector3)value) : Vector3.zero);
				break;
			case VariableType.Color:
				vector4Values[index] = ((value != null) ? ((Color)value) : Color.white);
				break;
			case VariableType.Rect:
			{
				Rect rect = ((value != null) ? ((Rect)value) : new Rect(0f, 0f, 0f, 0f));
				vector4Values[index] = new Vector4(rect.x, rect.y, rect.width, rect.height);
				break;
			}
			case VariableType.Material:
				objectReferences[index] = value as Material;
				break;
			case VariableType.Texture:
				objectReferences[index] = value as Texture;
				break;
			case VariableType.Quaternion:
			{
				Quaternion quaternion = ((value != null) ? ((Quaternion)value) : Quaternion.identity);
				vector4Values[index] = new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
				break;
			}
			case VariableType.Object:
				objectReferences[index] = value as UnityEngine.Object;
				break;
			case VariableType.Enum:
				intValues[index] = Convert.ToInt32(value);
				break;
			case VariableType.Array:
				Debug.LogError("Nested arrays are not supported yet!");
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case VariableType.Unknown:
				break;
			}
		}

		public void SetType(VariableType newType)
		{
			if (type != newType)
			{
				type = newType;
				ObjectType = null;
				Reset();
				ConformSourceArraySize();
			}
		}

		public void SaveChanges()
		{
			ConformSourceArraySize();
			for (int i = 0; i < values.Length; i++)
			{
				Save(i, values[i]);
				values[i] = Load(i);
			}
		}

		public void CopyValues(FsmArray source)
		{
			if (source != null)
			{
				Resize(source.Length);
				object[] array = source.Values;
				for (int i = 0; i < array.Length; i++)
				{
					Set(i, array[i]);
				}
				SaveChanges();
			}
		}

		private void ConformSourceArraySize()
		{
			switch (type)
			{
			case VariableType.Float:
				floatValues = new float[Values.Length];
				break;
			case VariableType.Int:
			case VariableType.Enum:
				intValues = new int[Values.Length];
				break;
			case VariableType.Bool:
				boolValues = new bool[Values.Length];
				break;
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				objectReferences = new UnityEngine.Object[Values.Length];
				break;
			case VariableType.String:
				stringValues = new string[Values.Length];
				break;
			case VariableType.Vector2:
			case VariableType.Vector3:
			case VariableType.Color:
			case VariableType.Rect:
			case VariableType.Quaternion:
				vector4Values = new Vector4[Values.Length];
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case VariableType.Unknown:
			case VariableType.Array:
				break;
			}
		}

		private Array GetSourceArray()
		{
			switch (type)
			{
			case VariableType.Float:
				return floatValues ?? (floatValues = new float[0]);
			case VariableType.Int:
			case VariableType.Enum:
				return intValues ?? (intValues = new int[0]);
			case VariableType.Bool:
				return boolValues ?? (boolValues = new bool[0]);
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				return objectReferences ?? (objectReferences = new UnityEngine.Object[0]);
			case VariableType.String:
				return stringValues ?? (stringValues = new string[0]);
			case VariableType.Vector2:
			case VariableType.Vector3:
			case VariableType.Color:
			case VariableType.Rect:
			case VariableType.Quaternion:
				return vector4Values ?? (vector4Values = new Vector4[0]);
			case VariableType.Unknown:
				return null;
			case VariableType.Array:
				return null;
			default:
				Debug.LogError(type);
				throw new ArgumentOutOfRangeException();
			}
		}

		public void Resize(int newLength)
		{
			if (newLength != Values.Length)
			{
				if (newLength < 0)
				{
					newLength = 0;
				}
				Type elementType = Values.GetType().GetElementType();
				Array array = Array.CreateInstance(elementType, newLength);
				Array.Copy(values, array, Math.Min(values.Length, newLength));
				Values = (object[])array;
				SaveChanges();
			}
		}

		public void Reset()
		{
			floatValues = null;
			intValues = null;
			boolValues = null;
			stringValues = null;
			vector4Values = null;
			objectReferences = null;
			objectType = null;
			objectTypeName = null;
			InitArray();
		}

		public FsmArray()
		{
		}

		public FsmArray(string name)
			: base(name)
		{
		}

		public FsmArray(FsmArray source)
			: base(source)
		{
			if (source != null)
			{
				type = source.type;
				ObjectType = source.ObjectType;
				CopyValues(source);
				SaveChanges();
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmArray(this);
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < Values.Length; i++)
			{
				object obj = Values[i];
				if (obj == null)
				{
					text += "null";
				}
				else
				{
					UnityEngine.Object @object = obj as UnityEngine.Object;
					text = ((!(@object != null)) ? (text + obj.ToString()) : (text + @object.name));
				}
				if (i < Values.Length - 1)
				{
					text += ", ";
				}
			}
			if (text == string.Empty)
			{
				text = "Empty";
			}
			return text;
		}

		public override bool TestTypeConstraint(VariableType variableType, Type _objectType = null)
		{
			if (variableType == VariableType.Unknown)
			{
				return true;
			}
			if (base.TestTypeConstraint(variableType, objectType))
			{
				if (!object.ReferenceEquals(ObjectType, _objectType))
				{
					return object.ReferenceEquals(_objectType, null);
				}
				return true;
			}
			return false;
		}

		public Type RealType()
		{
			switch (type)
			{
			case VariableType.Float:
				return typeof(float[]);
			case VariableType.Int:
				return typeof(int[]);
			case VariableType.Bool:
				return typeof(bool[]);
			case VariableType.GameObject:
				return typeof(GameObject[]);
			case VariableType.String:
				return typeof(string[]);
			case VariableType.Vector2:
				return typeof(Vector2[]);
			case VariableType.Vector3:
				return typeof(Vector3[]);
			case VariableType.Color:
				return typeof(Color[]);
			case VariableType.Rect:
				return typeof(Rect[]);
			case VariableType.Material:
				return typeof(Material[]);
			case VariableType.Texture:
				return typeof(Texture[]);
			case VariableType.Quaternion:
				return typeof(Quaternion[]);
			case VariableType.Object:
				return ObjectType.MakeArrayType();
			case VariableType.Enum:
				return ObjectType.MakeArrayType();
			case VariableType.Unknown:
				return null;
			case VariableType.Array:
				Debug.LogError("Nested arrays are not supported yet!");
				return null;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
