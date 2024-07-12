using HutongGames.PlayMaker;
using UnityEngine;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class BinaryExpressionNode : ExpressionNode
	{
		public BinaryOperator Operator { get; private set; }

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

		public BinaryExpressionNode(BinaryOperator op, Node left, Node right)
			: base(left)
		{
			Operator = op;
			Right = right;
		}

		public override bool ToBoolean()
		{
			int num = Left.ToInt();
			int num2 = Right.ToInt();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return num + num2 != 0;
			case BinaryOperator.Subtract:
				return num - num2 != 0;
			case BinaryOperator.Multiply:
				return num * num2 != 0;
			case BinaryOperator.Divide:
				return num / num2 != 0;
			case BinaryOperator.Modulo:
				return num % num2 != 0;
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override float ToFloat()
		{
			float num = Left.ToFloat();
			float num2 = Right.ToFloat();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return num + num2;
			case BinaryOperator.Subtract:
				return num - num2;
			case BinaryOperator.Multiply:
				return num * num2;
			case BinaryOperator.Divide:
				return num / num2;
			case BinaryOperator.Modulo:
				return num % num2;
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override int ToInt()
		{
			int num = Left.ToInt();
			int num2 = Right.ToInt();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return num + num2;
			case BinaryOperator.Subtract:
				return num - num2;
			case BinaryOperator.Multiply:
				return num * num2;
			case BinaryOperator.Divide:
				return num / num2;
			case BinaryOperator.Modulo:
				return num % num2;
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
			Color color = Left.ToColor();
			Color color2 = Right.ToColor();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return color + color2;
			case BinaryOperator.Subtract:
				return color - color2;
			case BinaryOperator.Multiply:
				return color * color2;
			case BinaryOperator.Divide:
			case BinaryOperator.Modulo:
				throw new InvalidOperatorException(Operator, VariableType.Color);
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override Quaternion ToQuaternion()
		{
			Quaternion quaternion = Left.ToQuaternion();
			Quaternion quaternion2 = Right.ToQuaternion();
			switch (Operator)
			{
			case BinaryOperator.Multiply:
				return quaternion * quaternion2;
			case BinaryOperator.Add:
			case BinaryOperator.Subtract:
			case BinaryOperator.Divide:
			case BinaryOperator.Modulo:
				throw new InvalidOperatorException(Operator, VariableType.Quaternion);
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override Rect ToRect()
		{
			Rect rect = Left.ToRect();
			Rect rect2 = Right.ToRect();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return new Rect(rect.x + rect2.x, rect.y + rect2.y, rect.width + rect2.width, rect.height + rect2.height);
			case BinaryOperator.Subtract:
				return new Rect(rect.x - rect2.x, rect.y - rect2.y, rect.width - rect2.width, rect.height - rect2.height);
			case BinaryOperator.Multiply:
				return new Rect(rect.x * rect2.x, rect.y * rect2.y, rect.width * rect2.width, rect.height * rect2.height);
			case BinaryOperator.Divide:
				return new Rect(rect.x / rect2.x, rect.y / rect2.y, rect.width / rect2.width, rect.height / rect2.height);
			case BinaryOperator.Modulo:
				return new Rect(rect.x % rect2.x, rect.y % rect2.y, rect.width % rect2.width, rect.height % rect2.height);
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override Vector2 ToVector2()
		{
			Vector2 vector = Left.ToVector2();
			Vector2 vector2 = Right.ToVector2();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return new Vector2(vector.x + vector2.x, vector.y + vector2.y);
			case BinaryOperator.Subtract:
				return new Vector2(vector.x - vector2.x, vector.y - vector2.y);
			case BinaryOperator.Multiply:
				return new Vector2(vector.x * vector2.x, vector.y * vector2.y);
			case BinaryOperator.Divide:
				return new Vector2(vector.x / vector2.x, vector.y / vector2.y);
			case BinaryOperator.Modulo:
				return new Vector2(vector.x % vector2.x, vector.y % vector2.y);
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override Vector3 ToVector3()
		{
			Vector3 vector = Left.ToVector3();
			Vector3 vector2 = Right.ToVector3();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return new Vector3(vector.x + vector2.x, vector.y + vector2.y, vector.z + vector2.z);
			case BinaryOperator.Subtract:
				return new Vector3(vector.x - vector2.x, vector.y - vector2.y, vector.z - vector2.z);
			case BinaryOperator.Multiply:
				return new Vector3(vector.x * vector2.x, vector.y * vector2.y, vector.z * vector2.z);
			case BinaryOperator.Divide:
				return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
			case BinaryOperator.Modulo:
				return new Vector3(vector.x % vector2.x, vector.y % vector2.y, vector.z % vector2.z);
			default:
				throw new InvalidOperatorException(Operator);
			}
		}

		public override string ToString()
		{
			string text = Left.ToString();
			string text2 = Right.ToString();
			switch (Operator)
			{
			case BinaryOperator.Add:
				return text + text2;
			case BinaryOperator.Subtract:
			case BinaryOperator.Multiply:
			case BinaryOperator.Divide:
			case BinaryOperator.Modulo:
				throw new InvalidOperatorException(Operator, VariableType.Quaternion);
			default:
				throw new InvalidOperatorException(Operator);
			}
		}
	}
}
