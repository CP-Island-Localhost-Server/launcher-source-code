using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression
{
	internal static class Utility
	{
		public static VariableType GetDominantType(VariableType left, VariableType right)
		{
			switch (left)
			{
			case VariableType.Bool:
				switch (right)
				{
				case VariableType.Bool:
				case VariableType.GameObject:
				case VariableType.String:
				case VariableType.Material:
				case VariableType.Texture:
				case VariableType.Object:
					return VariableType.Bool;
				case VariableType.Float:
				case VariableType.Int:
					return right;
				}
				break;
			case VariableType.Float:
				switch (right)
				{
				case VariableType.Float:
				case VariableType.Int:
				case VariableType.Bool:
				case VariableType.GameObject:
				case VariableType.String:
				case VariableType.Material:
				case VariableType.Texture:
				case VariableType.Object:
					return VariableType.Float;
				}
				break;
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				switch (right)
				{
				case VariableType.GameObject:
				case VariableType.Material:
				case VariableType.Texture:
				case VariableType.Object:
					return VariableType.Object;
				case VariableType.Float:
				case VariableType.Int:
				case VariableType.Bool:
					return right;
				case VariableType.String:
					return VariableType.Bool;
				}
				break;
			case VariableType.Int:
				switch (right)
				{
				case VariableType.Int:
				case VariableType.Bool:
				case VariableType.GameObject:
				case VariableType.String:
				case VariableType.Material:
				case VariableType.Texture:
				case VariableType.Object:
					return VariableType.Int;
				case VariableType.Float:
					return VariableType.Float;
				}
				break;
			case VariableType.Vector2:
			case VariableType.Vector3:
			case VariableType.Color:
			case VariableType.Rect:
			case VariableType.Quaternion:
				if (right == left)
				{
					return left;
				}
				break;
			case VariableType.String:
				switch (right)
				{
				case VariableType.String:
					return VariableType.String;
				case VariableType.Float:
				case VariableType.Int:
				case VariableType.Bool:
					return right;
				case VariableType.GameObject:
				case VariableType.Material:
				case VariableType.Texture:
				case VariableType.Object:
					return VariableType.Bool;
				}
				break;
			}
			throw new IncompatibleTypeException(left, right);
		}
	}
}
