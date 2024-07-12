using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public class ActionReport
	{
		public static readonly List<ActionReport> ActionReportList = new List<ActionReport>();

		public static int InfoCount;

		public static int ErrorCount;

		public PlayMakerFSM fsm;

		public FsmState state;

		public FsmStateAction action;

		public int actionIndex;

		public string logText;

		public bool isError;

		public string parameter;

		public static void Start()
		{
			ActionReportList.Clear();
			InfoCount = 0;
			ErrorCount = 0;
		}

		public static ActionReport Log(PlayMakerFSM fsm, FsmState state, FsmStateAction action, int actionIndex, string parameter, string logLine, bool isError = false)
		{
			if (!PlayMakerGlobals.IsEditor)
			{
				return null;
			}
			ActionReport actionReport = new ActionReport();
			actionReport.fsm = fsm;
			actionReport.state = state;
			actionReport.action = action;
			actionReport.actionIndex = actionIndex;
			actionReport.parameter = parameter;
			actionReport.logText = logLine;
			actionReport.isError = isError;
			ActionReport actionReport2 = actionReport;
			if (!ActionReportContains(actionReport2))
			{
				ActionReportList.Add(actionReport2);
				InfoCount++;
				return actionReport2;
			}
			return null;
		}

		private static bool ActionReportContains(ActionReport report)
		{
			foreach (ActionReport actionReport in ActionReportList)
			{
				if (actionReport.SameAs(report))
				{
					return true;
				}
			}
			return false;
		}

		private bool SameAs(ActionReport actionReport)
		{
			if (object.ReferenceEquals(actionReport.fsm, fsm) && actionReport.state == state && actionReport.actionIndex == actionIndex && actionReport.logText == logText && actionReport.isError == isError)
			{
				return actionReport.parameter == parameter;
			}
			return false;
		}

		public static void LogWarning(PlayMakerFSM fsm, FsmState state, FsmStateAction action, int actionIndex, string parameter, string logLine)
		{
			Log(fsm, state, action, actionIndex, parameter, logLine, true);
			Debug.LogWarning(FsmUtility.GetPath(state, action) + logLine, fsm);
			ErrorCount++;
		}

		public static void LogError(PlayMakerFSM fsm, FsmState state, FsmStateAction action, int actionIndex, string parameter, string logLine)
		{
			Log(fsm, state, action, actionIndex, parameter, logLine, true);
			Debug.LogError(FsmUtility.GetPath(state, action) + logLine, fsm);
			ErrorCount++;
		}

		public static void LogError(PlayMakerFSM fsm, FsmState state, FsmStateAction action, int actionIndex, string logLine)
		{
			Log(fsm, state, action, actionIndex, logLine, "", true);
			Debug.LogError(FsmUtility.GetPath(state, action) + logLine, fsm);
			ErrorCount++;
		}

		public static void Clear()
		{
			ActionReportList.Clear();
		}

		public static void Remove(PlayMakerFSM fsm)
		{
			ActionReportList.RemoveAll((ActionReport x) => x.fsm == fsm);
		}

		public static int GetCount()
		{
			return ActionReportList.Count;
		}
	}
}
