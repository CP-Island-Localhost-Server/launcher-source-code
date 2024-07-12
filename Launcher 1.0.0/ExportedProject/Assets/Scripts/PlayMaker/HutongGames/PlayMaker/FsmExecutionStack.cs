using System.Collections.Generic;

namespace HutongGames.PlayMaker
{
	public static class FsmExecutionStack
	{
		private static readonly Stack<Fsm> fsmExecutionStack = new Stack<Fsm>(256);

		public static Fsm ExecutingFsm
		{
			get
			{
				if (fsmExecutionStack.Count <= 0)
				{
					return null;
				}
				return fsmExecutionStack.Peek();
			}
		}

		public static FsmState ExecutingState
		{
			get
			{
				if (ExecutingFsm == null)
				{
					return null;
				}
				return ExecutingFsm.ActiveState;
			}
		}

		public static string ExecutingStateName
		{
			get
			{
				if (ExecutingFsm == null)
				{
					return "[none]";
				}
				return ExecutingFsm.ActiveStateName;
			}
		}

		public static FsmStateAction ExecutingAction
		{
			get
			{
				if (ExecutingState == null)
				{
					return null;
				}
				return ExecutingState.ActiveAction;
			}
		}

		public static int StackCount
		{
			get
			{
				return fsmExecutionStack.Count;
			}
		}

		public static int MaxStackCount { get; private set; }

		public static void PushFsm(Fsm executingFsm)
		{
			fsmExecutionStack.Push(executingFsm);
			if (fsmExecutionStack.Count > MaxStackCount)
			{
				MaxStackCount = fsmExecutionStack.Count;
			}
		}

		public static void PopFsm()
		{
			fsmExecutionStack.Pop();
		}

		public static string GetDebugString()
		{
			string text = "";
			text = text + "\nExecutingFsm: " + ExecutingFsm;
			return text + "\nExecutingState: " + ExecutingStateName;
		}
	}
}
