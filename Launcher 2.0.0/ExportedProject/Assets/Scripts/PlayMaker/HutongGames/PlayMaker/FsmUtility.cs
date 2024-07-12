using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public static class FsmUtility
	{
		public static class BitConverter
		{
			public static int ToInt32(byte[] value, int startIndex)
			{
				if (System.BitConverter.IsLittleEndian)
				{
					return System.BitConverter.ToInt32(value, startIndex);
				}
				Array.Reverse(value, startIndex, 4);
				return System.BitConverter.ToInt32(value, startIndex);
			}

			public static float ToSingle(byte[] value, int startIndex)
			{
				if (System.BitConverter.IsLittleEndian)
				{
					return System.BitConverter.ToSingle(value, startIndex);
				}
				Array.Reverse(value, startIndex, 4);
				return System.BitConverter.ToSingle(value, startIndex);
			}

			public static bool ToBoolean(byte[] value, int startIndex)
			{
				return System.BitConverter.ToBoolean(value, startIndex);
			}

			public static byte[] GetBytes(bool value)
			{
				if (System.BitConverter.IsLittleEndian)
				{
					return System.BitConverter.GetBytes(value);
				}
				byte[] bytes = System.BitConverter.GetBytes(value);
				Array.Reverse(bytes);
				return bytes;
			}

			public static byte[] GetBytes(int value)
			{
				if (System.BitConverter.IsLittleEndian)
				{
					return System.BitConverter.GetBytes(value);
				}
				byte[] bytes = System.BitConverter.GetBytes(value);
				Array.Reverse(bytes);
				return bytes;
			}

			public static byte[] GetBytes(float value)
			{
				if (System.BitConverter.IsLittleEndian)
				{
					return System.BitConverter.GetBytes(value);
				}
				byte[] bytes = System.BitConverter.GetBytes(value);
				Array.Reverse(bytes);
				return bytes;
			}
		}

		private static UTF8Encoding encoding;

		public static UTF8Encoding Encoding
		{
			get
			{
				return encoding ?? (encoding = new UTF8Encoding());
			}
		}

		[Obsolete("Use VariableType property in NamedVariable")]
		public static VariableType GetVariableType(INamedVariable variable)
		{
			if (variable == null)
			{
				return VariableType.Unknown;
			}
			Type type = variable.GetType();
			if (object.ReferenceEquals(type, typeof(FsmMaterial)))
			{
				return VariableType.Material;
			}
			if (object.ReferenceEquals(type, typeof(FsmTexture)))
			{
				return VariableType.Texture;
			}
			if (object.ReferenceEquals(type, typeof(FsmFloat)))
			{
				return VariableType.Float;
			}
			if (object.ReferenceEquals(type, typeof(FsmInt)))
			{
				return VariableType.Int;
			}
			if (object.ReferenceEquals(type, typeof(FsmBool)))
			{
				return VariableType.Bool;
			}
			if (object.ReferenceEquals(type, typeof(FsmString)))
			{
				return VariableType.String;
			}
			if (object.ReferenceEquals(type, typeof(FsmGameObject)))
			{
				return VariableType.GameObject;
			}
			if (object.ReferenceEquals(type, typeof(FsmVector2)))
			{
				return VariableType.Vector2;
			}
			if (object.ReferenceEquals(type, typeof(FsmVector3)))
			{
				return VariableType.Vector3;
			}
			if (object.ReferenceEquals(type, typeof(FsmRect)))
			{
				return VariableType.Rect;
			}
			if (object.ReferenceEquals(type, typeof(FsmQuaternion)))
			{
				return VariableType.Quaternion;
			}
			if (object.ReferenceEquals(type, typeof(FsmColor)))
			{
				return VariableType.Color;
			}
			if (object.ReferenceEquals(type, typeof(FsmObject)))
			{
				return VariableType.Object;
			}
			if (object.ReferenceEquals(type, typeof(FsmEnum)))
			{
				return VariableType.Enum;
			}
			if (object.ReferenceEquals(type, typeof(FsmArray)))
			{
				return VariableType.Array;
			}
			return VariableType.Unknown;
		}

		public static Type GetVariableRealType(VariableType variableType)
		{
			switch (variableType)
			{
			case VariableType.Float:
				return typeof(float);
			case VariableType.Int:
				return typeof(int);
			case VariableType.Bool:
				return typeof(bool);
			case VariableType.GameObject:
				return typeof(GameObject);
			case VariableType.String:
				return typeof(string);
			case VariableType.Vector2:
				return typeof(Vector2);
			case VariableType.Vector3:
				return typeof(Vector3);
			case VariableType.Color:
				return typeof(Color);
			case VariableType.Rect:
				return typeof(Rect);
			case VariableType.Material:
				return typeof(Material);
			case VariableType.Texture:
				return typeof(Texture);
			case VariableType.Quaternion:
				return typeof(Quaternion);
			case VariableType.Object:
				return typeof(UnityEngine.Object);
			case VariableType.Unknown:
				return null;
			case VariableType.Array:
				return typeof(Array);
			case VariableType.Enum:
				return typeof(Enum);
			default:
				throw new ArgumentOutOfRangeException("variableType");
			}
		}

		public static object GetEnum(Type enumType, int enumValue)
		{
			return Enum.ToObject(enumType, (object)enumValue);
		}

		public static ICollection<byte> FsmEventToByteArray(FsmEvent fsmEvent)
		{
			if (fsmEvent == null)
			{
				return null;
			}
			List<byte> list = new List<byte>();
			list.AddRange(StringToByteArray(fsmEvent.Name));
			return list;
		}

		public static ICollection<byte> FsmFloatToByteArray(FsmFloat fsmFloat)
		{
			if (fsmFloat == null)
			{
				fsmFloat = new FsmFloat();
			}
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(fsmFloat.Value));
			list.AddRange(BitConverter.GetBytes(fsmFloat.UseVariable));
			list.AddRange(StringToByteArray(fsmFloat.Name));
			return list;
		}

		public static ICollection<byte> FsmIntToByteArray(FsmInt fsmInt)
		{
			if (fsmInt == null)
			{
				fsmInt = new FsmInt();
			}
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(fsmInt.Value));
			list.AddRange(BitConverter.GetBytes(fsmInt.UseVariable));
			list.AddRange(StringToByteArray(fsmInt.Name));
			return list;
		}

		public static ICollection<byte> FsmBoolToByteArray(FsmBool fsmBool)
		{
			if (fsmBool == null)
			{
				fsmBool = new FsmBool();
			}
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(fsmBool.Value));
			list.AddRange(BitConverter.GetBytes(fsmBool.UseVariable));
			list.AddRange(StringToByteArray(fsmBool.Name));
			return list;
		}

		public static ICollection<byte> FsmVector2ToByteArray(FsmVector2 fsmVector2)
		{
			if (fsmVector2 == null)
			{
				fsmVector2 = new FsmVector2();
			}
			List<byte> list = new List<byte>();
			list.AddRange(Vector2ToByteArray(fsmVector2.Value));
			list.AddRange(BitConverter.GetBytes(fsmVector2.UseVariable));
			list.AddRange(StringToByteArray(fsmVector2.Name));
			return list;
		}

		public static ICollection<byte> FsmVector3ToByteArray(FsmVector3 fsmVector3)
		{
			if (fsmVector3 == null)
			{
				fsmVector3 = new FsmVector3();
			}
			List<byte> list = new List<byte>();
			list.AddRange(Vector3ToByteArray(fsmVector3.Value));
			list.AddRange(BitConverter.GetBytes(fsmVector3.UseVariable));
			list.AddRange(StringToByteArray(fsmVector3.Name));
			return list;
		}

		public static ICollection<byte> FsmRectToByteArray(FsmRect fsmRect)
		{
			if (fsmRect == null)
			{
				fsmRect = new FsmRect();
			}
			List<byte> list = new List<byte>();
			list.AddRange(RectToByteArray(fsmRect.Value));
			list.AddRange(BitConverter.GetBytes(fsmRect.UseVariable));
			list.AddRange(StringToByteArray(fsmRect.Name));
			return list;
		}

		public static ICollection<byte> FsmQuaternionToByteArray(FsmQuaternion fsmQuaternion)
		{
			if (fsmQuaternion == null)
			{
				fsmQuaternion = new FsmQuaternion();
			}
			List<byte> list = new List<byte>();
			list.AddRange(QuaternionToByteArray(fsmQuaternion.Value));
			list.AddRange(BitConverter.GetBytes(fsmQuaternion.UseVariable));
			list.AddRange(StringToByteArray(fsmQuaternion.Name));
			return list;
		}

		public static ICollection<byte> FsmColorToByteArray(FsmColor fsmColor)
		{
			if (fsmColor == null)
			{
				fsmColor = new FsmColor();
			}
			List<byte> list = new List<byte>();
			list.AddRange(ColorToByteArray(fsmColor.Value));
			list.AddRange(BitConverter.GetBytes(fsmColor.UseVariable));
			list.AddRange(StringToByteArray(fsmColor.Name));
			return list;
		}

		public static ICollection<byte> ColorToByteArray(Color color)
		{
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(color.r));
			list.AddRange(BitConverter.GetBytes(color.g));
			list.AddRange(BitConverter.GetBytes(color.b));
			list.AddRange(BitConverter.GetBytes(color.a));
			return list;
		}

		public static ICollection<byte> Vector2ToByteArray(Vector2 vector2)
		{
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(vector2[0]));
			list.AddRange(BitConverter.GetBytes(vector2[1]));
			return list;
		}

		public static ICollection<byte> Vector3ToByteArray(Vector3 vector3)
		{
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(vector3[0]));
			list.AddRange(BitConverter.GetBytes(vector3[1]));
			list.AddRange(BitConverter.GetBytes(vector3[2]));
			return list;
		}

		public static ICollection<byte> Vector4ToByteArray(Vector4 vector4)
		{
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(vector4[0]));
			list.AddRange(BitConverter.GetBytes(vector4[1]));
			list.AddRange(BitConverter.GetBytes(vector4[2]));
			list.AddRange(BitConverter.GetBytes(vector4[3]));
			return list;
		}

		public static ICollection<byte> RectToByteArray(Rect rect)
		{
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(rect.x));
			list.AddRange(BitConverter.GetBytes(rect.y));
			list.AddRange(BitConverter.GetBytes(rect.width));
			list.AddRange(BitConverter.GetBytes(rect.height));
			return list;
		}

		public static ICollection<byte> QuaternionToByteArray(Quaternion quaternion)
		{
			List<byte> list = new List<byte>();
			list.AddRange(BitConverter.GetBytes(quaternion.x));
			list.AddRange(BitConverter.GetBytes(quaternion.y));
			list.AddRange(BitConverter.GetBytes(quaternion.z));
			list.AddRange(BitConverter.GetBytes(quaternion.w));
			return list;
		}

		public static byte[] StringToByteArray(string str)
		{
			if (str == null)
			{
				str = "";
			}
			return Encoding.GetBytes(str);
		}

		public static string ByteArrayToString(byte[] bytes)
		{
			if (bytes.Length == 0)
			{
				return "";
			}
			return Encoding.GetString(bytes);
		}

		public static string ByteArrayToString(byte[] bytes, int startIndex, int count)
		{
			if (count == 0)
			{
				return string.Empty;
			}
			return Encoding.GetString(bytes, startIndex, count);
		}

		public static FsmEvent ByteArrayToFsmEvent(byte[] bytes, int startIndex, int size)
		{
			string text = ByteArrayToString(bytes, startIndex, size);
			if (!string.IsNullOrEmpty(text))
			{
				return FsmEvent.GetFsmEvent(text);
			}
			return null;
		}

		public static FsmFloat ByteArrayToFsmFloat(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 5, totalLength - 5);
			if (@string != string.Empty)
			{
				return fsm.GetFsmFloat(@string);
			}
			FsmFloat fsmFloat = new FsmFloat();
			fsmFloat.Value = BitConverter.ToSingle(bytes, startIndex);
			fsmFloat.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 4);
			return fsmFloat;
		}

		public static FsmInt ByteArrayToFsmInt(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 5, totalLength - 5);
			if (@string != string.Empty)
			{
				return fsm.GetFsmInt(@string);
			}
			FsmInt fsmInt = new FsmInt();
			fsmInt.Value = BitConverter.ToInt32(bytes, startIndex);
			fsmInt.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 4);
			return fsmInt;
		}

		public static FsmBool ByteArrayToFsmBool(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 2, totalLength - 2);
			if (@string != string.Empty)
			{
				return fsm.GetFsmBool(@string);
			}
			FsmBool fsmBool = new FsmBool();
			fsmBool.Value = BitConverter.ToBoolean(bytes, startIndex);
			fsmBool.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 1);
			return fsmBool;
		}

		public static Color ByteArrayToColor(byte[] bytes, int startIndex)
		{
			float r = BitConverter.ToSingle(bytes, startIndex);
			float g = BitConverter.ToSingle(bytes, startIndex + 4);
			float b = BitConverter.ToSingle(bytes, startIndex + 8);
			float a = BitConverter.ToSingle(bytes, startIndex + 12);
			return new Color(r, g, b, a);
		}

		public static Vector2 ByteArrayToVector2(byte[] bytes, int startIndex)
		{
			float x = BitConverter.ToSingle(bytes, startIndex);
			float y = BitConverter.ToSingle(bytes, startIndex + 4);
			return new Vector2(x, y);
		}

		public static FsmVector2 ByteArrayToFsmVector2(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 9, totalLength - 9);
			if (@string != string.Empty)
			{
				return fsm.GetFsmVector2(@string);
			}
			FsmVector2 fsmVector = new FsmVector2();
			fsmVector.Value = ByteArrayToVector2(bytes, startIndex);
			fsmVector.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 8);
			return fsmVector;
		}

		public static Vector3 ByteArrayToVector3(byte[] bytes, int startIndex)
		{
			float x = BitConverter.ToSingle(bytes, startIndex);
			float y = BitConverter.ToSingle(bytes, startIndex + 4);
			float z = BitConverter.ToSingle(bytes, startIndex + 8);
			return new Vector3(x, y, z);
		}

		public static FsmVector3 ByteArrayToFsmVector3(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 13, totalLength - 13);
			if (@string != string.Empty)
			{
				return fsm.GetFsmVector3(@string);
			}
			FsmVector3 fsmVector = new FsmVector3();
			fsmVector.Value = ByteArrayToVector3(bytes, startIndex);
			fsmVector.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 12);
			return fsmVector;
		}

		public static FsmRect ByteArrayToFsmRect(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 17, totalLength - 17);
			if (@string != string.Empty)
			{
				return fsm.GetFsmRect(@string);
			}
			FsmRect fsmRect = new FsmRect();
			fsmRect.Value = ByteArrayToRect(bytes, startIndex);
			fsmRect.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 16);
			return fsmRect;
		}

		public static FsmQuaternion ByteArrayToFsmQuaternion(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 17, totalLength - 17);
			if (@string != string.Empty)
			{
				return fsm.GetFsmQuaternion(@string);
			}
			FsmQuaternion fsmQuaternion = new FsmQuaternion();
			fsmQuaternion.Value = ByteArrayToQuaternion(bytes, startIndex);
			fsmQuaternion.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 16);
			return fsmQuaternion;
		}

		public static FsmColor ByteArrayToFsmColor(Fsm fsm, byte[] bytes, int startIndex, int totalLength)
		{
			string @string = Encoding.GetString(bytes, startIndex + 17, totalLength - 17);
			if (@string != string.Empty)
			{
				return fsm.GetFsmColor(@string);
			}
			FsmColor fsmColor = new FsmColor();
			fsmColor.Value = ByteArrayToColor(bytes, startIndex);
			fsmColor.UseVariable = BitConverter.ToBoolean(bytes, startIndex + 16);
			return fsmColor;
		}

		public static Vector4 ByteArrayToVector4(byte[] bytes, int startIndex)
		{
			float x = BitConverter.ToSingle(bytes, startIndex);
			float y = BitConverter.ToSingle(bytes, startIndex + 4);
			float z = BitConverter.ToSingle(bytes, startIndex + 8);
			float w = BitConverter.ToSingle(bytes, startIndex + 12);
			return new Vector4(x, y, z, w);
		}

		public static Rect ByteArrayToRect(byte[] bytes, int startIndex)
		{
			float x = BitConverter.ToSingle(bytes, startIndex);
			float y = BitConverter.ToSingle(bytes, startIndex + 4);
			float width = BitConverter.ToSingle(bytes, startIndex + 8);
			float height = BitConverter.ToSingle(bytes, startIndex + 12);
			return new Rect(x, y, width, height);
		}

		public static Quaternion ByteArrayToQuaternion(byte[] bytes, int startIndex)
		{
			float x = BitConverter.ToSingle(bytes, startIndex);
			float y = BitConverter.ToSingle(bytes, startIndex + 4);
			float z = BitConverter.ToSingle(bytes, startIndex + 8);
			float w = BitConverter.ToSingle(bytes, startIndex + 12);
			return new Quaternion(x, y, z, w);
		}

		private static byte[] ReadToEnd(Stream stream)
		{
			long position = stream.Position;
			stream.Position = 0L;
			try
			{
				byte[] array = new byte[4096];
				int num = 0;
				int num2;
				while ((num2 = stream.Read(array, num, array.Length - num)) > 0)
				{
					num += num2;
					if (num == array.Length)
					{
						int num3 = stream.ReadByte();
						if (num3 != -1)
						{
							byte[] array2 = new byte[array.Length * 2];
							Buffer.BlockCopy(array, 0, array2, 0, array.Length);
							Buffer.SetByte(array2, num, (byte)num3);
							array = array2;
							num++;
						}
					}
				}
				byte[] array3 = array;
				if (array.Length != num)
				{
					array3 = new byte[num];
					Buffer.BlockCopy(array, 0, array3, 0, num);
				}
				return array3;
			}
			finally
			{
				stream.Position = position;
			}
		}

		public static string StripNamespace(string name)
		{
			if (name == null)
			{
				return "[missing name]";
			}
			return name.Substring(name.LastIndexOf(".", StringComparison.Ordinal) + 1);
		}

		public static string GetPath(FsmState state)
		{
			if (state == null)
			{
				return "[missing state]";
			}
			return ((state.Fsm != null) ? (state.Fsm.OwnerDebugName + ": " + state.Fsm.Name) : "[missing FSM]") + ": " + state.Name + ": ";
		}

		public static string GetPath(FsmState state, FsmStateAction action)
		{
			if (action == null)
			{
				return GetPath(state) + "[missing action] ";
			}
			return GetPath(state) + action.GetType().Name + ": ";
		}

		public static string GetPath(FsmState state, FsmStateAction action, string parameter)
		{
			return GetPath(state, action) + parameter + ": ";
		}

		public static string GetFullFsmLabel(Fsm fsm)
		{
			if (fsm == null)
			{
				return "None (FSM)";
			}
			if (fsm.UsedInTemplate != null)
			{
				return "Template: " + fsm.UsedInTemplate.name;
			}
			if (fsm.Owner == null)
			{
				return "FSM Missing Owner";
			}
			return fsm.OwnerName + " : " + GetFsmLabel(fsm);
		}

		public static string GetFullFsmLabel(PlayMakerFSM fsm)
		{
			if (fsm == null)
			{
				return "None (PlayMakerFSM)";
			}
			if (fsm.Fsm == null)
			{
				return "None (Fsm)";
			}
			return fsm.gameObject.name + " : " + fsm.FsmName;
		}

		public static string GetFsmLabel(Fsm fsm)
		{
			if (fsm != null)
			{
				return fsm.Name;
			}
			return "None (Fsm)";
		}

		public static UnityEngine.Object GetOwner(Fsm fsm)
		{
			if (fsm == null)
			{
				return null;
			}
			if ((bool)fsm.UsedInTemplate)
			{
				return fsm.UsedInTemplate;
			}
			return fsm.Owner;
		}
	}
}
