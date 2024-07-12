using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class NamedVariable : INameable, INamedVariable, IComparable
	{
		[SerializeField]
		private bool useVariable;

		[SerializeField]
		private string name;

		[SerializeField]
		private string tooltip = "";

		[SerializeField]
		private bool showInInspector;

		[SerializeField]
		private bool networkSync;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public virtual VariableType VariableType
		{
			get
			{
				throw new Exception("VariableType not implemented: " + GetType().FullName);
			}
		}

		public virtual Type ObjectType
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual VariableType TypeConstraint
		{
			get
			{
				return VariableType;
			}
		}

		public virtual object RawValue
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string Tooltip
		{
			get
			{
				return tooltip;
			}
			set
			{
				tooltip = value;
			}
		}

		public bool UseVariable
		{
			get
			{
				return useVariable;
			}
			set
			{
				useVariable = value;
			}
		}

		public bool ShowInInspector
		{
			get
			{
				return showInInspector;
			}
			set
			{
				showInInspector = value;
			}
		}

		public bool NetworkSync
		{
			get
			{
				return networkSync;
			}
			set
			{
				networkSync = value;
			}
		}

		public bool IsNone
		{
			get
			{
				if (useVariable)
				{
					return string.IsNullOrEmpty(name);
				}
				return false;
			}
		}

		public bool UsesVariable
		{
			get
			{
				if (useVariable)
				{
					return !string.IsNullOrEmpty(name);
				}
				return false;
			}
		}

		public static bool IsNullOrNone(NamedVariable variable)
		{
			if (variable != null)
			{
				return variable.IsNone;
			}
			return true;
		}

		public NamedVariable()
		{
			name = "";
			tooltip = "";
		}

		public NamedVariable(string name)
		{
			this.name = name;
			if (!string.IsNullOrEmpty(name))
			{
				useVariable = true;
			}
		}

		public NamedVariable(NamedVariable source)
		{
			if (source != null)
			{
				useVariable = source.useVariable;
				name = source.name;
				showInInspector = source.showInInspector;
				tooltip = source.tooltip;
				networkSync = source.networkSync;
			}
		}

		public virtual bool TestTypeConstraint(VariableType variableType, Type objectType = null)
		{
			if (variableType == VariableType.Unknown)
			{
				return true;
			}
			return TypeConstraint == variableType;
		}

		public virtual void SafeAssign(object val)
		{
			throw new NotImplementedException();
		}

		public virtual NamedVariable Clone()
		{
			throw new NotImplementedException();
		}

		public string GetDisplayName()
		{
			if (string.IsNullOrEmpty(Name))
			{
				return "None";
			}
			return Name;
		}

		public int CompareTo(object obj)
		{
			NamedVariable namedVariable = obj as NamedVariable;
			if (namedVariable == null)
			{
				return 0;
			}
			return string.CompareOrdinal(name, namedVariable.name);
		}
	}
}
