using System;
using System.Reflection;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmProperty
	{
		public FsmObject TargetObject = new FsmObject();

		public string TargetTypeName = "";

		public Type TargetType;

		public string PropertyName = "";

		public Type PropertyType;

		public FsmBool BoolParameter;

		public FsmFloat FloatParameter;

		public FsmInt IntParameter;

		public FsmGameObject GameObjectParameter;

		public FsmString StringParameter;

		public FsmVector2 Vector2Parameter;

		public FsmVector3 Vector3Parameter;

		public FsmRect RectParamater;

		public FsmQuaternion QuaternionParameter;

		public FsmObject ObjectParameter;

		public FsmMaterial MaterialParameter;

		public FsmTexture TextureParameter;

		public FsmColor ColorParameter;

		public FsmEnum EnumParameter;

		public FsmArray ArrayParameter;

		public bool setProperty;

		private bool initialized;

		[NonSerialized]
		private UnityEngine.Object targetObjectCached;

		private MemberInfo[] memberInfo;

		public FsmProperty()
		{
			ResetParameters();
		}

		public FsmProperty(FsmProperty source)
		{
			setProperty = source.setProperty;
			TargetObject = new FsmObject(source.TargetObject);
			TargetTypeName = source.TargetTypeName;
			TargetType = source.TargetType;
			PropertyName = source.PropertyName;
			PropertyType = source.PropertyType;
			BoolParameter = new FsmBool(source.BoolParameter);
			FloatParameter = new FsmFloat(source.FloatParameter);
			IntParameter = new FsmInt(source.IntParameter);
			GameObjectParameter = new FsmGameObject(source.GameObjectParameter);
			StringParameter = new FsmString(source.StringParameter);
			Vector2Parameter = new FsmVector2(source.Vector2Parameter);
			Vector3Parameter = new FsmVector3(source.Vector3Parameter);
			RectParamater = new FsmRect(source.RectParamater);
			QuaternionParameter = new FsmQuaternion(source.QuaternionParameter);
			ObjectParameter = new FsmObject(source.ObjectParameter);
			MaterialParameter = new FsmMaterial(source.MaterialParameter);
			TextureParameter = new FsmTexture(source.TextureParameter);
			ColorParameter = new FsmColor(source.ColorParameter);
			EnumParameter = new FsmEnum(source.EnumParameter);
			ArrayParameter = new FsmArray(source.ArrayParameter);
		}

		public void SetVariable(NamedVariable variable)
		{
			if (variable == null)
			{
				ResetParameters();
				return;
			}
			switch (variable.VariableType)
			{
			case VariableType.Float:
				FloatParameter = variable as FsmFloat;
				break;
			case VariableType.Int:
				IntParameter = variable as FsmInt;
				break;
			case VariableType.Bool:
				BoolParameter = variable as FsmBool;
				break;
			case VariableType.GameObject:
				GameObjectParameter = variable as FsmGameObject;
				break;
			case VariableType.String:
				StringParameter = variable as FsmString;
				break;
			case VariableType.Vector2:
				Vector2Parameter = variable as FsmVector2;
				break;
			case VariableType.Vector3:
				Vector3Parameter = variable as FsmVector3;
				break;
			case VariableType.Color:
				ColorParameter = variable as FsmColor;
				break;
			case VariableType.Rect:
				RectParamater = variable as FsmRect;
				break;
			case VariableType.Material:
				MaterialParameter = variable as FsmMaterial;
				break;
			case VariableType.Texture:
				TextureParameter = variable as FsmTexture;
				break;
			case VariableType.Quaternion:
				QuaternionParameter = variable as FsmQuaternion;
				break;
			case VariableType.Object:
				ObjectParameter = variable as FsmObject;
				break;
			case VariableType.Array:
				ArrayParameter = variable as FsmArray;
				break;
			case VariableType.Enum:
				EnumParameter = variable as FsmEnum;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case VariableType.Unknown:
				break;
			}
		}

		public NamedVariable GetVariable()
		{
			CheckForReinitialize();
			if (PropertyType.IsAssignableFrom(typeof(bool)))
			{
				return BoolParameter;
			}
			if (PropertyType.IsAssignableFrom(typeof(int)))
			{
				return IntParameter;
			}
			if (PropertyType.IsAssignableFrom(typeof(float)))
			{
				return FloatParameter;
			}
			if (PropertyType.IsAssignableFrom(typeof(string)))
			{
				return StringParameter;
			}
			if (PropertyType.IsAssignableFrom(typeof(Vector2)))
			{
				return Vector2Parameter;
			}
			if (PropertyType.IsAssignableFrom(typeof(Vector3)))
			{
				return Vector3Parameter;
			}
			if (PropertyType.IsAssignableFrom(typeof(Rect)))
			{
				return RectParamater;
			}
			if (PropertyType.IsAssignableFrom(typeof(Quaternion)))
			{
				return QuaternionParameter;
			}
			if (object.ReferenceEquals(PropertyType, typeof(GameObject)))
			{
				return GameObjectParameter;
			}
			if (object.ReferenceEquals(PropertyType, typeof(Material)))
			{
				return MaterialParameter;
			}
			if (object.ReferenceEquals(PropertyType, typeof(Texture)))
			{
				return TextureParameter;
			}
			if (object.ReferenceEquals(PropertyType, typeof(Color)))
			{
				return ColorParameter;
			}
			if (PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				return ObjectParameter;
			}
			if (PropertyType.IsArray)
			{
				return ArrayParameter;
			}
			if (PropertyType.IsEnum)
			{
				return EnumParameter;
			}
			return null;
		}

		public void SetPropertyName(string propertyName)
		{
			ResetParameters();
			PropertyName = propertyName;
			if (!string.IsNullOrEmpty(PropertyName))
			{
				if (!object.ReferenceEquals(TargetType, null))
				{
					PropertyType = ReflectionUtils.GetPropertyType(TargetType, PropertyName);
					if (TargetType.IsSubclassOf(typeof(UnityEngine.Object)) && !object.ReferenceEquals(PropertyType, null))
					{
						ObjectParameter.ObjectType = PropertyType;
					}
					else if (PropertyType.IsArray)
					{
						ArrayParameter.ElementType = FsmVar.GetVariableType(PropertyType.GetElementType());
					}
				}
			}
			else
			{
				PropertyType = null;
			}
			Init();
		}

		public void SetValue()
		{
			CheckForReinitialize();
			if (targetObjectCached == null || memberInfo == null)
			{
				return;
			}
			if (PropertyType.IsAssignableFrom(typeof(bool)) && !BoolParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, BoolParameter.Value);
			}
			else if (PropertyType.IsAssignableFrom(typeof(int)) && !IntParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, IntParameter.Value);
			}
			else if (PropertyType.IsAssignableFrom(typeof(float)) && !FloatParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, FloatParameter.Value);
			}
			else if (PropertyType.IsAssignableFrom(typeof(string)) && !StringParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, StringParameter.Value);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Vector2)) && !Vector2Parameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, Vector2Parameter.Value);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Vector3)) && !Vector3Parameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, Vector3Parameter.Value);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Rect)) && !RectParamater.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, RectParamater.Value);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Quaternion)) && !QuaternionParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, QuaternionParameter.Value);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(GameObject)) && !GameObjectParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, GameObjectParameter.Value);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(Material)) && !MaterialParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, MaterialParameter.Value);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(Texture)) && !TextureParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, TextureParameter.Value);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(Color)) && !ColorParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, ColorParameter.Value);
			}
			else if (PropertyType.IsSubclassOf(typeof(UnityEngine.Object)) && !ObjectParameter.IsNone)
			{
				if (ObjectParameter.Value == null)
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, null);
				}
				else
				{
					ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, ObjectParameter.Value);
				}
			}
			else if (PropertyType.IsArray && !ArrayParameter.IsNone)
			{
				object[] values = ArrayParameter.Values;
				Array array = Array.CreateInstance(PropertyType.GetElementType(), values.Length);
				for (int i = 0; i < values.Length; i++)
				{
					array.SetValue(values[i], i);
				}
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, array);
			}
			else if (PropertyType.IsEnum && !EnumParameter.IsNone)
			{
				ReflectionUtils.SetMemberValue(memberInfo, targetObjectCached, EnumParameter.Value);
			}
		}

		public void GetValue()
		{
			CheckForReinitialize();
			if (targetObjectCached == null || memberInfo == null)
			{
				return;
			}
			if (PropertyType.IsAssignableFrom(typeof(bool)))
			{
				BoolParameter.Value = (bool)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsAssignableFrom(typeof(int)))
			{
				IntParameter.Value = (int)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsAssignableFrom(typeof(float)))
			{
				FloatParameter.Value = (float)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsAssignableFrom(typeof(string)))
			{
				StringParameter.Value = (string)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Vector2)))
			{
				Vector2Parameter.Value = (Vector2)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Vector3)))
			{
				Vector3Parameter.Value = (Vector3)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Rect)))
			{
				RectParamater.Value = (Rect)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsAssignableFrom(typeof(Quaternion)))
			{
				QuaternionParameter.Value = (Quaternion)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(GameObject)))
			{
				GameObjectParameter.Value = (GameObject)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(Material)))
			{
				MaterialParameter.Value = (Material)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(Texture)))
			{
				TextureParameter.Value = (Texture)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (object.ReferenceEquals(PropertyType, typeof(Color)))
			{
				ColorParameter.Value = (Color)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsEnum)
			{
				EnumParameter.Value = (Enum)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
			else if (PropertyType.IsArray)
			{
				Array array = (Array)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
				object[] array2 = new object[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = array.GetValue(i);
				}
				ArrayParameter.Values = array2;
			}
			else if (PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				ObjectParameter.Value = (UnityEngine.Object)ReflectionUtils.GetMemberValue(memberInfo, targetObjectCached);
			}
		}

		public void Init()
		{
			if (TargetObject == null)
			{
				return;
			}
			initialized = true;
			targetObjectCached = TargetObject.Value;
			if (TargetObject.UseVariable)
			{
				TargetTypeName = TargetObject.TypeName;
				TargetType = TargetObject.ObjectType;
			}
			else if (TargetObject.Value != null)
			{
				TargetType = TargetObject.Value.GetType();
				TargetTypeName = TargetType.FullName;
			}
			if (!string.IsNullOrEmpty(PropertyName))
			{
				memberInfo = ReflectionUtils.GetMemberInfo(TargetType, PropertyName);
				if (object.ReferenceEquals(memberInfo, null))
				{
					PropertyName = "";
					PropertyType = null;
					ResetParameters();
					return;
				}
				PropertyType = ReflectionUtils.GetMemberUnderlyingType(memberInfo[memberInfo.Length - 1]);
			}
			if (!object.ReferenceEquals(PropertyType, null) && PropertyType.IsEnum && !FsmString.IsNullOrEmpty(StringParameter))
			{
				EnumParameter = new FsmEnum("")
				{
					EnumType = PropertyType,
					Value = (Enum)Enum.Parse(PropertyType, StringParameter.Value)
				};
				StringParameter.Value = null;
			}
		}

		public void CheckForReinitialize()
		{
			if (!initialized || targetObjectCached != TargetObject.Value || (TargetObject.UseVariable && !object.ReferenceEquals(TargetType, TargetObject.ObjectType)))
			{
				Init();
			}
		}

		public void ResetParameters()
		{
			BoolParameter = false;
			FloatParameter = 0f;
			IntParameter = 0;
			StringParameter = "";
			GameObjectParameter = new FsmGameObject("");
			Vector2Parameter = new FsmVector2();
			Vector3Parameter = new FsmVector3();
			RectParamater = new FsmRect();
			QuaternionParameter = new FsmQuaternion();
			ObjectParameter = new FsmObject();
			MaterialParameter = new FsmMaterial();
			TextureParameter = new FsmTexture();
			ColorParameter = new FsmColor();
			EnumParameter = new FsmEnum();
			ArrayParameter = new FsmArray();
		}
	}
}
