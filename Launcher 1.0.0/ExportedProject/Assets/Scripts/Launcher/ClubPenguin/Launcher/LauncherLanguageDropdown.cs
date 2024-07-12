using System.Collections.Generic;
using DevonLocalization.Core;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(Dropdown))]
	public class LauncherLanguageDropdown : MonoBehaviour
	{
		public string[] LanguageTokens;

		public Language[] Languages;

		private SettingsDataService settingsDataService;

		private Localizer localizer;

		private void Awake()
		{
			settingsDataService = Service.Get<SettingsDataService>();
			localizer = Service.Get<Localizer>();
			Dropdown component = GetComponent<Dropdown>();
			int value = 0;
			List<string> list = new List<string>();
			for (int i = 0; i < LanguageTokens.Length; i++)
			{
				list.Add(localizer.GetTokenTranslation(LanguageTokens[i]));
				if (localizer.Language == Languages[i])
				{
					value = i;
				}
			}
			component.ClearOptions();
			component.AddOptions(list);
			component.value = value;
			component.onValueChanged.AddListener(onLanguageChanged);
		}

		private void onLanguageChanged(int value)
		{
			settingsDataService.Language = Languages[value];
			localizer.ChangeLanguage(Languages[value]);
		}
	}
}
