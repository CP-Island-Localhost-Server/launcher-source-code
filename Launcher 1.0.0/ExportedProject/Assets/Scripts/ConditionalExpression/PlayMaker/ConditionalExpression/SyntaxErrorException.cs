using System;

namespace PlayMaker.ConditionalExpression
{
	public class SyntaxErrorException : Exception
	{
		public SyntaxErrorException(string message)
			: base(message)
		{
		}
	}
}
