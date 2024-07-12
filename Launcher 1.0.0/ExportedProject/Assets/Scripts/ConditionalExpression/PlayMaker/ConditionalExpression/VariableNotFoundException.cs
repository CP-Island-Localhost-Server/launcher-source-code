using System;

namespace PlayMaker.ConditionalExpression
{
	public class VariableNotFoundException : Exception
	{
		public VariableNotFoundException(string variableName)
			: base(string.Format("Variable was not found '{0}'.", new object[1] { variableName }))
		{
		}
	}
}
