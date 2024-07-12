using System;
using DevonLocalization.Core;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	[Serializable]
	public class ButtonDefinition
	{
		public Button ButtonPrefab;

		[LocalizationToken]
		public string ButtonToken;
	}
}
