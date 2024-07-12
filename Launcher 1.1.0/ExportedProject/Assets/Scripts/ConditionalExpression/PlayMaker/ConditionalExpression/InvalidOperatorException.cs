using System;
using HutongGames.PlayMaker;
using PlayMaker.ConditionalExpression.Ast;

namespace PlayMaker.ConditionalExpression
{
	public class InvalidOperatorException : Exception
	{
		public InvalidOperatorException(BinaryOperator op)
			: base(string.Format("Invalid binary operator '{0}'.", new object[1] { op }))
		{
		}

		public InvalidOperatorException(BinaryOperator op, VariableType type)
			: base(string.Format("Binary operator '{0}' cannot be used with operand '{1}'.", new object[2] { op, type }))
		{
		}

		public InvalidOperatorException(ComparisonOperator op)
			: base(string.Format("Invalid comparison operator '{0}'.", new object[1] { op }))
		{
		}

		public InvalidOperatorException(ComparisonOperator op, VariableType type)
			: base(string.Format("Comparison operator '{0}' cannot be used with operand '{1}'.", new object[2] { op, type }))
		{
		}

		public InvalidOperatorException(LogicalOperator op)
			: base(string.Format("Invalid logical operator '{0}'.", new object[1] { op }))
		{
		}

		public InvalidOperatorException(UnaryOperator op)
			: base(string.Format("Invalid unary operator '{0}'.", new object[1] { op }))
		{
		}

		public InvalidOperatorException(UnaryOperator op, VariableType type)
			: base(string.Format("Unary operator '{0}' cannot be used with operand '{1}'.", new object[2] { op, type }))
		{
		}
	}
}
