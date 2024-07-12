using HutongGames.PlayMaker;
using UnityEngine;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class VarNode : Node
	{
		public string Name { get; private set; }

		public override VariableType Type
		{
			get
			{
				return Parser.EvaluatorContext.GetVariable(Name).Type;
			}
		}

		public VarNode(string name)
		{
			Name = name;
		}

		public override bool ToBoolean()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			switch (Type)
			{
			case VariableType.Bool:
				return variable.boolValue;
			case VariableType.Float:
				return variable.floatValue != 0f;
			case VariableType.Int:
				return variable.intValue != 0;
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				return variable.objectReference != null;
			case VariableType.String:
				return !string.IsNullOrEmpty(variable.stringValue);
			default:
				return base.ToBoolean();
			}
		}

		public override float ToFloat()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			switch (Type)
			{
			case VariableType.Bool:
				if (!variable.boolValue)
				{
					return 0f;
				}
				return 1f;
			case VariableType.Float:
				return variable.floatValue;
			case VariableType.Int:
				return variable.intValue;
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				if (!(variable.objectReference != null))
				{
					return 0f;
				}
				return 1f;
			case VariableType.String:
				if (string.IsNullOrEmpty(variable.stringValue))
				{
					return 0f;
				}
				return 1f;
			default:
				return base.ToFloat();
			}
		}

		public override int ToInt()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			switch (Type)
			{
			case VariableType.Bool:
				if (!variable.boolValue)
				{
					return 0;
				}
				return 1;
			case VariableType.Float:
				return (int)variable.floatValue;
			case VariableType.Int:
				return variable.intValue;
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				if (!(variable.objectReference != null))
				{
					return 0;
				}
				return 1;
			case VariableType.String:
				if (string.IsNullOrEmpty(variable.stringValue))
				{
					return 0;
				}
				return 1;
			default:
				return base.ToInt();
			}
		}

		public override Object ToObject()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			switch (Type)
			{
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				return variable.objectReference;
			default:
				return base.ToObject();
			}
		}

		public override Color ToColor()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			if (Type == VariableType.Color)
			{
				return variable.colorValue;
			}
			return base.ToColor();
		}

		public override Quaternion ToQuaternion()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			if (Type == VariableType.Quaternion)
			{
				return variable.quaternionValue;
			}
			return base.ToQuaternion();
		}

		public override Rect ToRect()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			if (Type == VariableType.Rect)
			{
				return variable.rectValue;
			}
			return base.ToRect();
		}

		public override Vector2 ToVector2()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			if (Type == VariableType.Vector2)
			{
				return variable.vector2Value;
			}
			return base.ToVector2();
		}

		public override Vector3 ToVector3()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			if (Type == VariableType.Vector3)
			{
				return variable.vector3Value;
			}
			return base.ToVector3();
		}

		public override string ToString()
		{
			FsmVar variable = Parser.EvaluatorContext.GetVariable(Name);
			switch (Type)
			{
			case VariableType.Bool:
				if (!variable.boolValue)
				{
					return "";
				}
				return "1";
			case VariableType.Float:
				if (variable.floatValue == 0f)
				{
					return "";
				}
				return "1";
			case VariableType.Int:
				if (variable.intValue == 0)
				{
					return "";
				}
				return "1";
			case VariableType.GameObject:
			case VariableType.Material:
			case VariableType.Texture:
			case VariableType.Object:
				if (!(variable.objectReference != null))
				{
					return "";
				}
				return "1";
			case VariableType.String:
				return variable.stringValue;
			default:
				return base.ToString();
			}
		}
	}
}
