using System.Collections.Generic;
using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	public class SettingsGraphicsController : MonoBehaviour
	{
		private const string WINDOWED_TOKEN = "Settings.Graphics.WindowMode.Windowed";

		private const string FULLSCREEN_TOKEN = "Settings.Graphics.WindowMode.Fullscreen";

		[SerializeField]
		private Dropdown displayModeDropdown;

		[SerializeField]
		private Dropdown resolutionsDropdown;

		private SettingsDataService settingsDataService;

		private Resolution[] resolutions;

		private void Start()
		{
			settingsDataService = Service.Get<SettingsDataService>();
			Localizer localizer = Service.Get<Localizer>();
			displayModeDropdown.options[0].text = localizer.GetTokenTranslation("Settings.Graphics.WindowMode.Windowed");
			displayModeDropdown.options[1].text = localizer.GetTokenTranslation("Settings.Graphics.WindowMode.Fullscreen");
			displayModeDropdown.value = ((settingsDataService.IsFullscreen > -1) ? settingsDataService.IsFullscreen : 0);
			resolutions = Screen.resolutions;
			int value = 0;
			List<string> list = new List<string>();
			for (int i = 0; i < resolutions.Length; i++)
			{
				list.Add(resolutions[i].width + "x" + resolutions[i].height);
				if (resolutions[i].width == settingsDataService.ScreenWidth && resolutions[i].height == settingsDataService.ScreenHeight)
				{
					value = i;
				}
			}
			resolutionsDropdown.ClearOptions();
			resolutionsDropdown.AddOptions(list);
			resolutionsDropdown.value = value;
			displayModeDropdown.onValueChanged.AddListener(onDisplayModeChanged);
			resolutionsDropdown.onValueChanged.AddListener(onResolutionChanged);
		}

		private void onDisplayModeChanged(int value)
		{
			settingsDataService.IsFullscreen = value;
			Service.Get<ICPSwrveService>().Action("settings", "set_fullscreen", (value == 0) ? "windowed" : "fullscreen");
		}

		private void onResolutionChanged(int value)
		{
			settingsDataService.ScreenWidth = resolutions[value].width;
			settingsDataService.ScreenHeight = resolutions[value].height;
			Service.Get<ICPSwrveService>().Action("settings", "set_resolution", resolutions[value].width + "x" + resolutions[value].height);
		}
	}
}
