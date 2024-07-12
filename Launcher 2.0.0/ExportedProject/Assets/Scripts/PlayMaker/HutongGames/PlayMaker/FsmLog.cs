using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public class FsmLog
	{
		public static int MaxSize;

		private static readonly List<FsmLog> Logs;

		private static readonly FsmLogEntry[] logEntryPool;

		private static int nextLogEntryPoolIndex;

		private List<FsmLogEntry> entries = new List<FsmLogEntry>();

		public static bool LoggingEnabled { get; set; }

		public static bool MirrorDebugLog { get; set; }

		public static bool EnableDebugFlow { get; set; }

		public Fsm Fsm { get; private set; }

		public List<FsmLogEntry> Entries
		{
			get
			{
				return entries;
			}
		}

		static FsmLog()
		{
			MaxSize = 10000;
			Logs = new List<FsmLog>();
			LoggingEnabled = !Application.isEditor;
			logEntryPool = new FsmLogEntry[MaxSize];
			for (int i = 0; i < logEntryPool.Length; i++)
			{
				logEntryPool[i] = new FsmLogEntry();
			}
		}

		private FsmLog(Fsm fsm)
		{
			Fsm = fsm;
		}

		public static FsmLog GetLog(Fsm fsm)
		{
			if (fsm == null)
			{
				return null;
			}
			foreach (FsmLog log in Logs)
			{
				if (log.Fsm == fsm)
				{
					return log;
				}
			}
			FsmLog fsmLog = new FsmLog(fsm);
			Logs.Add(fsmLog);
			return fsmLog;
		}

		public static void ClearLogs()
		{
			foreach (FsmLog log in Logs)
			{
				log.Clear();
			}
		}

		private void AddEntry(FsmLogEntry entry, bool sendToUnityLog = false)
		{
			entry.Log = this;
			entry.Time = FsmTime.RealtimeSinceStartup;
			entry.FrameCount = Time.frameCount;
			if (IsCollisionEvent(entry.Event))
			{
				entry.GameObject = entry.Fsm.CollisionGO;
				entry.GameObjectName = entry.Fsm.CollisionName;
			}
			if (IsTriggerEvent(entry.Event))
			{
				entry.GameObject = entry.Fsm.TriggerGO;
				entry.GameObjectName = entry.Fsm.TriggerName;
			}
			if (IsCollision2DEvent(entry.Event))
			{
				entry.GameObject = entry.Fsm.Collision2dGO;
				entry.GameObjectName = entry.Fsm.Collision2dName;
			}
			if (IsTrigger2DEvent(entry.Event))
			{
				entry.GameObject = entry.Fsm.Trigger2dGO;
				entry.GameObjectName = entry.Fsm.Trigger2dName;
			}
			entries.Add(entry);
			switch (entry.LogType)
			{
			case FsmLogType.Error:
				Debug.LogError(FormatUnityLogString(entry.Text));
				return;
			case FsmLogType.Warning:
				Debug.LogWarning(FormatUnityLogString(entry.Text));
				return;
			}
			if ((MirrorDebugLog || sendToUnityLog) && entry.LogType != FsmLogType.Transition)
			{
				Debug.Log(FormatUnityLogString(entry.Text));
			}
		}

		private static bool IsCollisionEvent(FsmEvent fsmEvent)
		{
			if (fsmEvent != null)
			{
				if (fsmEvent != FsmEvent.CollisionEnter && fsmEvent != FsmEvent.CollisionStay)
				{
					return fsmEvent == FsmEvent.CollisionExit;
				}
				return true;
			}
			return false;
		}

		private static bool IsTriggerEvent(FsmEvent fsmEvent)
		{
			if (fsmEvent != null)
			{
				if (fsmEvent != FsmEvent.TriggerEnter && fsmEvent != FsmEvent.TriggerStay)
				{
					return fsmEvent == FsmEvent.TriggerExit;
				}
				return true;
			}
			return false;
		}

		private static bool IsCollision2DEvent(FsmEvent fsmEvent)
		{
			if (fsmEvent != null)
			{
				if (fsmEvent != FsmEvent.CollisionEnter2D && fsmEvent != FsmEvent.CollisionStay2D)
				{
					return fsmEvent == FsmEvent.CollisionExit2D;
				}
				return true;
			}
			return false;
		}

		private static bool IsTrigger2DEvent(FsmEvent fsmEvent)
		{
			if (fsmEvent != null)
			{
				if (fsmEvent != FsmEvent.TriggerEnter2D && fsmEvent != FsmEvent.TriggerStay2D)
				{
					return fsmEvent == FsmEvent.TriggerExit2D;
				}
				return true;
			}
			return false;
		}

		private FsmLogEntry NewFsmLogEntry(FsmLogType logType)
		{
			FsmLogEntry fsmLogEntry = logEntryPool[nextLogEntryPoolIndex];
			if (fsmLogEntry.Log != null)
			{
				fsmLogEntry.Log.RemoveEntry(fsmLogEntry);
				fsmLogEntry.Reset();
			}
			fsmLogEntry.Log = this;
			fsmLogEntry.LogType = logType;
			nextLogEntryPoolIndex++;
			if (nextLogEntryPoolIndex >= logEntryPool.Length)
			{
				nextLogEntryPoolIndex = 0;
			}
			return fsmLogEntry;
		}

		private void RemoveEntry(FsmLogEntry entry)
		{
			if (entries != null)
			{
				entries.Remove(entry);
			}
		}

		public void LogEvent(FsmEvent fsmEvent, FsmState state)
		{
			FsmLogEntry fsmLogEntry = NewFsmLogEntry(FsmLogType.Event);
			fsmLogEntry.State = state;
			fsmLogEntry.SentByState = Fsm.EventData.SentByState;
			fsmLogEntry.Action = Fsm.EventData.SentByAction;
			fsmLogEntry.Event = fsmEvent;
			AddEntry(fsmLogEntry);
		}

		public void LogSendEvent(FsmState state, FsmEvent fsmEvent, FsmEventTarget eventTarget)
		{
			if (state != null && fsmEvent != null && !fsmEvent.IsSystemEvent)
			{
				FsmLogEntry fsmLogEntry = NewFsmLogEntry(FsmLogType.SendEvent);
				fsmLogEntry.State = state;
				fsmLogEntry.Event = fsmEvent;
				fsmLogEntry.EventTarget = new FsmEventTarget(eventTarget);
				AddEntry(fsmLogEntry);
			}
		}

		public void LogExitState(FsmState state)
		{
			if (state != null)
			{
				FsmLogEntry fsmLogEntry = NewFsmLogEntry(FsmLogType.ExitState);
				fsmLogEntry.State = state;
				fsmLogEntry.StateTime = FsmTime.RealtimeSinceStartup - state.RealStartTime;
				if (EnableDebugFlow && state.Fsm.EnableDebugFlow && !PlayMakerFSM.ApplicationIsQuitting)
				{
					fsmLogEntry.FsmVariablesCopy = new FsmVariables(state.Fsm.Variables);
					fsmLogEntry.GlobalVariablesCopy = new FsmVariables(FsmVariables.GlobalVariables);
				}
				AddEntry(fsmLogEntry);
			}
		}

		public void LogEnterState(FsmState state)
		{
			if (state != null)
			{
				FsmLogEntry fsmLogEntry = NewFsmLogEntry(FsmLogType.EnterState);
				fsmLogEntry.State = state;
				if (EnableDebugFlow && state.Fsm.EnableDebugFlow)
				{
					fsmLogEntry.FsmVariablesCopy = new FsmVariables(state.Fsm.Variables);
					fsmLogEntry.GlobalVariablesCopy = new FsmVariables(FsmVariables.GlobalVariables);
				}
				AddEntry(fsmLogEntry);
			}
		}

		public void LogTransition(FsmState fromState, FsmTransition transition)
		{
			FsmLogEntry fsmLogEntry = NewFsmLogEntry(FsmLogType.Transition);
			fsmLogEntry.State = fromState;
			fsmLogEntry.Transition = transition;
			AddEntry(fsmLogEntry);
		}

		public void LogBreak()
		{
			FsmLogEntry fsmLogEntry = NewFsmLogEntry(FsmLogType.Break);
			fsmLogEntry.State = FsmExecutionStack.ExecutingState;
			Debug.Log("BREAK: " + FormatUnityLogString("Breakpoint"));
			AddEntry(fsmLogEntry);
		}

		public void LogAction(FsmLogType logType, string text, bool sendToUnityLog = false)
		{
			if (FsmExecutionStack.ExecutingAction == null)
			{
				switch (logType)
				{
				case FsmLogType.Info:
					Debug.Log(text);
					break;
				case FsmLogType.Warning:
					Debug.LogWarning(text);
					break;
				case FsmLogType.Error:
					Debug.LogError(text);
					break;
				default:
					Debug.Log(text);
					break;
				}
			}
			else
			{
				FsmLogEntry fsmLogEntry = NewFsmLogEntry(logType);
				fsmLogEntry.State = FsmExecutionStack.ExecutingState;
				fsmLogEntry.Action = FsmExecutionStack.ExecutingAction;
				fsmLogEntry.Text = FsmUtility.StripNamespace(FsmExecutionStack.ExecutingAction.ToString()) + " : " + text;
				AddEntry(fsmLogEntry, sendToUnityLog);
			}
		}

		public void Log(FsmLogType logType, string text)
		{
			FsmLogEntry fsmLogEntry = NewFsmLogEntry(logType);
			fsmLogEntry.State = FsmExecutionStack.ExecutingState;
			fsmLogEntry.Text = text;
			AddEntry(fsmLogEntry);
		}

		public void LogStart(FsmState startState)
		{
			FsmLogEntry fsmLogEntry = NewFsmLogEntry(FsmLogType.Start);
			fsmLogEntry.State = startState;
			AddEntry(fsmLogEntry);
		}

		public void LogStop()
		{
			FsmLogEntry entry = NewFsmLogEntry(FsmLogType.Stop);
			AddEntry(entry);
		}

		public void Log(string text)
		{
			Log(FsmLogType.Info, text);
		}

		public void LogWarning(string text)
		{
			Log(FsmLogType.Warning, text);
		}

		public void LogError(string text)
		{
			Log(FsmLogType.Error, text);
		}

		private string FormatUnityLogString(string text)
		{
			string text2 = Fsm.GetFullFsmLabel(Fsm);
			if (FsmExecutionStack.ExecutingState != null)
			{
				text2 = text2 + " : " + FsmExecutionStack.ExecutingStateName;
			}
			if (FsmExecutionStack.ExecutingAction != null)
			{
				text2 += FsmExecutionStack.ExecutingAction.Name;
			}
			return text2 + " : " + text;
		}

		public void Clear()
		{
			if (entries != null)
			{
				entries.Clear();
			}
		}

		public void OnDestroy()
		{
			Logs.Remove(this);
			Clear();
			entries = null;
			Fsm = null;
		}
	}
}
