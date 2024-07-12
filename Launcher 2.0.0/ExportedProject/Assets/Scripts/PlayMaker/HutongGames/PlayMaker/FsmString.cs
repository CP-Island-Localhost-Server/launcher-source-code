using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmString : NamedVariable
	{
		[SerializeField]
		private string value = "";

		public string Value
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
				this.value = (string)value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.String;
			}
		}

		public FsmString()
		{
		}

		public FsmString(string name)
			: base(name)
		{
		}

		public FsmString(FsmString source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmString(this);
		}

		public override string ToString()
		{
			return value;
		}

		public static implicit operator FsmString(string value)
		{
			FsmString fsmString = new FsmString(string.Empty);
			fsmString.value = value;
			return fsmString;
		}

		public static bool IsNullOrEmpty(FsmString fsmString)
		{
			if (fsmString == null)
			{
				return true;
			}
			if (fsmString.IsNone)
			{
				return true;
			}
			return string.IsNullOrEmpty(fsmString.value);
		}
	}
}
