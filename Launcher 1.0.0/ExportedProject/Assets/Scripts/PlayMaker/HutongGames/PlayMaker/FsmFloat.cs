using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmFloat : NamedVariable
	{
		[SerializeField]
		private float value;

		public float Value
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
				this.value = (float)value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Float;
			}
		}

		public override void SafeAssign(object val)
		{
			if (val is float)
			{
				value = (float)val;
			}
			if (val is int)
			{
				value = (int)val;
			}
		}

		public FsmFloat()
		{
		}

		public FsmFloat(string name)
			: base(name)
		{
		}

		public FsmFloat(FsmFloat source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmFloat(this);
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator FsmFloat(float value)
		{
			FsmFloat fsmFloat = new FsmFloat(string.Empty);
			fsmFloat.value = value;
			return fsmFloat;
		}
	}
}
