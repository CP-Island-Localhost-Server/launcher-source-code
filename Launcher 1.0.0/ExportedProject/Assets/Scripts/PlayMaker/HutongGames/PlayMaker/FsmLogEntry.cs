using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public class FsmLogEntry
	{
		private string text;

		private string textWithTimecode;

		public FsmLog Log { get; set; }

		public FsmLogType LogType { get; set; }

		public Fsm Fsm
		{
			get
			{
				return Log.Fsm;
			}
		}

		public FsmState State { get; set; }

		public FsmState SentByState { get; set; }

		public FsmStateAction Action { get; set; }

		public FsmEvent Event { get; set; }

		public FsmTransition Transition { get; set; }

		public FsmEventTarget EventTarget { get; set; }

		public float Time { get; set; }

		public float StateTime { get; set; }

		public int FrameCount { get; set; }

		public FsmVariables FsmVariablesCopy { get; set; }

		public FsmVariables GlobalVariablesCopy { get; set; }

		public GameObject GameObject { get; set; }

		public string GameObjectName { get; set; }

		public Texture GameObjectIcon { get; set; }

		public string Text
		{
			get
			{
				if (text == null)
				{
					switch (LogType)
					{
					case FsmLogType.Event:
						text = "EVENT: " + Event.Name;
						break;
					case FsmLogType.ExitState:
						text = string.Format("EXIT: {0} [{1:f2}s]", State.Name, StateTime);
						break;
					case FsmLogType.EnterState:
						text = "ENTER: " + State.Name;
						break;
					case FsmLogType.Break:
						text = "BREAK: " + State.Name;
						break;
					case FsmLogType.SendEvent:
						text = "SEND EVENT: " + Event.Name;
						break;
					case FsmLogType.Start:
						text = "START";
						break;
					case FsmLogType.Stop:
						text = "STOP";
						break;
					default:
						throw new ArgumentOutOfRangeException();
					case FsmLogType.Info:
					case FsmLogType.Warning:
					case FsmLogType.Error:
					case FsmLogType.Transition:
						break;
					}
				}
				return text;
			}
			set
			{
				text = value;
			}
		}

		public string Text2 { get; set; }

		public string TextWithTimecode
		{
			get
			{
				return textWithTimecode ?? (textWithTimecode = FsmTime.FormatTime(Time) + " " + Text);
			}
		}

		public int GetIndex()
		{
			for (int i = 0; i < Log.Entries.Count; i++)
			{
				if (Log.Entries[i] == this)
				{
					return i;
				}
			}
			return -1;
		}

		public void Reset()
		{
			Log = null;
			State = null;
			SentByState = null;
			Action = null;
			Event = null;
			Transition = null;
			EventTarget = null;
			Time = 0f;
			StateTime = 0f;
			FrameCount = 0;
			FsmVariablesCopy = null;
			GlobalVariablesCopy = null;
			GameObject = null;
			GameObjectName = null;
			GameObjectIcon = null;
			Text = null;
			Text2 = null;
			textWithTimecode = null;
		}

		public void DebugLog()
		{
			Debug.Log("Sent By: " + FsmUtility.GetPath(SentByState) + " : " + ((Action != null) ? Action.Name : "None (Action)"));
		}
	}
}
