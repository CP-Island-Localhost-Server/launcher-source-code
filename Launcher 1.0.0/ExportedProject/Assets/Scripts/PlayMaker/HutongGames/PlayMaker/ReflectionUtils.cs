using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace HutongGames.PlayMaker
{
	public static class ReflectionUtils
	{
		private static List<string> assemblyNames;

		private static Assembly[] loadedAssemblies;

		private static readonly Dictionary<string, Type> typeLookup = new Dictionary<string, Type>();

		public static Assembly[] GetLoadedAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		[Localizable(false)]
		public static Type GetGlobalType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				return null;
			}
			Type value;
			typeLookup.TryGetValue(typeName, out value);
			if (!object.ReferenceEquals(value, null))
			{
				return value;
			}
			value = Type.GetType(typeName + ",Assembly-CSharp") ?? Type.GetType(typeName + ",PlayMaker");
			if (object.ReferenceEquals(value, null))
			{
				value = Type.GetType(typeName + ",Assembly-CSharp-firstpass") ?? Type.GetType(typeName);
			}
			if (object.ReferenceEquals(value, null))
			{
				if (assemblyNames == null)
				{
					assemblyNames = new List<string>();
					loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
					Assembly[] array = loadedAssemblies;
					foreach (Assembly assembly in array)
					{
						assemblyNames.Add(assembly.FullName);
					}
				}
				foreach (string assemblyName in assemblyNames)
				{
					value = Type.GetType(typeName + "," + assemblyName);
					if (!object.ReferenceEquals(value, null))
					{
						break;
					}
				}
				if (object.ReferenceEquals(value, null))
				{
					for (int j = 0; j < loadedAssemblies.Length; j++)
					{
						Type[] types = loadedAssemblies[j].GetTypes();
						for (int k = 0; k < types.Length; k++)
						{
							if (types[k].Name == typeName && (types[k].Namespace == "UnityEngine" || types[k].Namespace == "HutongGames.PlayMaker" || types[k].Namespace == "HutongGames.PlayMaker.Actions"))
							{
								value = types[k];
								typeLookup[typeName] = value;
								return value;
							}
						}
					}
				}
			}
			typeLookup.Remove(typeName);
			typeLookup[typeName] = value;
			return value;
		}

		public static Type GetPropertyType(Type type, string path)
		{
			string[] array = path.Split('.');
			string[] array2 = array;
			foreach (string name in array2)
			{
				PropertyInfo property = type.GetProperty(name);
				if (!object.ReferenceEquals(property, null))
				{
					type = property.PropertyType;
					continue;
				}
				FieldInfo field = type.GetField(name);
				if (!object.ReferenceEquals(field, null))
				{
					type = field.FieldType;
					continue;
				}
				return null;
			}
			return type;
		}

		public static MemberInfo[] GetMemberInfo(Type type, string path)
		{
			if (object.ReferenceEquals(type, null))
			{
				return null;
			}
			string[] array = path.Split('.');
			MemberInfo[] array2 = new MemberInfo[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string name = array[i];
				PropertyInfo property = type.GetProperty(name);
				if (!object.ReferenceEquals(property, null))
				{
					array2[i] = property;
					type = property.PropertyType;
					continue;
				}
				FieldInfo field = type.GetField(name);
				if (!object.ReferenceEquals(field, null))
				{
					array2[i] = field;
					type = field.FieldType;
					continue;
				}
				return null;
			}
			return array2;
		}

		public static bool CanReadMemberValue(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return true;
			case MemberTypes.Property:
				return ((PropertyInfo)member).CanRead;
			default:
				return false;
			}
		}

		public static bool CanSetMemberValue(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return true;
			case MemberTypes.Property:
				return ((PropertyInfo)member).CanWrite;
			default:
				return false;
			}
		}

		public static bool CanGetMemberValue(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return true;
			case MemberTypes.Property:
				return ((PropertyInfo)member).CanRead;
			default:
				return false;
			}
		}

		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).FieldType;
			case MemberTypes.Property:
				return ((PropertyInfo)member).PropertyType;
			case MemberTypes.Event:
				return ((EventInfo)member).EventHandlerType;
			default:
				throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo or EventInfo", "member");
			}
		}

		public static object GetMemberValue(MemberInfo[] memberInfo, object target)
		{
			for (int i = 0; i < memberInfo.Length; i++)
			{
				target = GetMemberValue(memberInfo[i], target);
			}
			return target;
		}

		public static object GetMemberValue(MemberInfo member, object target)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).GetValue(target);
			case MemberTypes.Property:
				try
				{
					return ((PropertyInfo)member).GetValue(target, null);
				}
				catch (TargetParameterCountException innerException)
				{
					throw new ArgumentException("MemberInfo has index parameters", "member", innerException);
				}
			default:
				throw new ArgumentException("MemberInfo is not of type FieldInfo or PropertyInfo", "member");
			}
		}

		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			switch (member.MemberType)
			{
			case MemberTypes.Field:
				((FieldInfo)member).SetValue(target, value);
				break;
			case MemberTypes.Property:
				((PropertyInfo)member).SetValue(target, value, null);
				break;
			default:
				throw new ArgumentException("MemberInfo must be if type FieldInfo or PropertyInfo", "member");
			}
		}

		public static void SetMemberValue(MemberInfo[] memberInfo, object target, object value)
		{
			object parent = null;
			MemberInfo targetInfo = null;
			for (int i = 0; i < memberInfo.Length - 1; i++)
			{
				parent = target;
				targetInfo = memberInfo[i];
				target = GetMemberValue(memberInfo[i], target);
			}
			if (target.GetType().IsValueType)
			{
				SetBoxedMemberValue(parent, targetInfo, target, memberInfo[memberInfo.Length - 1], value);
			}
			else
			{
				SetMemberValue(memberInfo[memberInfo.Length - 1], target, value);
			}
		}

		public static void SetBoxedMemberValue(object parent, MemberInfo targetInfo, object target, MemberInfo propertyInfo, object value)
		{
			SetMemberValue(propertyInfo, target, value);
			SetMemberValue(targetInfo, parent, target);
		}

		public static List<MemberInfo> GetFieldsAndProperties<T>(BindingFlags bindingAttr)
		{
			return GetFieldsAndProperties(typeof(T), bindingAttr);
		}

		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(type.GetFields(bindingAttr));
			list.AddRange(type.GetProperties(bindingAttr));
			return list;
		}

		public static FieldInfo[] GetPublicFields(this Type type)
		{
			return type.GetFields(BindingFlags.Instance | BindingFlags.Public);
		}

		public static PropertyInfo[] GetPublicProperties(this Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}
	}
}
