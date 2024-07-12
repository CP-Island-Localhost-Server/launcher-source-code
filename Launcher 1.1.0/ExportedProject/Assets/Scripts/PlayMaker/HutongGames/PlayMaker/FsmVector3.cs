using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmVector3 : NamedVariable
	{
		[SerializeField]
		private Vector3 value;

		public Vector3 Value
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
				this.value = (Vector3)value;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.Vector3;
			}
		}

		public FsmVector3()
		{
		}

		public FsmVector3(string name)
			: base(name)
		{
		}

		public FsmVector3(FsmVector3 source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmVector3(this);
		}

		public override string ToString()
		{
			return value.ToString();
		}

		public static implicit operator FsmVector3(Vector3 value)
		{
			FsmVector3 fsmVector = new FsmVector3(string.Empty);
			fsmVector.value = value;
			return fsmVector;
		}
	}
}
