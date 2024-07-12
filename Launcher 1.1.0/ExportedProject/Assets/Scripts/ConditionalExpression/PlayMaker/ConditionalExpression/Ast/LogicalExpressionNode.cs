using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class LogicalExpressionNode : LogicalNode
	{
		public LogicalOperator Operator { get; private set; }

		public Node Right { get; private set; }

		public override VariableType Type
		{
			get
			{
				return VariableType.Bool;
			}
		}

		public Node Left
		{
			get
			{
				return base.Inner;
			}
		}

		public LogicalExpressionNode(LogicalOperator op, Node left, Node right)
			: base(left)
		{
			Operator = op;
			Right = right;
		}

		public override bool ToBoolean()
		{
			bool flag = Left.ToBoolean();
			bool result = Right.ToBoolean();
			switch (Operator)
			{
			case LogicalOperator.And:
				if (flag)
				{
					return result;
				}
				return false;
			case LogicalOperator.Or:
				if (!flag)
				{
					return result;
				}
				return true;
			default:
				throw new InvalidOperatorException(Operator);
			}
		}
	}
}
