using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FunctionCall
	{
		public string FunctionName = "";

		[SerializeField]
		private string parameterType;

		public FsmBool BoolParameter;

		public FsmFloat FloatParameter;

		public FsmInt IntParameter;

		public FsmGameObject GameObjectParameter;

		public FsmObject ObjectParameter;

		public FsmString StringParameter;

		public FsmVector2 Vector2Parameter;

		public FsmVector3 Vector3Parameter;

		public FsmRect RectParamater;

		public FsmQuaternion QuaternionParameter;

		public FsmMaterial MaterialParameter;

		public FsmTexture TextureParameter;

		public FsmColor ColorParameter;

		public FsmEnum EnumParameter;

		public FsmArray ArrayParameter;

		public string ParameterType
		{
			get
			{
				return parameterType;
			}
			set
			{
				parameterType = value;
			}
		}

		public FunctionCall()
		{
			ResetParameters();
		}

		public FunctionCall(FunctionCall source)
		{
			FunctionName = source.FunctionName;
			parameterType = source.parameterType;
			BoolParameter = new FsmBool(source.BoolParameter);
			FloatParameter = new FsmFloat(source.FloatParameter);
			IntParameter = new FsmInt(source.IntParameter);
			GameObjectParameter = new FsmGameObject(source.GameObjectParameter);
			ObjectParameter = source.ObjectParameter;
			StringParameter = new FsmString(source.StringParameter);
			Vector2Parameter = new FsmVector2(source.Vector2Parameter);
			Vector3Parameter = new FsmVector3(source.Vector3Parameter);
			RectParamater = new FsmRect(source.RectParamater);
			QuaternionParameter = new FsmQuaternion(source.QuaternionParameter);
			MaterialParameter = new FsmMaterial(source.MaterialParameter);
			TextureParameter = new FsmTexture(source.TextureParameter);
			ColorParameter = new FsmColor(source.ColorParameter);
			EnumParameter = new FsmEnum(source.EnumParameter);
			ArrayParameter = new FsmArray(source.ArrayParameter);
		}

		public void ResetParameters()
		{
			BoolParameter = false;
			FloatParameter = 0f;
			IntParameter = 0;
			StringParameter = "";
			GameObjectParameter = new FsmGameObject("");
			ObjectParameter = null;
			Vector2Parameter = new FsmVector2();
			Vector3Parameter = new FsmVector3();
			RectParamater = new FsmRect();
			QuaternionParameter = new FsmQuaternion();
			MaterialParameter = new FsmMaterial();
			TextureParameter = new FsmTexture();
			ColorParameter = new FsmColor();
			EnumParameter = new FsmEnum();
			ArrayParameter = new FsmArray();
		}
	}
}
