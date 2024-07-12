using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	public class SettingsAudioController : MonoBehaviour
	{
		public Slider MusicSlider;

		public Slider SFXSlider;

		private SettingsDataService settingsDataService;

		private void Start()
		{
			settingsDataService = Service.Get<SettingsDataService>();
			MusicSlider.value = settingsDataService.MusicVolume;
			SFXSlider.value = settingsDataService.SFXVolume;
			MusicSlider.onValueChanged.AddListener(onMusicVolumeChanged);
			SFXSlider.onValueChanged.AddListener(onSFXVolumeChanged);
		}

		private void onMusicVolumeChanged(float value)
		{
			settingsDataService.MusicVolume = value;
		}

		private void onSFXVolumeChanged(float value)
		{
			settingsDataService.SFXVolume = value;
		}
	}
}
