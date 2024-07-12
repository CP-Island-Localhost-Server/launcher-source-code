using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ActionSection : Attribute
	{
		private readonly string section;

		public string Section
		{
			get
			{
				return section;
			}
		}

		public ActionSection(string section)
		{
			this.section = section;
		}
	}
}
