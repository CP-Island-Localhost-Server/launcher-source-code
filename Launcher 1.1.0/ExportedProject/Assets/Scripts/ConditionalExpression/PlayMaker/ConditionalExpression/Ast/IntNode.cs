using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class IntNode : Node
	{
		public int Value { get; private set; }

		public override VariableType Type
		{
			get
			{
				return VariableType.Int;
			}
		}

		public IntNode(int value)
		{
			Value = value;
		}

		public override bool ToBoolean()
		{
			return Value != 0;
		}

		public override float ToFloat()
		{
			return Value;
		}

		public override int ToInt()
		{
			return Value;
		}

		public override string ToString()
		{
			if (Value == 0)
			{
				return "";
			}
			return "1";
		}
	}
}
