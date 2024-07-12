using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmMaterial : FsmObject
	{
		public new Material Value
		{
			get
			{
				return base.Value as Material;
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
				return VariableType.Material;
			}
		}

		public FsmMaterial()
		{
		}

		public FsmMaterial(string name)
			: base(name)
		{
		}

		public FsmMaterial(FsmObject source)
			: base(source)
		{
		}

		public override NamedVariable Clone()
		{
			return new FsmMaterial(this);
		}

		public override bool TestTypeConstraint(VariableType variableType, Type _objectType = null)
		{
			if (variableType == VariableType.Unknown)
			{
				return true;
			}
			return variableType == VariableType.Material;
		}
	}
}
