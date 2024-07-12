using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class BoolNode : Node
	{
		public bool Value { get; private set; }

		public override VariableType Type
		{
			get
			{
				return VariableType.Bool;
			}
		}

		public BoolNode(bool value)
		{
			Value = value;
		}

		public override bool ToBoolean()
		{
			return Value;
		}

		public override float ToFloat()
		{
			if (!Value)
			{
				return 0f;
			}
			return 1f;
		}

		public override int ToInt()
		{
			if (!Value)
			{
				return 0;
			}
			return 1;
		}

		public override string ToString()
		{
			if (!Value)
			{
				return "";
			}
			return "1";
		}
	}
}
