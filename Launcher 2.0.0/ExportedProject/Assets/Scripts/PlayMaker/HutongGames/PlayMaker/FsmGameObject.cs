using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmGameObject : NamedVariable
	{
		[SerializeField]
		private GameObject value;

		public GameObject Value
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
				this.value = value as GameObject;
			}
		}

		public override VariableType VariableType
		{
			get
			{
				return VariableType.GameObject;
			}
		}

		public override void SafeAssign(object val)
		{
			value = val as GameObject;
		}

		public FsmGameObject()
		{
		}

		public FsmGameObject(string name)
			: base(name)
		{
		}

		public FsmGameObject(FsmGameObject source)
			: base(source)
		{
			if (source != null)
			{
				value = source.value;
			}
		}

		public override NamedVariable Clone()
		{
			return new FsmGameObject(this);
		}

		public override string ToString()
		{
			if (!(value == null))
			{
				return value.name;
			}
			return "None";
		}

		public static implicit operator FsmGameObject(GameObject value)
		{
			FsmGameObject fsmGameObject = new FsmGameObject(string.Empty);
			fsmGameObject.value = value;
			return fsmGameObject;
		}
	}
}
