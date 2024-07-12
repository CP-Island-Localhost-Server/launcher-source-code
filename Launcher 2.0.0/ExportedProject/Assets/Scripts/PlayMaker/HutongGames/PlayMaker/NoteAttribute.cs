using System;

namespace HutongGames.PlayMaker
{
	[AttributeUsage(AttributeTargets.All)]
	public sealed class NoteAttribute : Attribute
	{
		private readonly string text;

		public string Text
		{
			get
			{
				return text;
			}
		}

		public NoteAttribute(string text)
		{
			this.text = text;
		}
	}
}
