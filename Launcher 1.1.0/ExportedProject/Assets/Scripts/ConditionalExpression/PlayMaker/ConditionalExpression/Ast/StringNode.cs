using System.Text.RegularExpressions;
using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression.Ast
{
	internal class StringNode : Node
	{
		private static MatchEvaluator _unescapeLiteral = UnescapeLiteralEvaluator;

		public string Value { get; private set; }

		public override VariableType Type
		{
			get
			{
				return VariableType.String;
			}
		}

		private static string UnescapeLiteralEvaluator(Match match)
		{
			switch (match.Groups[1].Value)
			{
			case "'":
				return "'";
			case "\"":
				return "\"";
			case "\\n":
				return "\n";
			case "\\r":
				return "\r";
			case "\\t":
				return "\t";
			case "\\":
				return "\\";
			default:
				return match.Value;
			}
		}

		public StringNode(string value)
		{
			Value = Regex.Replace(value, "\\\\(.)", _unescapeLiteral);
		}

		public override bool ToBoolean()
		{
			return !string.IsNullOrEmpty(Value);
		}

		public override float ToFloat()
		{
			if (string.IsNullOrEmpty(Value))
			{
				return 0f;
			}
			return 1f;
		}

		public override int ToInt()
		{
			if (string.IsNullOrEmpty(Value))
			{
				return 0;
			}
			return 1;
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
