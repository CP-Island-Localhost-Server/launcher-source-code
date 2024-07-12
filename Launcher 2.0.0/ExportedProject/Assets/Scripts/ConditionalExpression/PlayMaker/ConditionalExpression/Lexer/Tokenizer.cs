using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PlayMaker.ConditionalExpression.Lexer
{
	public class Tokenizer
	{
		private static Dictionary<TokenType, Regex> _tokenDictionary;

		private List<Token> _tokens;

		private int _position = -1;

		private static Dictionary<TokenType, Regex> TokenDictionary
		{
			get
			{
				if (_tokenDictionary == null)
				{
					InitTokenDictionary();
				}
				return _tokenDictionary;
			}
		}

		public ReadOnlyCollection<Token> Tokens { get; private set; }

		public bool HasFinished
		{
			get
			{
				return _position + 1 >= _tokens.Count;
			}
		}

		private static void AddToken(TokenType type, string regex)
		{
			_tokenDictionary[type] = new Regex("\\G(" + regex + ")", RegexOptions.IgnoreCase);
		}

		private static void InitTokenDictionary()
		{
			_tokenDictionary = new Dictionary<TokenType, Regex>();
			AddToken(TokenType.Whitespace, "\\s+");
			AddToken(TokenType.CompareEqual, "==");
			AddToken(TokenType.CompareNotEqual, "!=");
			AddToken(TokenType.CompareGreaterOrEqual, ">=");
			AddToken(TokenType.CompareLessOrEqual, "<=");
			AddToken(TokenType.CompareGreater, ">");
			AddToken(TokenType.CompareLess, "<");
			AddToken(TokenType.Operator, "\\+|\\-|\\*|\\/|\\%|!");
			AddToken(TokenType.Symbol, "[\\(\\)]");
			AddToken(TokenType.KeywordAnd, "and|&&");
			AddToken(TokenType.KeywordOr, "or|\\|\\|");
			AddToken(TokenType.KeywordTrue, "true|on|yes");
			AddToken(TokenType.KeywordFalse, "false|off|no|null");
			AddToken(TokenType.NumericFloat, "(([0-9]*\\.[0-9]+)|([0-9]+))([Ee][0-9]+)?f?");
			AddToken(TokenType.NumericInteger, "[0-9]+");
			AddToken(TokenType.Literal, "('([^'\\\\]|\\\\.)*\\')|(\"([^\"\\\\]|\\\\.)*\\\")");
			AddToken(TokenType.Identifier, "[a-z_\\-][0-9a-z_\\-\\$]*|\\$\\([^\\)\"']+\\)");
		}

		public Tokenizer(string expression)
		{
			_tokens = new List<Token>();
			Tokens = new ReadOnlyCollection<Token>(_tokens);
			Tokenize(expression);
		}

		private void Tokenize(string expression)
		{
			int num = 0;
			Token item = default(Token);
			while (num < expression.Length)
			{
				foreach (KeyValuePair<TokenType, Regex> item2 in TokenDictionary)
				{
					if (!item2.Value.IsMatch(expression, num))
					{
						continue;
					}
					item.type = item2.Key;
					item.content = item2.Value.Match(expression, num).Value;
					num += item.content.Length;
					if (item.type != 0)
					{
						_tokens.Add(item);
					}
					goto IL_00a8;
				}
				throw new TokenizerInvalidCharacterException(expression[num]);
				IL_00a8:;
			}
		}

		public Token Next(int count = 1)
		{
			if (_position + count >= _tokens.Count)
			{
				return Token.none;
			}
			return _tokens[_position += count];
		}

		public Token Peek(int ahead = 1)
		{
			if (_position + ahead >= _tokens.Count)
			{
				return Token.none;
			}
			return _tokens[_position + ahead];
		}
	}
}
