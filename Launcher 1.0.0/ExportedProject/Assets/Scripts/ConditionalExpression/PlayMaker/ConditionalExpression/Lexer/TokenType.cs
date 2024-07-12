namespace PlayMaker.ConditionalExpression.Lexer
{
	public enum TokenType
	{
		Whitespace = 0,
		Symbol = 1,
		KeywordAnd = 2,
		KeywordOr = 3,
		CompareEqual = 4,
		CompareNotEqual = 5,
		CompareGreater = 6,
		CompareLess = 7,
		CompareGreaterOrEqual = 8,
		CompareLessOrEqual = 9,
		Operator = 10,
		KeywordTrue = 11,
		KeywordFalse = 12,
		Identifier = 13,
		Literal = 14,
		NumericFloat = 15,
		NumericInteger = 16
	}
}
