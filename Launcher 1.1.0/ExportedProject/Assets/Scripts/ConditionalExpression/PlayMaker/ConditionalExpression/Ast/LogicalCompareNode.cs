using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class LogicalCompareNode : LogicalNode
	{
		public ComparisonOperator Operator { get; private set; }

		public Node Right { get; private set; }

		public override VariableType Type
		{
			get
			{
				return Utility.GetDominantType(Left.Type, Right.Type);
			}
		}

		public Node Left
		{
			get
			{
				return base.Inner;
			}
		}

		public LogicalCompareNode(ComparisonOperator op, Node left, Node right)
			: base(left)
		{
			Operator = op;
			Right = right;
		}

		public override bool ToBoolean()
		{
			switch (Type)
			{
			case VariableType.Object:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToObject() == Right.ToObject();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToObject() != Right.ToObject();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Object);
				}
			case VariableType.Bool:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToBoolean() == Right.ToBoolean();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToBoolean() != Right.ToBoolean();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Bool);
				}
			case VariableType.Float:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToFloat() == Right.ToFloat();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToFloat() != Right.ToFloat();
				case ComparisonOperator.CompareGreater:
					return Left.ToFloat() > Right.ToFloat();
				case ComparisonOperator.CompareGreaterOrEqual:
					return Left.ToFloat() >= Right.ToFloat();
				case ComparisonOperator.CompareLess:
					return Left.ToFloat() < Right.ToFloat();
				case ComparisonOperator.CompareLessOrEqual:
					return Left.ToFloat() <= Right.ToFloat();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Float);
				}
			case VariableType.Int:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToInt() == Right.ToInt();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToInt() != Right.ToInt();
				case ComparisonOperator.CompareGreater:
					return Left.ToInt() > Right.ToInt();
				case ComparisonOperator.CompareGreaterOrEqual:
					return Left.ToInt() >= Right.ToInt();
				case ComparisonOperator.CompareLess:
					return Left.ToInt() < Right.ToInt();
				case ComparisonOperator.CompareLessOrEqual:
					return Left.ToInt() <= Right.ToInt();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Int);
				}
			case VariableType.String:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToString() == Right.ToString();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToString() != Right.ToString();
				default:
					throw new InvalidOperatorException(Operator, VariableType.String);
				}
			case VariableType.Color:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToColor() == Right.ToColor();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToColor() != Right.ToColor();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Color);
				}
			case VariableType.Quaternion:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToQuaternion() == Right.ToQuaternion();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToQuaternion() != Right.ToQuaternion();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Quaternion);
				}
			case VariableType.Rect:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToRect() == Right.ToRect();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToRect() != Right.ToRect();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Rect);
				}
			case VariableType.Vector2:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToVector2() == Right.ToVector2();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToVector2() != Right.ToVector2();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Vector2);
				}
			case VariableType.Vector3:
				switch (Operator)
				{
				case ComparisonOperator.CompareEqual:
					return Left.ToVector3() == Right.ToVector3();
				case ComparisonOperator.CompareNotEqual:
					return Left.ToVector3() != Right.ToVector3();
				default:
					throw new InvalidOperatorException(Operator, VariableType.Vector3);
				}
			default:
				throw new InvalidOperatorException(Operator);
			}
		}
	}
}
