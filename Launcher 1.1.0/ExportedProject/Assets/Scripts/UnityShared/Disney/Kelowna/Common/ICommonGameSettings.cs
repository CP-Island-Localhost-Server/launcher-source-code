namespace Disney.Kelowna.Common
{
	public interface ICommonGameSettings
	{
		CacheableType<bool> SfxEnabled { get; }

		CacheableType<bool> MusicEnabled { get; }

		CacheableType<float> SfxVolume { get; }

		CacheableType<float> MusicVolume { get; }

		void RegisterSetting(ICachableType setting, bool canBeReset);
	}
}
