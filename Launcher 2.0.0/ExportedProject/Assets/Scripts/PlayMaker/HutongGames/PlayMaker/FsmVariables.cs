using System;
using System.Collections.Generic;
using HutongGames.Utility;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmVariables
	{
		[SerializeField]
		private FsmFloat[] floatVariables;

		[SerializeField]
		private FsmInt[] intVariables;

		[SerializeField]
		private FsmBool[] boolVariables;

		[SerializeField]
		private FsmString[] stringVariables;

		[SerializeField]
		private FsmVector2[] vector2Variables;

		[SerializeField]
		private FsmVector3[] vector3Variables;

		[SerializeField]
		private FsmColor[] colorVariables;

		[SerializeField]
		private FsmRect[] rectVariables;

		[SerializeField]
		private FsmQuaternion[] quaternionVariables;

		[SerializeField]
		private FsmGameObject[] gameObjectVariables;

		[SerializeField]
		private FsmObject[] objectVariables;

		[SerializeField]
		private FsmMaterial[] materialVariables;

		[SerializeField]
		private FsmTexture[] textureVariables;

		[SerializeField]
		private FsmArray[] arrayVariables;

		[SerializeField]
		private FsmEnum[] enumVariables;

		[SerializeField]
		private string[] categories = new string[1] { "" };

		[SerializeField]
		private int[] variableCategoryIDs = new int[0];

		public static PlayMakerGlobals GlobalsComponent
		{
			get
			{
				return PlayMakerGlobals.Instance;
			}
		}

		public static FsmVariables GlobalVariables
		{
			get
			{
				return PlayMakerGlobals.Instance.Variables;
			}
		}

		public static bool GlobalVariablesSynced { get; set; }

		public string[] Categories
		{
			get
			{
				return categories;
			}
			set
			{
				categories = value;
			}
		}

		public int[] CategoryIDs
		{
			get
			{
				return variableCategoryIDs;
			}
			set
			{
				variableCategoryIDs = value;
			}
		}

		public FsmFloat[] FloatVariables
		{
			get
			{
				return floatVariables ?? Arrays<FsmFloat>.Empty;
			}
			set
			{
				floatVariables = value;
			}
		}

		public FsmInt[] IntVariables
		{
			get
			{
				return intVariables ?? Arrays<FsmInt>.Empty;
			}
			set
			{
				intVariables = value;
			}
		}

		public FsmBool[] BoolVariables
		{
			get
			{
				return boolVariables ?? Arrays<FsmBool>.Empty;
			}
			set
			{
				boolVariables = value;
			}
		}

		public FsmString[] StringVariables
		{
			get
			{
				return stringVariables ?? Arrays<FsmString>.Empty;
			}
			set
			{
				stringVariables = value;
			}
		}

		public FsmVector2[] Vector2Variables
		{
			get
			{
				return vector2Variables ?? Arrays<FsmVector2>.Empty;
			}
			set
			{
				vector2Variables = value;
			}
		}

		public FsmVector3[] Vector3Variables
		{
			get
			{
				return vector3Variables ?? Arrays<FsmVector3>.Empty;
			}
			set
			{
				vector3Variables = value;
			}
		}

		public FsmRect[] RectVariables
		{
			get
			{
				return rectVariables ?? Arrays<FsmRect>.Empty;
			}
			set
			{
				rectVariables = value;
			}
		}

		public FsmQuaternion[] QuaternionVariables
		{
			get
			{
				return quaternionVariables ?? Arrays<FsmQuaternion>.Empty;
			}
			set
			{
				quaternionVariables = value;
			}
		}

		public FsmColor[] ColorVariables
		{
			get
			{
				return colorVariables ?? Arrays<FsmColor>.Empty;
			}
			set
			{
				colorVariables = value;
			}
		}

		public FsmGameObject[] GameObjectVariables
		{
			get
			{
				return gameObjectVariables ?? Arrays<FsmGameObject>.Empty;
			}
			set
			{
				gameObjectVariables = value;
			}
		}

		public FsmArray[] ArrayVariables
		{
			get
			{
				return arrayVariables ?? Arrays<FsmArray>.Empty;
			}
			set
			{
				arrayVariables = value;
			}
		}

		public FsmEnum[] EnumVariables
		{
			get
			{
				return enumVariables ?? Arrays<FsmEnum>.Empty;
			}
			set
			{
				enumVariables = value;
			}
		}

		public FsmObject[] ObjectVariables
		{
			get
			{
				return objectVariables ?? Arrays<FsmObject>.Empty;
			}
			set
			{
				objectVariables = value;
			}
		}

		public FsmMaterial[] MaterialVariables
		{
			get
			{
				return materialVariables ?? Arrays<FsmMaterial>.Empty;
			}
			set
			{
				materialVariables = value;
			}
		}

		public FsmTexture[] TextureVariables
		{
			get
			{
				return textureVariables ?? Arrays<FsmTexture>.Empty;
			}
			set
			{
				textureVariables = value;
			}
		}

		public NamedVariable[] GetAllNamedVariables()
		{
			List<NamedVariable> list = new List<NamedVariable>();
			list.AddRange(FloatVariables);
			list.AddRange(IntVariables);
			list.AddRange(BoolVariables);
			list.AddRange(StringVariables);
			list.AddRange(Vector2Variables);
			list.AddRange(Vector3Variables);
			list.AddRange(RectVariables);
			list.AddRange(QuaternionVariables);
			list.AddRange(GameObjectVariables);
			list.AddRange(ObjectVariables);
			list.AddRange(MaterialVariables);
			list.AddRange(TextureVariables);
			list.AddRange(ColorVariables);
			list.AddRange(ArrayVariables);
			list.AddRange(EnumVariables);
			return list.ToArray();
		}

		public NamedVariable[] GetNamedVariables(VariableType type)
		{
			switch (type)
			{
			case VariableType.Float:
				return FloatVariables;
			case VariableType.Int:
				return IntVariables;
			case VariableType.Bool:
				return BoolVariables;
			case VariableType.GameObject:
				return GameObjectVariables;
			case VariableType.String:
				return StringVariables;
			case VariableType.Vector2:
				return Vector2Variables;
			case VariableType.Vector3:
				return Vector3Variables;
			case VariableType.Color:
				return ColorVariables;
			case VariableType.Rect:
				return RectVariables;
			case VariableType.Material:
				return MaterialVariables;
			case VariableType.Texture:
				return TextureVariables;
			case VariableType.Quaternion:
				return QuaternionVariables;
			case VariableType.Object:
				return ObjectVariables;
			case VariableType.Array:
				return ArrayVariables;
			case VariableType.Enum:
				return EnumVariables;
			case VariableType.Unknown:
				return GetAllNamedVariables();
			default:
				throw new ArgumentOutOfRangeException("type");
			}
		}

		public bool Contains(string variableName)
		{
			NamedVariable[] allNamedVariables = GetAllNamedVariables();
			NamedVariable[] array = allNamedVariables;
			foreach (NamedVariable namedVariable in array)
			{
				if (namedVariable.Name == variableName)
				{
					return true;
				}
			}
			return false;
		}

		public bool Contains(NamedVariable variable)
		{
			NamedVariable[] allNamedVariables = GetAllNamedVariables();
			NamedVariable[] array = allNamedVariables;
			foreach (NamedVariable namedVariable in array)
			{
				if (namedVariable == variable)
				{
					return true;
				}
			}
			return false;
		}

		public NamedVariable[] GetNames(Type ofType)
		{
			if (object.ReferenceEquals(ofType, typeof(FsmFloat)))
			{
				return FloatVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmInt)))
			{
				return IntVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmBool)))
			{
				return BoolVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmString)))
			{
				return StringVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmVector2)))
			{
				return Vector2Variables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmVector3)))
			{
				return Vector3Variables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmRect)))
			{
				return RectVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmQuaternion)))
			{
				return QuaternionVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmObject)))
			{
				return ObjectVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmMaterial)))
			{
				return MaterialVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmTexture)))
			{
				return TextureVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmColor)))
			{
				return ColorVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmGameObject)))
			{
				return GameObjectVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmArray)))
			{
				return ArrayVariables;
			}
			if (object.ReferenceEquals(ofType, typeof(FsmEnum)))
			{
				return EnumVariables;
			}
			return new NamedVariable[0];
		}

		public FsmVariables()
		{
		}

		public FsmVariables(FsmVariables source)
		{
			if (source == null)
			{
				return;
			}
			if (source.floatVariables != null)
			{
				floatVariables = new FsmFloat[source.floatVariables.Length];
				for (int i = 0; i < source.floatVariables.Length; i++)
				{
					floatVariables[i] = new FsmFloat(source.floatVariables[i]);
				}
			}
			if (source.intVariables != null)
			{
				intVariables = new FsmInt[source.intVariables.Length];
				for (int j = 0; j < source.intVariables.Length; j++)
				{
					intVariables[j] = new FsmInt(source.intVariables[j]);
				}
			}
			if (source.boolVariables != null)
			{
				boolVariables = new FsmBool[source.boolVariables.Length];
				for (int k = 0; k < source.boolVariables.Length; k++)
				{
					boolVariables[k] = new FsmBool(source.boolVariables[k]);
				}
			}
			if (source.gameObjectVariables != null)
			{
				gameObjectVariables = new FsmGameObject[source.gameObjectVariables.Length];
				for (int l = 0; l < source.gameObjectVariables.Length; l++)
				{
					gameObjectVariables[l] = new FsmGameObject(source.gameObjectVariables[l]);
				}
			}
			if (source.colorVariables != null)
			{
				colorVariables = new FsmColor[source.colorVariables.Length];
				for (int m = 0; m < source.colorVariables.Length; m++)
				{
					colorVariables[m] = new FsmColor(source.colorVariables[m]);
				}
			}
			if (source.vector2Variables != null)
			{
				vector2Variables = new FsmVector2[source.vector2Variables.Length];
				for (int n = 0; n < source.vector2Variables.Length; n++)
				{
					vector2Variables[n] = new FsmVector2(source.vector2Variables[n]);
				}
			}
			if (source.vector3Variables != null)
			{
				vector3Variables = new FsmVector3[source.vector3Variables.Length];
				for (int num = 0; num < source.vector3Variables.Length; num++)
				{
					vector3Variables[num] = new FsmVector3(source.vector3Variables[num]);
				}
			}
			if (source.rectVariables != null)
			{
				rectVariables = new FsmRect[source.rectVariables.Length];
				for (int num2 = 0; num2 < source.rectVariables.Length; num2++)
				{
					rectVariables[num2] = new FsmRect(source.rectVariables[num2]);
				}
			}
			if (source.quaternionVariables != null)
			{
				quaternionVariables = new FsmQuaternion[source.quaternionVariables.Length];
				for (int num3 = 0; num3 < source.quaternionVariables.Length; num3++)
				{
					quaternionVariables[num3] = new FsmQuaternion(source.quaternionVariables[num3]);
				}
			}
			if (source.objectVariables != null)
			{
				objectVariables = new FsmObject[source.objectVariables.Length];
				for (int num4 = 0; num4 < source.objectVariables.Length; num4++)
				{
					objectVariables[num4] = new FsmObject(source.objectVariables[num4]);
				}
			}
			if (source.materialVariables != null)
			{
				materialVariables = new FsmMaterial[source.materialVariables.Length];
				for (int num5 = 0; num5 < source.materialVariables.Length; num5++)
				{
					materialVariables[num5] = new FsmMaterial(source.materialVariables[num5]);
				}
			}
			if (source.textureVariables != null)
			{
				textureVariables = new FsmTexture[source.textureVariables.Length];
				for (int num6 = 0; num6 < source.textureVariables.Length; num6++)
				{
					textureVariables[num6] = new FsmTexture(source.textureVariables[num6]);
				}
			}
			if (source.stringVariables != null)
			{
				stringVariables = new FsmString[source.stringVariables.Length];
				for (int num7 = 0; num7 < source.stringVariables.Length; num7++)
				{
					stringVariables[num7] = new FsmString(source.stringVariables[num7]);
				}
			}
			if (source.arrayVariables != null)
			{
				arrayVariables = new FsmArray[source.arrayVariables.Length];
				for (int num8 = 0; num8 < source.arrayVariables.Length; num8++)
				{
					arrayVariables[num8] = new FsmArray(source.arrayVariables[num8]);
				}
			}
			if (source.enumVariables != null)
			{
				enumVariables = new FsmEnum[source.enumVariables.Length];
				for (int num9 = 0; num9 < source.enumVariables.Length; num9++)
				{
					enumVariables[num9] = new FsmEnum(source.enumVariables[num9]);
				}
			}
			if (source.categories != null)
			{
				categories = new string[source.categories.Length];
				for (int num10 = 0; num10 < source.categories.Length; num10++)
				{
					categories[num10] = source.categories[num10];
				}
			}
			if (source.CategoryIDs != null)
			{
				CategoryIDs = new int[source.CategoryIDs.Length];
				for (int num11 = 0; num11 < source.CategoryIDs.Length; num11++)
				{
					CategoryIDs[num11] = source.CategoryIDs[num11];
				}
			}
			if (source.Categories != null)
			{
				Categories = new string[source.Categories.Length];
				for (int num12 = 0; num12 < source.Categories.Length; num12++)
				{
					Categories[num12] = source.Categories[num12];
				}
			}
		}

		public void OverrideVariableValues(FsmVariables source)
		{
			for (int i = 0; i < source.FloatVariables.Length; i++)
			{
				for (int j = 0; j < FloatVariables.Length; j++)
				{
					if (floatVariables[j].ShowInInspector && source.floatVariables[i].Name == floatVariables[j].Name)
					{
						floatVariables[j].Value = source.floatVariables[i].Value;
					}
				}
			}
			for (int k = 0; k < source.IntVariables.Length; k++)
			{
				for (int l = 0; l < IntVariables.Length; l++)
				{
					if (intVariables[l].ShowInInspector && source.intVariables[k].Name == intVariables[l].Name)
					{
						intVariables[l].Value = source.intVariables[k].Value;
					}
				}
			}
			for (int m = 0; m < source.BoolVariables.Length; m++)
			{
				for (int n = 0; n < BoolVariables.Length; n++)
				{
					if (boolVariables[n].ShowInInspector && source.boolVariables[m].Name == boolVariables[n].Name)
					{
						boolVariables[n].Value = source.boolVariables[m].Value;
					}
				}
			}
			for (int num = 0; num < source.GameObjectVariables.Length; num++)
			{
				for (int num2 = 0; num2 < GameObjectVariables.Length; num2++)
				{
					if (gameObjectVariables[num2].ShowInInspector && source.gameObjectVariables[num].Name == gameObjectVariables[num2].Name)
					{
						gameObjectVariables[num2].Value = source.gameObjectVariables[num].Value;
					}
				}
			}
			for (int num3 = 0; num3 < source.ColorVariables.Length; num3++)
			{
				for (int num4 = 0; num4 < ColorVariables.Length; num4++)
				{
					if (colorVariables[num4].ShowInInspector && source.colorVariables[num3].Name == colorVariables[num4].Name)
					{
						colorVariables[num4].Value = source.colorVariables[num3].Value;
					}
				}
			}
			for (int num5 = 0; num5 < source.Vector2Variables.Length; num5++)
			{
				for (int num6 = 0; num6 < Vector2Variables.Length; num6++)
				{
					if (vector2Variables[num6].ShowInInspector && source.vector2Variables[num5].Name == vector2Variables[num6].Name)
					{
						vector2Variables[num6].Value = source.vector2Variables[num5].Value;
					}
				}
			}
			for (int num7 = 0; num7 < source.Vector3Variables.Length; num7++)
			{
				for (int num8 = 0; num8 < Vector3Variables.Length; num8++)
				{
					if (vector3Variables[num8].ShowInInspector && source.vector3Variables[num7].Name == vector3Variables[num8].Name)
					{
						vector3Variables[num8].Value = source.vector3Variables[num7].Value;
					}
				}
			}
			for (int num9 = 0; num9 < source.RectVariables.Length; num9++)
			{
				for (int num10 = 0; num10 < RectVariables.Length; num10++)
				{
					if (rectVariables[num10].ShowInInspector && source.rectVariables[num9].Name == rectVariables[num10].Name)
					{
						rectVariables[num10].Value = source.rectVariables[num9].Value;
					}
				}
			}
			for (int num11 = 0; num11 < source.QuaternionVariables.Length; num11++)
			{
				for (int num12 = 0; num12 < QuaternionVariables.Length; num12++)
				{
					if (quaternionVariables[num12].ShowInInspector && source.quaternionVariables[num11].Name == quaternionVariables[num12].Name)
					{
						quaternionVariables[num12].Value = source.quaternionVariables[num11].Value;
					}
				}
			}
			for (int num13 = 0; num13 < source.ObjectVariables.Length; num13++)
			{
				for (int num14 = 0; num14 < ObjectVariables.Length; num14++)
				{
					if (objectVariables[num14].ShowInInspector && source.objectVariables[num13].Name == objectVariables[num14].Name)
					{
						objectVariables[num14].Value = source.objectVariables[num13].Value;
					}
				}
			}
			for (int num15 = 0; num15 < source.MaterialVariables.Length; num15++)
			{
				for (int num16 = 0; num16 < MaterialVariables.Length; num16++)
				{
					if (materialVariables[num16].ShowInInspector && source.materialVariables[num15].Name == materialVariables[num16].Name)
					{
						materialVariables[num16].Value = source.materialVariables[num15].Value;
					}
				}
			}
			for (int num17 = 0; num17 < source.TextureVariables.Length; num17++)
			{
				for (int num18 = 0; num18 < TextureVariables.Length; num18++)
				{
					if (textureVariables[num18].ShowInInspector && source.textureVariables[num17].Name == textureVariables[num18].Name)
					{
						textureVariables[num18].Value = source.textureVariables[num17].Value;
					}
				}
			}
			for (int num19 = 0; num19 < source.StringVariables.Length; num19++)
			{
				for (int num20 = 0; num20 < StringVariables.Length; num20++)
				{
					if (stringVariables[num20].ShowInInspector && source.stringVariables[num19].Name == stringVariables[num20].Name)
					{
						stringVariables[num20].Value = source.stringVariables[num19].Value;
					}
				}
			}
			for (int num21 = 0; num21 < source.ArrayVariables.Length; num21++)
			{
				for (int num22 = 0; num22 < ArrayVariables.Length; num22++)
				{
					if (arrayVariables[num22].ShowInInspector && source.arrayVariables[num21].Name == arrayVariables[num22].Name)
					{
						arrayVariables[num22].CopyValues(source.arrayVariables[num21]);
					}
				}
			}
			for (int num23 = 0; num23 < source.EnumVariables.Length; num23++)
			{
				for (int num24 = 0; num24 < EnumVariables.Length; num24++)
				{
					if (enumVariables[num24].ShowInInspector && source.enumVariables[num23].Name == enumVariables[num24].Name)
					{
						enumVariables[num24].Value = source.enumVariables[num23].Value;
					}
				}
			}
		}

		public void ApplyVariableValues(FsmVariables source)
		{
			if (source != null)
			{
				for (int i = 0; i < source.FloatVariables.Length; i++)
				{
					floatVariables[i].Value = source.floatVariables[i].Value;
				}
				for (int j = 0; j < source.IntVariables.Length; j++)
				{
					intVariables[j].Value = source.intVariables[j].Value;
				}
				for (int k = 0; k < source.BoolVariables.Length; k++)
				{
					boolVariables[k].Value = source.boolVariables[k].Value;
				}
				for (int l = 0; l < source.GameObjectVariables.Length; l++)
				{
					gameObjectVariables[l].Value = source.gameObjectVariables[l].Value;
				}
				for (int m = 0; m < source.ColorVariables.Length; m++)
				{
					colorVariables[m].Value = source.colorVariables[m].Value;
				}
				for (int n = 0; n < source.Vector2Variables.Length; n++)
				{
					vector2Variables[n].Value = source.vector2Variables[n].Value;
				}
				for (int num = 0; num < source.Vector3Variables.Length; num++)
				{
					vector3Variables[num].Value = source.vector3Variables[num].Value;
				}
				for (int num2 = 0; num2 < source.RectVariables.Length; num2++)
				{
					rectVariables[num2].Value = source.rectVariables[num2].Value;
				}
				for (int num3 = 0; num3 < source.QuaternionVariables.Length; num3++)
				{
					quaternionVariables[num3].Value = source.quaternionVariables[num3].Value;
				}
				for (int num4 = 0; num4 < source.ObjectVariables.Length; num4++)
				{
					objectVariables[num4].Value = source.objectVariables[num4].Value;
				}
				for (int num5 = 0; num5 < source.MaterialVariables.Length; num5++)
				{
					materialVariables[num5].Value = source.materialVariables[num5].Value;
				}
				for (int num6 = 0; num6 < source.TextureVariables.Length; num6++)
				{
					textureVariables[num6].Value = source.textureVariables[num6].Value;
				}
				for (int num7 = 0; num7 < source.StringVariables.Length; num7++)
				{
					stringVariables[num7].Value = source.stringVariables[num7].Value;
				}
				for (int num8 = 0; num8 < source.EnumVariables.Length; num8++)
				{
					enumVariables[num8].Value = source.enumVariables[num8].Value;
				}
				for (int num9 = 0; num9 < source.ArrayVariables.Length; num9++)
				{
					arrayVariables[num9].CopyValues(source.arrayVariables[num9]);
				}
			}
		}

		public void ApplyVariableValuesCareful(FsmVariables source)
		{
			if (source == null)
			{
				return;
			}
			for (int i = 0; i < source.FloatVariables.Length; i++)
			{
				FsmFloat fsmFloat = FindFsmFloat(source.floatVariables[i].Name);
				if (fsmFloat != null)
				{
					fsmFloat.Value = source.floatVariables[i].Value;
				}
			}
			for (int j = 0; j < source.IntVariables.Length; j++)
			{
				FsmInt fsmInt = FindFsmInt(source.IntVariables[j].Name);
				if (fsmInt != null)
				{
					fsmInt.Value = source.IntVariables[j].Value;
				}
			}
			for (int k = 0; k < source.BoolVariables.Length; k++)
			{
				FsmBool fsmBool = FindFsmBool(source.BoolVariables[k].Name);
				if (fsmBool != null)
				{
					fsmBool.Value = source.BoolVariables[k].Value;
				}
			}
			for (int l = 0; l < source.GameObjectVariables.Length; l++)
			{
				FsmBool fsmBool2 = FindFsmBool(source.BoolVariables[l].Name);
				if (fsmBool2 != null)
				{
					fsmBool2.Value = source.BoolVariables[l].Value;
				}
			}
			for (int m = 0; m < source.ColorVariables.Length; m++)
			{
				FsmBool fsmBool3 = FindFsmBool(source.BoolVariables[m].Name);
				if (fsmBool3 != null)
				{
					fsmBool3.Value = source.BoolVariables[m].Value;
				}
			}
			for (int n = 0; n < source.Vector2Variables.Length; n++)
			{
				FsmBool fsmBool4 = FindFsmBool(source.BoolVariables[n].Name);
				if (fsmBool4 != null)
				{
					fsmBool4.Value = source.BoolVariables[n].Value;
				}
			}
			for (int num = 0; num < source.Vector3Variables.Length; num++)
			{
				FsmBool fsmBool5 = FindFsmBool(source.BoolVariables[num].Name);
				if (fsmBool5 != null)
				{
					fsmBool5.Value = source.BoolVariables[num].Value;
				}
			}
			for (int num2 = 0; num2 < source.RectVariables.Length; num2++)
			{
				FsmRect fsmRect = FindFsmRect(source.RectVariables[num2].Name);
				if (fsmRect != null)
				{
					fsmRect.Value = source.RectVariables[num2].Value;
				}
			}
			for (int num3 = 0; num3 < source.QuaternionVariables.Length; num3++)
			{
				FsmQuaternion fsmQuaternion = FindFsmQuaternion(source.QuaternionVariables[num3].Name);
				if (fsmQuaternion != null)
				{
					fsmQuaternion.Value = source.QuaternionVariables[num3].Value;
				}
			}
			for (int num4 = 0; num4 < source.ObjectVariables.Length; num4++)
			{
				FsmObject fsmObject = FindFsmObject(source.ObjectVariables[num4].Name);
				if (fsmObject != null)
				{
					fsmObject.Value = source.ObjectVariables[num4].Value;
				}
			}
			for (int num5 = 0; num5 < source.MaterialVariables.Length; num5++)
			{
				FsmMaterial fsmMaterial = FindFsmMaterial(source.MaterialVariables[num5].Name);
				if (fsmMaterial != null)
				{
					fsmMaterial.Value = source.MaterialVariables[num5].Value;
				}
			}
			for (int num6 = 0; num6 < source.TextureVariables.Length; num6++)
			{
				FsmTexture fsmTexture = FindFsmTexture(source.TextureVariables[num6].Name);
				if (fsmTexture != null)
				{
					fsmTexture.Value = source.TextureVariables[num6].Value;
				}
			}
			for (int num7 = 0; num7 < source.StringVariables.Length; num7++)
			{
				FsmString fsmString = FindFsmString(source.StringVariables[num7].Name);
				if (fsmString != null)
				{
					fsmString.Value = source.StringVariables[num7].Value;
				}
			}
			for (int num8 = 0; num8 < source.EnumVariables.Length; num8++)
			{
				FsmEnum fsmEnum = FindFsmEnum(source.EnumVariables[num8].Name);
				if (fsmEnum != null)
				{
					fsmEnum.Value = source.EnumVariables[num8].Value;
				}
			}
			for (int num9 = 0; num9 < source.ArrayVariables.Length; num9++)
			{
				FsmArray fsmArray = FindFsmArray(source.ArrayVariables[num9].Name);
				if (fsmArray != null)
				{
					fsmArray.CopyValues(source.arrayVariables[num9]);
				}
			}
		}

		public NamedVariable GetVariable(string name)
		{
			FsmFloat[] array = FloatVariables;
			foreach (FsmFloat fsmFloat in array)
			{
				if (fsmFloat.Name == name)
				{
					return fsmFloat;
				}
			}
			FsmInt[] array2 = IntVariables;
			foreach (FsmInt fsmInt in array2)
			{
				if (fsmInt.Name == name)
				{
					return fsmInt;
				}
			}
			FsmBool[] array3 = BoolVariables;
			foreach (FsmBool fsmBool in array3)
			{
				if (fsmBool.Name == name)
				{
					return fsmBool;
				}
			}
			FsmVector2[] array4 = Vector2Variables;
			foreach (FsmVector2 fsmVector in array4)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			FsmVector3[] array5 = Vector3Variables;
			foreach (FsmVector3 fsmVector2 in array5)
			{
				if (fsmVector2.Name == name)
				{
					return fsmVector2;
				}
			}
			FsmString[] array6 = StringVariables;
			foreach (FsmString fsmString in array6)
			{
				if (fsmString.Name == name)
				{
					return fsmString;
				}
			}
			FsmRect[] array7 = RectVariables;
			foreach (FsmRect fsmRect in array7)
			{
				if (fsmRect.Name == name)
				{
					return fsmRect;
				}
			}
			FsmColor[] array8 = ColorVariables;
			foreach (FsmColor fsmColor in array8)
			{
				if (fsmColor.Name == name)
				{
					return fsmColor;
				}
			}
			FsmMaterial[] array9 = MaterialVariables;
			foreach (FsmMaterial fsmMaterial in array9)
			{
				if (fsmMaterial.Name == name)
				{
					return fsmMaterial;
				}
			}
			FsmTexture[] array10 = TextureVariables;
			foreach (FsmTexture fsmTexture in array10)
			{
				if (fsmTexture.Name == name)
				{
					return fsmTexture;
				}
			}
			FsmObject[] array11 = ObjectVariables;
			foreach (FsmObject fsmObject in array11)
			{
				if (fsmObject.Name == name)
				{
					return fsmObject;
				}
			}
			FsmGameObject[] array12 = GameObjectVariables;
			foreach (FsmGameObject fsmGameObject in array12)
			{
				if (fsmGameObject.Name == name)
				{
					return fsmGameObject;
				}
			}
			FsmQuaternion[] array13 = QuaternionVariables;
			foreach (FsmQuaternion fsmQuaternion in array13)
			{
				if (fsmQuaternion.Name == name)
				{
					return fsmQuaternion;
				}
			}
			FsmEnum[] array14 = EnumVariables;
			foreach (FsmEnum fsmEnum in array14)
			{
				if (fsmEnum.Name == name)
				{
					return fsmEnum;
				}
			}
			FsmArray[] array15 = ArrayVariables;
			foreach (FsmArray fsmArray in array15)
			{
				if (fsmArray.Name == name)
				{
					return fsmArray;
				}
			}
			if (GlobalVariables != null)
			{
				array = GlobalVariables.FloatVariables;
				foreach (FsmFloat fsmFloat2 in array)
				{
					if (fsmFloat2.Name == name)
					{
						return fsmFloat2;
					}
				}
				array2 = GlobalVariables.IntVariables;
				foreach (FsmInt fsmInt2 in array2)
				{
					if (fsmInt2.Name == name)
					{
						return fsmInt2;
					}
				}
				array3 = GlobalVariables.BoolVariables;
				foreach (FsmBool fsmBool2 in array3)
				{
					if (fsmBool2.Name == name)
					{
						return fsmBool2;
					}
				}
				array4 = GlobalVariables.Vector2Variables;
				foreach (FsmVector2 fsmVector3 in array4)
				{
					if (fsmVector3.Name == name)
					{
						return fsmVector3;
					}
				}
				array5 = GlobalVariables.Vector3Variables;
				foreach (FsmVector3 fsmVector4 in array5)
				{
					if (fsmVector4.Name == name)
					{
						return fsmVector4;
					}
				}
				array6 = GlobalVariables.StringVariables;
				foreach (FsmString fsmString2 in array6)
				{
					if (fsmString2.Name == name)
					{
						return fsmString2;
					}
				}
				array7 = GlobalVariables.RectVariables;
				foreach (FsmRect fsmRect2 in array7)
				{
					if (fsmRect2.Name == name)
					{
						return fsmRect2;
					}
				}
				array8 = GlobalVariables.ColorVariables;
				foreach (FsmColor fsmColor2 in array8)
				{
					if (fsmColor2.Name == name)
					{
						return fsmColor2;
					}
				}
				array9 = GlobalVariables.MaterialVariables;
				foreach (FsmMaterial fsmMaterial2 in array9)
				{
					if (fsmMaterial2.Name == name)
					{
						return fsmMaterial2;
					}
				}
				array10 = GlobalVariables.TextureVariables;
				foreach (FsmTexture fsmTexture2 in array10)
				{
					if (fsmTexture2.Name == name)
					{
						return fsmTexture2;
					}
				}
				array11 = GlobalVariables.ObjectVariables;
				foreach (FsmObject fsmObject2 in array11)
				{
					if (fsmObject2.Name == name)
					{
						return fsmObject2;
					}
				}
				array12 = GlobalVariables.GameObjectVariables;
				foreach (FsmGameObject fsmGameObject2 in array12)
				{
					if (fsmGameObject2.Name == name)
					{
						return fsmGameObject2;
					}
				}
				array13 = GlobalVariables.QuaternionVariables;
				foreach (FsmQuaternion fsmQuaternion2 in array13)
				{
					if (fsmQuaternion2.Name == name)
					{
						return fsmQuaternion2;
					}
				}
				array14 = GlobalVariables.EnumVariables;
				foreach (FsmEnum fsmEnum2 in array14)
				{
					if (fsmEnum2.Name == name)
					{
						return fsmEnum2;
					}
				}
				array15 = GlobalVariables.ArrayVariables;
				foreach (FsmArray fsmArray2 in array15)
				{
					if (fsmArray2.Name == name)
					{
						return fsmArray2;
					}
				}
			}
			return null;
		}

		public FsmFloat GetFsmFloat(string name)
		{
			FsmFloat[] array = FloatVariables;
			foreach (FsmFloat fsmFloat in array)
			{
				if (fsmFloat.Name == name)
				{
					return fsmFloat;
				}
			}
			if (GlobalVariables != null)
			{
				FsmFloat[] array2 = GlobalVariables.FloatVariables;
				foreach (FsmFloat fsmFloat2 in array2)
				{
					if (fsmFloat2.Name == name)
					{
						return fsmFloat2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmFloat(name);
		}

		public FsmObject GetFsmObject(string name)
		{
			FsmObject[] array = ObjectVariables;
			foreach (FsmObject fsmObject in array)
			{
				if (fsmObject.Name == name)
				{
					return fsmObject;
				}
			}
			if (GlobalVariables != null)
			{
				FsmObject[] array2 = GlobalVariables.ObjectVariables;
				foreach (FsmObject fsmObject2 in array2)
				{
					if (fsmObject2.Name == name)
					{
						return fsmObject2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmObject(name);
		}

		public FsmMaterial GetFsmMaterial(string name)
		{
			FsmMaterial[] array = MaterialVariables;
			foreach (FsmMaterial fsmMaterial in array)
			{
				if (fsmMaterial.Name == name)
				{
					return fsmMaterial;
				}
			}
			if (GlobalVariables != null)
			{
				FsmMaterial[] array2 = GlobalVariables.MaterialVariables;
				foreach (FsmMaterial fsmMaterial2 in array2)
				{
					if (fsmMaterial2.Name == name)
					{
						return fsmMaterial2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmMaterial(name);
		}

		public FsmTexture GetFsmTexture(string name)
		{
			FsmTexture[] array = TextureVariables;
			foreach (FsmTexture fsmTexture in array)
			{
				if (fsmTexture.Name == name)
				{
					return fsmTexture;
				}
			}
			if (GlobalVariables != null)
			{
				FsmTexture[] array2 = GlobalVariables.TextureVariables;
				foreach (FsmTexture fsmTexture2 in array2)
				{
					if (fsmTexture2.Name == name)
					{
						return fsmTexture2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmTexture(name);
		}

		public FsmInt GetFsmInt(string name)
		{
			FsmInt[] array = IntVariables;
			foreach (FsmInt fsmInt in array)
			{
				if (fsmInt.Name == name)
				{
					return fsmInt;
				}
			}
			if (GlobalVariables != null)
			{
				FsmInt[] array2 = GlobalVariables.IntVariables;
				foreach (FsmInt fsmInt2 in array2)
				{
					if (fsmInt2.Name == name)
					{
						return fsmInt2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmInt(name);
		}

		public FsmBool GetFsmBool(string name)
		{
			FsmBool[] array = BoolVariables;
			foreach (FsmBool fsmBool in array)
			{
				if (fsmBool.Name == name)
				{
					return fsmBool;
				}
			}
			if (GlobalVariables != null)
			{
				FsmBool[] array2 = GlobalVariables.BoolVariables;
				foreach (FsmBool fsmBool2 in array2)
				{
					if (fsmBool2.Name == name)
					{
						return fsmBool2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmBool(name);
		}

		public FsmString GetFsmString(string name)
		{
			FsmString[] array = StringVariables;
			foreach (FsmString fsmString in array)
			{
				if (fsmString.Name == name)
				{
					return fsmString;
				}
			}
			if (GlobalVariables != null)
			{
				FsmString[] array2 = GlobalVariables.StringVariables;
				foreach (FsmString fsmString2 in array2)
				{
					if (fsmString2.Name == name)
					{
						return fsmString2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmString(name);
		}

		public FsmVector2 GetFsmVector2(string name)
		{
			FsmVector2[] array = Vector2Variables;
			foreach (FsmVector2 fsmVector in array)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			if (GlobalVariables != null)
			{
				FsmVector2[] array2 = GlobalVariables.Vector2Variables;
				foreach (FsmVector2 fsmVector2 in array2)
				{
					if (fsmVector2.Name == name)
					{
						return fsmVector2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmVector2(name);
		}

		public FsmVector3 GetFsmVector3(string name)
		{
			FsmVector3[] array = Vector3Variables;
			foreach (FsmVector3 fsmVector in array)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			if (GlobalVariables != null)
			{
				FsmVector3[] array2 = GlobalVariables.Vector3Variables;
				foreach (FsmVector3 fsmVector2 in array2)
				{
					if (fsmVector2.Name == name)
					{
						return fsmVector2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmVector3(name);
		}

		public FsmRect GetFsmRect(string name)
		{
			FsmRect[] array = RectVariables;
			foreach (FsmRect fsmRect in array)
			{
				if (fsmRect.Name == name)
				{
					return fsmRect;
				}
			}
			if (GlobalVariables != null)
			{
				FsmRect[] array2 = GlobalVariables.RectVariables;
				foreach (FsmRect fsmRect2 in array2)
				{
					if (fsmRect2.Name == name)
					{
						return fsmRect2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmRect(name);
		}

		public FsmQuaternion GetFsmQuaternion(string name)
		{
			FsmQuaternion[] array = QuaternionVariables;
			foreach (FsmQuaternion fsmQuaternion in array)
			{
				if (fsmQuaternion.Name == name)
				{
					return fsmQuaternion;
				}
			}
			if (GlobalVariables != null)
			{
				FsmQuaternion[] array2 = GlobalVariables.QuaternionVariables;
				foreach (FsmQuaternion fsmQuaternion2 in array2)
				{
					if (fsmQuaternion2.Name == name)
					{
						return fsmQuaternion2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmQuaternion(name);
		}

		public FsmColor GetFsmColor(string name)
		{
			FsmColor[] array = ColorVariables;
			foreach (FsmColor fsmColor in array)
			{
				if (fsmColor.Name == name)
				{
					return fsmColor;
				}
			}
			if (GlobalVariables != null)
			{
				FsmColor[] array2 = GlobalVariables.ColorVariables;
				foreach (FsmColor fsmColor2 in array2)
				{
					if (fsmColor2.Name == name)
					{
						return fsmColor2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmColor(name);
		}

		public FsmGameObject GetFsmGameObject(string name)
		{
			FsmGameObject[] array = GameObjectVariables;
			foreach (FsmGameObject fsmGameObject in array)
			{
				if (fsmGameObject.Name == name)
				{
					return fsmGameObject;
				}
			}
			if (GlobalVariables != null)
			{
				FsmGameObject[] array2 = GlobalVariables.GameObjectVariables;
				foreach (FsmGameObject fsmGameObject2 in array2)
				{
					if (fsmGameObject2.Name == name)
					{
						return fsmGameObject2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmGameObject(name);
		}

		public FsmArray GetFsmArray(string name)
		{
			FsmArray[] array = ArrayVariables;
			foreach (FsmArray fsmArray in array)
			{
				if (fsmArray.Name == name)
				{
					return fsmArray;
				}
			}
			if (GlobalVariables != null)
			{
				FsmArray[] array2 = GlobalVariables.ArrayVariables;
				foreach (FsmArray fsmArray2 in array2)
				{
					if (fsmArray2.Name == name)
					{
						return fsmArray2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmArray(name);
		}

		public FsmEnum GetFsmEnum(string name)
		{
			FsmEnum[] array = EnumVariables;
			foreach (FsmEnum fsmEnum in array)
			{
				if (fsmEnum.Name == name)
				{
					return fsmEnum;
				}
			}
			if (GlobalVariables != null)
			{
				FsmEnum[] array2 = GlobalVariables.EnumVariables;
				foreach (FsmEnum fsmEnum2 in array2)
				{
					if (fsmEnum2.Name == name)
					{
						return fsmEnum2;
					}
				}
			}
			LogMissingVariable(name);
			return new FsmEnum(name);
		}

		private void LogMissingVariable(string name)
		{
			if (FsmExecutionStack.ExecutingFsm != null)
			{
				ActionHelpers.LogWarning("Missing Variable: " + name);
			}
		}

		public NamedVariable FindVariable(string name)
		{
			FsmFloat[] array = FloatVariables;
			foreach (FsmFloat fsmFloat in array)
			{
				if (fsmFloat.Name == name)
				{
					return fsmFloat;
				}
			}
			FsmInt[] array2 = IntVariables;
			foreach (FsmInt fsmInt in array2)
			{
				if (fsmInt.Name == name)
				{
					return fsmInt;
				}
			}
			FsmBool[] array3 = BoolVariables;
			foreach (FsmBool fsmBool in array3)
			{
				if (fsmBool.Name == name)
				{
					return fsmBool;
				}
			}
			FsmVector2[] array4 = Vector2Variables;
			foreach (FsmVector2 fsmVector in array4)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			FsmVector3[] array5 = Vector3Variables;
			foreach (FsmVector3 fsmVector2 in array5)
			{
				if (fsmVector2.Name == name)
				{
					return fsmVector2;
				}
			}
			FsmString[] array6 = StringVariables;
			foreach (FsmString fsmString in array6)
			{
				if (fsmString.Name == name)
				{
					return fsmString;
				}
			}
			FsmRect[] array7 = RectVariables;
			foreach (FsmRect fsmRect in array7)
			{
				if (fsmRect.Name == name)
				{
					return fsmRect;
				}
			}
			FsmColor[] array8 = ColorVariables;
			foreach (FsmColor fsmColor in array8)
			{
				if (fsmColor.Name == name)
				{
					return fsmColor;
				}
			}
			FsmMaterial[] array9 = MaterialVariables;
			foreach (FsmMaterial fsmMaterial in array9)
			{
				if (fsmMaterial.Name == name)
				{
					return fsmMaterial;
				}
			}
			FsmTexture[] array10 = TextureVariables;
			foreach (FsmTexture fsmTexture in array10)
			{
				if (fsmTexture.Name == name)
				{
					return fsmTexture;
				}
			}
			FsmObject[] array11 = ObjectVariables;
			foreach (FsmObject fsmObject in array11)
			{
				if (fsmObject.Name == name)
				{
					return fsmObject;
				}
			}
			FsmGameObject[] array12 = GameObjectVariables;
			foreach (FsmGameObject fsmGameObject in array12)
			{
				if (fsmGameObject.Name == name)
				{
					return fsmGameObject;
				}
			}
			FsmQuaternion[] array13 = QuaternionVariables;
			foreach (FsmQuaternion fsmQuaternion in array13)
			{
				if (fsmQuaternion.Name == name)
				{
					return fsmQuaternion;
				}
			}
			FsmEnum[] array14 = EnumVariables;
			foreach (FsmEnum fsmEnum in array14)
			{
				if (fsmEnum.Name == name)
				{
					return fsmEnum;
				}
			}
			FsmArray[] array15 = ArrayVariables;
			foreach (FsmArray fsmArray in array15)
			{
				if (fsmArray.Name == name)
				{
					return fsmArray;
				}
			}
			return null;
		}

		public NamedVariable FindVariable(VariableType type, string name)
		{
			switch (type)
			{
			case VariableType.Float:
			{
				FsmFloat[] array14 = FloatVariables;
				foreach (FsmFloat fsmFloat in array14)
				{
					if (fsmFloat.Name == name)
					{
						return fsmFloat;
					}
				}
				break;
			}
			case VariableType.Int:
			{
				FsmInt[] array6 = IntVariables;
				foreach (FsmInt fsmInt in array6)
				{
					if (fsmInt.Name == name)
					{
						return fsmInt;
					}
				}
				break;
			}
			case VariableType.Bool:
			{
				FsmBool[] array10 = BoolVariables;
				foreach (FsmBool fsmBool in array10)
				{
					if (fsmBool.Name == name)
					{
						return fsmBool;
					}
				}
				break;
			}
			case VariableType.GameObject:
			{
				FsmGameObject[] array2 = GameObjectVariables;
				foreach (FsmGameObject fsmGameObject in array2)
				{
					if (fsmGameObject.Name == name)
					{
						return fsmGameObject;
					}
				}
				break;
			}
			case VariableType.String:
			{
				FsmString[] array12 = StringVariables;
				foreach (FsmString fsmString in array12)
				{
					if (fsmString.Name == name)
					{
						return fsmString;
					}
				}
				break;
			}
			case VariableType.Vector2:
			{
				FsmVector2[] array8 = Vector2Variables;
				foreach (FsmVector2 fsmVector2 in array8)
				{
					if (fsmVector2.Name == name)
					{
						return fsmVector2;
					}
				}
				break;
			}
			case VariableType.Vector3:
			{
				FsmVector3[] array4 = Vector3Variables;
				foreach (FsmVector3 fsmVector in array4)
				{
					if (fsmVector.Name == name)
					{
						return fsmVector;
					}
				}
				break;
			}
			case VariableType.Color:
			{
				FsmColor[] array15 = ColorVariables;
				foreach (FsmColor fsmColor in array15)
				{
					if (fsmColor.Name == name)
					{
						return fsmColor;
					}
				}
				break;
			}
			case VariableType.Rect:
			{
				FsmRect[] array13 = RectVariables;
				foreach (FsmRect fsmRect in array13)
				{
					if (fsmRect.Name == name)
					{
						return fsmRect;
					}
				}
				break;
			}
			case VariableType.Material:
			{
				FsmMaterial[] array11 = MaterialVariables;
				foreach (FsmMaterial fsmMaterial in array11)
				{
					if (fsmMaterial.Name == name)
					{
						return fsmMaterial;
					}
				}
				break;
			}
			case VariableType.Texture:
			{
				FsmTexture[] array9 = TextureVariables;
				foreach (FsmTexture fsmTexture in array9)
				{
					if (fsmTexture.Name == name)
					{
						return fsmTexture;
					}
				}
				break;
			}
			case VariableType.Quaternion:
			{
				FsmQuaternion[] array7 = QuaternionVariables;
				foreach (FsmQuaternion fsmQuaternion in array7)
				{
					if (fsmQuaternion.Name == name)
					{
						return fsmQuaternion;
					}
				}
				break;
			}
			case VariableType.Object:
			{
				FsmObject[] array5 = ObjectVariables;
				foreach (FsmObject fsmObject in array5)
				{
					if (fsmObject.Name == name)
					{
						return fsmObject;
					}
				}
				break;
			}
			case VariableType.Array:
			{
				FsmArray[] array3 = ArrayVariables;
				foreach (FsmArray fsmArray in array3)
				{
					if (fsmArray.Name == name)
					{
						return fsmArray;
					}
				}
				break;
			}
			case VariableType.Enum:
			{
				FsmEnum[] array = EnumVariables;
				foreach (FsmEnum fsmEnum in array)
				{
					if (fsmEnum.Name == name)
					{
						return fsmEnum;
					}
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("type");
			case VariableType.Unknown:
				break;
			}
			return null;
		}

		public FsmFloat FindFsmFloat(string name)
		{
			FsmFloat[] array = FloatVariables;
			foreach (FsmFloat fsmFloat in array)
			{
				if (fsmFloat.Name == name)
				{
					return fsmFloat;
				}
			}
			return null;
		}

		public FsmObject FindFsmObject(string name)
		{
			FsmObject[] array = ObjectVariables;
			foreach (FsmObject fsmObject in array)
			{
				if (fsmObject.Name == name)
				{
					return fsmObject;
				}
			}
			return null;
		}

		public FsmMaterial FindFsmMaterial(string name)
		{
			FsmMaterial[] array = MaterialVariables;
			foreach (FsmMaterial fsmMaterial in array)
			{
				if (fsmMaterial.Name == name)
				{
					return fsmMaterial;
				}
			}
			return null;
		}

		public FsmTexture FindFsmTexture(string name)
		{
			FsmTexture[] array = TextureVariables;
			foreach (FsmTexture fsmTexture in array)
			{
				if (fsmTexture.Name == name)
				{
					return fsmTexture;
				}
			}
			return null;
		}

		public FsmInt FindFsmInt(string name)
		{
			FsmInt[] array = IntVariables;
			foreach (FsmInt fsmInt in array)
			{
				if (fsmInt.Name == name)
				{
					return fsmInt;
				}
			}
			return null;
		}

		public FsmBool FindFsmBool(string name)
		{
			FsmBool[] array = BoolVariables;
			foreach (FsmBool fsmBool in array)
			{
				if (fsmBool.Name == name)
				{
					return fsmBool;
				}
			}
			return null;
		}

		public FsmString FindFsmString(string name)
		{
			FsmString[] array = StringVariables;
			foreach (FsmString fsmString in array)
			{
				if (fsmString.Name == name)
				{
					return fsmString;
				}
			}
			return null;
		}

		public FsmVector2 FindFsmVector2(string name)
		{
			FsmVector2[] array = Vector2Variables;
			foreach (FsmVector2 fsmVector in array)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			return null;
		}

		public FsmVector3 FindFsmVector3(string name)
		{
			FsmVector3[] array = Vector3Variables;
			foreach (FsmVector3 fsmVector in array)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			return null;
		}

		public FsmRect FindFsmRect(string name)
		{
			FsmRect[] array = RectVariables;
			foreach (FsmRect fsmRect in array)
			{
				if (fsmRect.Name == name)
				{
					return fsmRect;
				}
			}
			return null;
		}

		public FsmQuaternion FindFsmQuaternion(string name)
		{
			FsmQuaternion[] array = QuaternionVariables;
			foreach (FsmQuaternion fsmQuaternion in array)
			{
				if (fsmQuaternion.Name == name)
				{
					return fsmQuaternion;
				}
			}
			return null;
		}

		public FsmColor FindFsmColor(string name)
		{
			FsmColor[] array = ColorVariables;
			foreach (FsmColor fsmColor in array)
			{
				if (fsmColor.Name == name)
				{
					return fsmColor;
				}
			}
			return null;
		}

		public FsmGameObject FindFsmGameObject(string name)
		{
			FsmGameObject[] array = GameObjectVariables;
			foreach (FsmGameObject fsmGameObject in array)
			{
				if (fsmGameObject.Name == name)
				{
					return fsmGameObject;
				}
			}
			return null;
		}

		public FsmEnum FindFsmEnum(string name)
		{
			FsmEnum[] array = EnumVariables;
			foreach (FsmEnum fsmEnum in array)
			{
				if (fsmEnum.Name == name)
				{
					return fsmEnum;
				}
			}
			return null;
		}

		public FsmArray FindFsmArray(string name)
		{
			FsmArray[] array = ArrayVariables;
			foreach (FsmArray fsmArray in array)
			{
				if (fsmArray.Name == name)
				{
					return fsmArray;
				}
			}
			return null;
		}
	}
}
