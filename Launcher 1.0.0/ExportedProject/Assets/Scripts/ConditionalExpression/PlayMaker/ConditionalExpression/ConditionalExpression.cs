using System;
using HutongGames.PlayMaker;

namespace PlayMaker.ConditionalExpression
{
	[Tooltip("Evaluates a conditional expression and stores the result and/or sends an event upon evaluating true or false.\nSpecial thanks to Lea Hayes!")]
	[ActionCategory(ActionCategory.Logic)]
	public class ConditionalExpression : FsmStateAction, IEvaluatorContext
	{
		[UIHint(UIHint.TextArea)]
		[Tooltip("Enter an expression to evaluate.\nExample: (a < b) && c\n$(variable name with spaces)")]
		public FsmString expression;

		[Tooltip("Event to send if the result is True.")]
		public FsmEvent isTrueEvent;

		[Tooltip("Event to send if the result is False.")]
		public FsmEvent isFalseEvent;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		[NonSerialized]
		public bool rawResult;

		[Tooltip("Store the result in a Bool Variable.")]
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;

		private string cachedExpression;

		public CompiledAst Ast { get; set; }

		public string LastErrorMessage { get; set; }

		FsmVar IEvaluatorContext.GetVariable(string name)
		{
			NamedVariable variable = base.Fsm.Variables.GetVariable(name);
			if (variable != null)
			{
				return new FsmVar(variable);
			}
			throw new VariableNotFoundException(name);
		}

		public override void Reset()
		{
			expression = null;
			storeResult = null;
			rawResult = false;
			Ast = null;
			isTrueEvent = null;
			isFalseEvent = null;
			everyFrame = false;
		}

		public override void Awake()
		{
			try
			{
				LastErrorMessage = "";
				CompileExpressionIfNeeded();
			}
			catch (Exception ex)
			{
				LastErrorMessage = ex.Message;
			}
		}

		public override void OnEnter()
		{
			DoAction();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoAction();
		}

		private void DoAction()
		{
			if (expression == null || string.IsNullOrEmpty(expression.Value))
			{
				return;
			}
			try
			{
				LastErrorMessage = "";
				CompileExpressionIfNeeded();
				storeResult.Value = (rawResult = Ast.Evaluate());
				base.Fsm.Event(rawResult ? isTrueEvent : isFalseEvent);
			}
			catch (Exception ex)
			{
				LastErrorMessage = ex.Message;
			}
		}

		private void CompileExpressionIfNeeded()
		{
			if (Ast == null || cachedExpression != expression.Value)
			{
				Ast = CompiledAst.Compile(expression.Value, this);
				cachedExpression = expression.Value;
			}
		}
	}
}
