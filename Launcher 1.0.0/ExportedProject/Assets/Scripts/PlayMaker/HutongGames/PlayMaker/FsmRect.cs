using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmRect : NamedVariable
	{
		[SerializeField]
		private Rect value;

		public Rect Value
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
				this.value = (Rect)value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Rect;
			}
		}

		public FsmRect()
		{
		}

		public FsmRect(string name)
			: base(name)
		{
		}

		public FsmRect(FsmRect source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmRect(this);
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator FsmRect(Rect value)
		{
			FsmRect fsmRect = new FsmRect(string.Empty);
			fsmRect.value = value;
			return fsmRect;
		}
	}
}
