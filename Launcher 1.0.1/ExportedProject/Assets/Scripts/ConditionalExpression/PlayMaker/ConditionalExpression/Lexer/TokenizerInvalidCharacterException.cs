using System;

namespace PlayMaker.ConditionalExpression.Lexer
{
	public class TokenizerInvalidCharacterException : Exception
	{
		public TokenizerInvalidCharacterException(char character)
			: base(string.Format("Invalid character '{0}' encountered whilst attempting to parse expression.", new object[1] { character }))
		{
		}
	}
}
