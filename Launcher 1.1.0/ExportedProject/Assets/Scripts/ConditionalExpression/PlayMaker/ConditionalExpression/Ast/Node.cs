using HutongGames.PlayMaker;
using UnityEngine;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal abstract class Node
	{
		public abstract VariableType Type { get; }

		public virtual bool ToBoolean()
		{
			throw new IncompatibleTypeException(Type, VariableType.Bool);
		}

		public virtual float ToFloat()
		{
			throw new IncompatibleTypeException(Type, VariableType.Float);
		}

		public virtual int ToInt()
		{
			throw new IncompatibleTypeException(Type, VariableType.Int);
		}

		public virtual Object ToObject()
		{
			throw new IncompatibleTypeException(Type, VariableType.Object);
		}

		public virtual Color ToColor()
		{
			throw new IncompatibleTypeException(Type, VariableType.Color);
		}

		public virtual Quaternion ToQuaternion()
		{
			throw new IncompatibleTypeException(Type, VariableType.Quaternion);
		}

		public virtual Rect ToRect()
		{
			throw new IncompatibleTypeException(Type, VariableType.Rect);
		}

		public virtual Vector2 ToVector2()
		{
			throw new IncompatibleTypeException(Type, VariableType.Vector2);
		}

		public virtual Vector3 ToVector3()
		{
			throw new IncompatibleTypeException(Type, VariableType.Vector3);
		}

		public override string ToString()
		{
			throw new IncompatibleTypeException(Type, VariableType.String);
		}
	}
}
