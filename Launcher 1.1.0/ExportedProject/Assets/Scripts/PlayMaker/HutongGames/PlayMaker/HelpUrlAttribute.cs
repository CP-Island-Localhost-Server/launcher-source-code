using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class HelpUrlAttribute : Attribute
	{
		private readonly string url;

		public string Url
		{
			get
			{
				return url;
			}
		}

		public HelpUrlAttribute(string url)
		{
			this.url = url;
		}
	}
}
