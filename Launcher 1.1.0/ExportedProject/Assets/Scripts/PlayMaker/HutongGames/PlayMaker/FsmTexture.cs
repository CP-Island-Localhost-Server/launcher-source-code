using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmTexture : FsmObject
	{
		public new Texture Value
		{
			get
			{
				return base.Value as Texture;
			}
			set
			{
				base.Value = value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Texture;
			}
		}

		public FsmTexture()
		{
		}

		public FsmTexture(string name)
			: base(name)
		{
		}

		public FsmTexture(FsmObject source)
			: base(source)
		{
		}

		public override NamedVariable Clone()
		{
			return new FsmTexture(this);
		}

		public override bool TestTypeConstraint(VariableType variableType, Type _objectType = null)
		{
			if (variableType == VariableType.Unknown)
			{
				return true;
			}
			return variableType == VariableType.Texture;
		}
	}
}
