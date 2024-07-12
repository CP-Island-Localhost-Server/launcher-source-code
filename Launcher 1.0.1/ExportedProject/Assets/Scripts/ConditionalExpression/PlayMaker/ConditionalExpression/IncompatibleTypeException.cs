using System;
using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression
{
	public class IncompatibleTypeException : Exception
	{
		public IncompatibleTypeException(VariableType left, VariableType right)
			: base(string.Format("Unable to convert between types '{0}' and '{1}'.", new object[2] { left, right }))
		{
		}
	}
}
