using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.All)]
	public sealed class TooltipAttribute : Attribute
	{
		private readonly string text;

		public string Text
		{
			get
			{
				return text;
			}
		}

		public TooltipAttribute(string text)
		{
			this.text = text;
		}
	}
}
