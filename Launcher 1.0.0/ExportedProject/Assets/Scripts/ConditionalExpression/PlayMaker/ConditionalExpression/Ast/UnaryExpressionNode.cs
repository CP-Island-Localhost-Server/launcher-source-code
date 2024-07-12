using HutongGames.PlayMaker;
using UnityEngine;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class UnaryExpressionNode : ExpressionNode
	{
		public UnaryOperator Operator { get; private set; }

		public override VariableType Type
		{
			get
			{
				if (Operator != UnaryOperator.Negate)
				{
					return base.Inner.Type;
				}
				return VariableType.Bool;
			}
		}

		public UnaryExpressionNode(UnaryOperator op, Node inner)
			: base(inner)
		{
			Operator = op;
		}

		public override bool ToBoolean()
		{
			int num = base.Inner.ToInt();
			switch (Operator)
			{
			case UnaryOperator.Minus:
				return num != 0;
			case UnaryOperator.Negate:
				return num == 0;
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override float ToFloat()
		{
			float num = base.Inner.ToFloat();
			switch (Operator)
			{
			case UnaryOperator.Minus:
				return 0f - num;
			case UnaryOperator.Negate:
				if (num != 0f)
				{
					return 0f;
				}
				return 1f;
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override int ToInt()
		{
			int num = base.Inner.ToInt();
			switch (Operator)
			{
			case UnaryOperator.Minus:
				return -num;
			case UnaryOperator.Negate:
				if (num != 0)
				{
					return 0;
				}
				return 1;
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override Object ToObject()
		{
			throw new InvalidOperatorException(Operator, VariableType.Object);
		}

		public override Color ToColor()
		{
			throw new InvalidOperatorException(Operator, VariableType.Color);
		}

		public override Quaternion ToQuaternion()
		{
			throw new InvalidOperatorException(Operator, VariableType.Quaternion);
		}

		public override Rect ToRect()
		{
			throw new InvalidOperatorException(Operator, VariableType.Quaternion);
		}

		public override Vector2 ToVector2()
		{
			Vector2 vector = base.Inner.ToVector2();
			if (Operator == UnaryOperator.Minus)
			{
				return new Vector2(0f - vector.x, 0f - vector.y);
			}
			throw new InvalidOperatorException(Operator);
		}

		public override Vector3 ToVector3()
		{
			Vector3 vector = base.Inner.ToVector3();
			if (Operator == UnaryOperator.Minus)
			{
				return new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z);
			}
			throw new InvalidOperatorException(Operator);
		}

		public override string ToString()
		{
			throw new InvalidOperatorException(Operator, VariableType.String);
		}
	}
}
