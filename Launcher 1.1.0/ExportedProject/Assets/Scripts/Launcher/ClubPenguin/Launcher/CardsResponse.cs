using System;
using System.Collections.Generic;

namespace ClubPenguin.Launcher
{
	[Serializable]
	public struct CardsResponse
	{
		public string language;

		public List<Card> cards;
	}
}
