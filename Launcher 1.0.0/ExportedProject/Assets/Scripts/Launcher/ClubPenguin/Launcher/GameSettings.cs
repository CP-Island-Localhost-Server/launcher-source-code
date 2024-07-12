using DevonLocalization.Core;
using Disney.Kelowna.Common;

namespace ClubPenguin.Launcher
{
	public class GameSettings : ICommonGameSettings
	{
		public CacheableType<bool> SfxEnabled { get; private set; }

		public CacheableType<float> SfxVolume { get; private set; }

		public CacheableType<bool> MusicEnabled { get; private set; }

		public CacheableType<float> MusicVolume { get; private set; }

		public CacheableType<string> ViewedSlides { get; private set; }

		public CacheableType<Language> SavedLanguage { get; private set; }

		public GameSettings()
		{
			SfxEnabled = new CacheableType<bool>("cp.SfxEnabled", true);
			SfxVolume = new CacheableType<float>("cp.SfxVolume", 1f);
			MusicEnabled = new CacheableType<bool>("cp.MusicEnabled", true);
			MusicVolume = new CacheableType<float>("cp.MusicVolume", 1f);
			ViewedSlides = new CacheableType<string>("cp.ViewedSlides", "");
			SavedLanguage = new CacheableType<Language>("cp.SavedLanguage", Language.none);
		}

		public void RegisterSetting(ICachableType setting, bool canBeReset)
		{
		}
	}
}
