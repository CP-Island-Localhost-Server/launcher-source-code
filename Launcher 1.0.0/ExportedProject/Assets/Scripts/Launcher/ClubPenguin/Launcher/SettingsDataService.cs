using DevonLocalization.Core;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class SettingsDataService
	{
		private const string MUSIC_VOLUME_KEY = "LaunchData.MusicVolume";

		private const string SFX_VOLUME_KEY = "LaunchData.SFXVolume";

		private const string IS_FULLSCREEN_KEY = "LaunchData.IsFullScreen";

		private const string SCREEN_WIDTH_KEY = "LaunchData.ScreenWidth";

		private const string SCREEN_HEIGHT_KEY = "LaunchData.ScreenHeight";

		private float musicVolume;

		private float sfxVolume;

		private int isFullscreen;

		private int screenWidth;

		private int screenHeight;

		private Language language;

		public float MusicVolume
		{
			get
			{
				return musicVolume;
			}
			set
			{
				musicVolume = value;
				PlayerPrefs.SetFloat("LaunchData.MusicVolume", value);
			}
		}

		public float SFXVolume
		{
			get
			{
				return sfxVolume;
			}
			set
			{
				sfxVolume = value;
				PlayerPrefs.SetFloat("LaunchData.SFXVolume", value);
			}
		}

		public int IsFullscreen
		{
			get
			{
				return isFullscreen;
			}
			set
			{
				isFullscreen = value;
				PlayerPrefs.SetInt("LaunchData.ScreenWidth", value);
			}
		}

		public int ScreenWidth
		{
			get
			{
				return screenWidth;
			}
			set
			{
				screenWidth = value;
				PlayerPrefs.SetInt("LaunchData.ScreenWidth", value);
			}
		}

		public int ScreenHeight
		{
			get
			{
				return screenHeight;
			}
			set
			{
				screenHeight = value;
				PlayerPrefs.SetInt("LaunchData.ScreenHeight", value);
			}
		}

		public Language Language
		{
			get
			{
				return language;
			}
			set
			{
				language = value;
				Service.Get<GameSettings>().SavedLanguage.Value = language;
			}
		}

		public SettingsDataService()
		{
			musicVolume = -1f;
			sfxVolume = -1f;
			isFullscreen = -1;
			screenWidth = 0;
			screenHeight = 0;
			language = Service.Get<GameSettings>().SavedLanguage;
		}
	}
}
