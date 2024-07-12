using PlayMaker.ConditionalExpression.Ast;
using PlayMaker.ConditionalExpression.Lexer;

namespace PlayMaker.ConditionalExpression
{
	public sealed class Parser
	{
		public static IEvaluatorContext EvaluatorContext { get; internal set; }

		private string Expression { get; set; }

		private Tokenizer Tokenizer { get; set; }

		public Parser(string expression)
		{
			Expression = expression;
			Tokenizer = new Tokenizer(expression);
		}

		internal Node Parse()
		{
			return or_expr(Tokenizer.Next());
		}

		private Node or_expr(Token token)
		{
			Node node = and_expr(token);
			if (Tokenizer.HasFinished)
			{
				return node;
			}
			return or_expr_rest(node);
		}

		private Node or_expr_rest(Node exp)
		{
			if (Tokenizer.Peek().type == TokenType.KeywordOr)
			{
				Tokenizer.Next();
				Node right = and_expr(Tokenizer.Next());
				exp = new LogicalExpressionNode(LogicalOperator.Or, exp, right);
				if (Tokenizer.HasFinished)
				{
					return exp;
				}
				return or_expr_rest(exp);
			}
			return exp;
		}

		private Node and_expr(Token token)
		{
			Node node = compare_expr(token);
			if (Tokenizer.HasFinished)
			{
				return node;
			}
			return and_expr_rest(node);
		}

		private Node and_expr_rest(Node exp)
		{
			if (Tokenizer.Peek().type == TokenType.KeywordAnd)
			{
				Tokenizer.Next();
				Node right = compare_expr(Tokenizer.Next());
				exp = new LogicalExpressionNode(LogicalOperator.And, exp, right);
				if (Tokenizer.HasFinished)
				{
					return exp;
				}
				return and_expr_rest(exp);
			}
			return exp;
		}

		private Node compare_expr(Token token)
		{
			Node node = expr(token);
			if (Tokenizer.HasFinished)
			{
				return node;
			}
			return compare_expr_rest(node);
		}

		private Node compare_expr_rest(Node exp)
		{
			Token token = Tokenizer.Peek();
			if (token.type >= TokenType.CompareEqual && token.type <= TokenType.CompareLessOrEqual)
			{
				Tokenizer.Next();
				ComparisonOperator op = (ComparisonOperator)(token.type - 4);
				Node right = expr(Tokenizer.Next());
				exp = new LogicalCompareNode(op, exp, right);
				if (Tokenizer.HasFinished)
				{
					return exp;
				}
				return compare_expr_rest(exp);
			}
			return exp;
		}

		private Node expr(Token token)
		{
			Node node = expr_term(token);
			if (Tokenizer.HasFinished)
			{
				return node;
			}
			return expr_rest(node);
		}

		private Node expr_rest(Node term)
		{
			Token token = Tokenizer.Peek();
			if (token.content == "+" || token.content == "-")
			{
				Tokenizer.Next();
				BinaryOperator op = ((!(token.content == "+")) ? BinaryOperator.Subtract : BinaryOperator.Add);
				Node right = expr_term(Tokenizer.Next());
				term = new BinaryExpressionNode(op, term, right);
				if (Tokenizer.HasFinished)
				{
					return term;
				}
				return expr_rest(term);
			}
			return term;
		}

		private Node expr_term(Token token)
		{
			Node node = expr_value(token);
			if (Tokenizer.HasFinished)
			{
				return node;
			}
			return expr_term_rest(node);
		}

		private Node expr_term_rest(Node term)
		{
			Token token = Tokenizer.Peek();
			if (token.content == "*" || token.content == "/" || token.content == "%")
			{
				Tokenizer.Next();
				BinaryOperator op = ((token.content == "*") ? BinaryOperator.Multiply : ((token.content == "/") ? BinaryOperator.Divide : BinaryOperator.Modulo));
				Node right = expr_value(Tokenizer.Next());
				term = new BinaryExpressionNode(op, term, right);
				if (Tokenizer.HasFinished)
				{
					return term;
				}
				return expr_term_rest(term);
			}
			return term;
		}

		private Node expr_value(Token token)
		{
			switch (token.type)
			{
			case TokenType.Symbol:
				if (token.content == "(")
				{
					Node inner = or_expr(Tokenizer.Next());
					token = Tokenizer.Next();
					if (token.type != TokenType.Symbol || token.content != ")")
					{
						throw new SyntaxErrorException(string.Format("Found '{0}' but expected ')'.", new object[1] { token.content }));
					}
					return new ExpressionNode(inner);
				}
				break;
			case TokenType.Operator:
				if (token.content == "-")
				{
					return new UnaryExpressionNode(UnaryOperator.Minus, expr_value(Tokenizer.Next()));
				}
				if (token.content == "!")
				{
					return new UnaryExpressionNode(UnaryOperator.Negate, expr_value(Tokenizer.Next()));
				}
				if (token.content == "+")
				{
					return expr_value(Tokenizer.Next());
				}
				break;
			case TokenType.Identifier:
				return new ExpressionNode(value_identifier(token));
			case TokenType.KeywordTrue:
			case TokenType.KeywordFalse:
				return new ExpressionNode(value_boolean(token));
			case TokenType.NumericInteger:
				return new ExpressionNode(value_integer(token));
			case TokenType.NumericFloat:
				return new ExpressionNode(value_float(token));
			case TokenType.Literal:
				return new ExpressionNode(value_literal(token));
			}
			throw new SyntaxErrorException(string.Format("Expected value but found '{0}'.", new object[1] { token.content }));
		}

		private VarNode value_identifier(Token token)
		{
			if (token.content[0] == '$')
			{
				return new VarNode(token.content.Substring(2, token.content.Length - 3));
			}
			return new VarNode(token.content);
		}

		private BoolNode value_boolean(Token token)
		{
			return new BoolNode(token.type == TokenType.KeywordTrue);
		}

		private IntNode value_integer(Token token)
		{
			return new IntNode(int.Parse(token.content));
		}

		private FloatNode value_float(Token token)
		{
			return new FloatNode(float.Parse(token.content));
		}

		private StringNode value_literal(Token token)
		{
			return new StringNode(token.content.Substring(1, token.content.Length - 2));
		}
	}
}
