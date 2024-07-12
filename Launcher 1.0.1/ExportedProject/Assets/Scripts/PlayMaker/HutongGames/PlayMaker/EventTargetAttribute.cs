using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class EventTargetAttribute : Attribute
	{
		private readonly FsmEventTarget.EventTarget target;

		public FsmEventTarget.EventTarget Target
		{
			get
			{
				return target;
			}
		}

		public EventTargetAttribute(FsmEventTarget.EventTarget target)
		{
			this.target = target;
		}
	}
}
