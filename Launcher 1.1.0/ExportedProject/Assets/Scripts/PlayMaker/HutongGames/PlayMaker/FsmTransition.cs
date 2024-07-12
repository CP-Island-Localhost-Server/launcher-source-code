using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmTransition : IEquatable<FsmTransition>
	{
		public enum CustomLinkStyle : byte
		{
			Default = 0,
			Bezier = 1,
			Circuit = 2
		}

		public enum CustomLinkConstraint : byte
		{
			None = 0,
			LockLeft = 1,
			LockRight = 2
		}

		[SerializeField]
		private FsmEvent fsmEvent;

		[SerializeField]
		private string toState;

		[SerializeField]
		private CustomLinkStyle linkStyle;

		[SerializeField]
		private CustomLinkConstraint linkConstraint;

		[SerializeField]
		private byte colorIndex;

		public FsmEvent FsmEvent
		{
			get
			{
				return fsmEvent;
			}
			set
			{
				fsmEvent = value;
			}
		}

		public string ToState
		{
			get
			{
				return toState;
			}
			set
			{
				toState = value;
			}
		}

		public CustomLinkStyle LinkStyle
		{
			get
			{
				return linkStyle;
			}
			set
			{
				linkStyle = value;
			}
		}

		public CustomLinkConstraint LinkConstraint
		{
			get
			{
				return linkConstraint;
			}
			set
			{
				linkConstraint = value;
			}
		}

		public int ColorIndex
		{
			get
			{
				return colorIndex;
			}
			set
			{
				colorIndex = (byte)Mathf.Clamp(value, 0, PlayMakerPrefs.Colors.Length - 1);
			}
		}

		public string EventName
		{
			get
			{
				if (!FsmEvent.IsNullOrEmpty(fsmEvent))
				{
					return fsmEvent.Name;
				}
				return string.Empty;
			}
		}

		public FsmTransition()
		{
		}

		public FsmTransition(FsmTransition source)
		{
			fsmEvent = source.FsmEvent;
			toState = source.toState;
			linkStyle = source.linkStyle;
			linkConstraint = source.linkConstraint;
			colorIndex = source.colorIndex;
		}

		public bool Equals(FsmTransition other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(this, other))
			{
				return true;
			}
			if (other.toState != toState)
			{
				return false;
			}
			return other.EventName == EventName;
		}
	}
}
