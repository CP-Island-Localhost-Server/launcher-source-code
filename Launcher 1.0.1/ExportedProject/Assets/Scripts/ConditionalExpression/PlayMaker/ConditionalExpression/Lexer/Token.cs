namespace PlayMaker.ConditionalExpression.Lexer
{
	public struct Token
	{
		public static readonly Token none = new Token(TokenType.Whitespace, "");

		public TokenType type;

		public string content;

		public Token(TokenType type, string content)
		{
			this.type = type;
			this.content = content;
		}
	}
}
