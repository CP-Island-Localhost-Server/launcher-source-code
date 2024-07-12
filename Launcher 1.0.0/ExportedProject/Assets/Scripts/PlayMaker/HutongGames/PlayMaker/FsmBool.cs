using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmBool : NamedVariable
	{
		[SerializeField]
		private bool value;

		public bool Value
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
				this.value = (bool)value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Bool;
			}
		}

		public FsmBool()
		{
		}

		public FsmBool(string name)
			: base(name)
		{
		}

		public FsmBool(FsmBool source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmBool(this);
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator FsmBool(bool value)
		{
			FsmBool fsmBool = new FsmBool(string.Empty);
			fsmBool.value = value;
			return fsmBool;
		}
	}
}
