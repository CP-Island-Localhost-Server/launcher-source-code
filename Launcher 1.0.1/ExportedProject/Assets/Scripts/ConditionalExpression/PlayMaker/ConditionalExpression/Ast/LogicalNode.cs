using HutongGames.PlayMaker;
using UnityEngine;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class LogicalNode : Node
	{
		public Node Inner { get; private set; }

		public override VariableType Type
		{
			get
			{
				return Inner.Type;
			}
		}

		public LogicalNode(Node inner)
		{
			Inner = inner;
		}

		public override bool ToBoolean()
		{
			return Inner.ToBoolean();
		}

		public override float ToFloat()
		{
			return Inner.ToFloat();
		}

		public override int ToInt()
		{
			return Inner.ToInt();
		}

		public override Object ToObject()
		{
			return Inner.ToObject();
		}

		public override Color ToColor()
		{
			return Inner.ToColor();
		}

		public override Quaternion ToQuaternion()
		{
			return Inner.ToQuaternion();
		}

		public override Rect ToRect()
		{
			return Inner.ToRect();
		}

		public override Vector2 ToVector2()
		{
			return Inner.ToVector2();
		}

		public override Vector3 ToVector3()
		{
			return Inner.ToVector3();
		}

		public override string ToString()
		{
			return Inner.ToString();
		}
	}
}
