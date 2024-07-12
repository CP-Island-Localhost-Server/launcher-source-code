using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class FloatNode : Node
	{
		public float Value { get; private set; }

		public override VariableType Type
		{
			get
			{
				return VariableType.Float;
			}
		}

		public FloatNode(float value)
		{
			Value = value;
		}

		public override bool ToBoolean()
		{
			return Value != 0f;
		}

		public override float ToFloat()
		{
			return Value;
		}

		public override int ToInt()
		{
			return (int)Value;
		}

		public override string ToString()
		{
			if (Value == 0f)
			{
				return "";
			}
			return "1";
		}
	}
}
