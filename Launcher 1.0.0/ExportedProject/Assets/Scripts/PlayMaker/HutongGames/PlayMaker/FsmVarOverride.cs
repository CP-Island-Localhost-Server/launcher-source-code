using System;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmVarOverride
	{
		public NamedVariable variable;

		public FsmVar fsmVar;

		public bool isEdited;

		public FsmVarOverride(FsmVarOverride source)
		{
			variable = new NamedVariable(source.variable.Name);
			fsmVar = new FsmVar(source.fsmVar);
			isEdited = source.isEdited;
		}

		public FsmVarOverride(NamedVariable namedVar)
		{
			variable = namedVar;
			fsmVar = new FsmVar(variable);
			isEdited = false;
		}

		public void Apply(FsmVariables variables)
		{
			variable = variables.GetVariable(variable.Name);
			fsmVar.ApplyValueTo(variable);
		}
	}
}
