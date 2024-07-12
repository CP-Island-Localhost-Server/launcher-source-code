using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression
{
	public interface IEvaluatorContext
	{
		FsmVar GetVariable(string name);
	}
}
