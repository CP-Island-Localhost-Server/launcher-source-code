using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmInt : NamedVariable
	{
		[SerializeField]
		private int value;

		public int Value
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
				this.value = (int)value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Int;
			}
		}

		public override void SafeAssign(object val)
		{
			if (val is int)
			{
				value = (int)val;
			}
			if (val is float)
			{
				value = Mathf.FloorToInt((float)val);
			}
		}

		public FsmInt()
		{
		}

		public FsmInt(string name)
			: base(name)
		{
		}

		public FsmInt(FsmInt source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmInt(this);
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator FsmInt(int value)
		{
			FsmInt fsmInt = new FsmInt(string.Empty);
			fsmInt.value = value;
			return fsmInt;
		}
	}
}
