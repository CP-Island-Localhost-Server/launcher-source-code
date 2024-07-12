using Disney.MobileNetwork;

namespace Disney.Kelowna.Common
{
	public class KeyChainCacheableString : ICachableType
	{
		private string key;

		private string defaultValue;

		public string Value
		{
			get
			{
				return GetValue();
			}
			set
			{
				SetValue(value);
			}
		}

		public KeyChainCacheableString(string playerPrefsKey, string defaultValue)
		{
			key = playerPrefsKey;
			this.defaultValue = defaultValue;
		}

		public void SetValue(string value)
		{
			Service.Get<KeyChainManager>().PutString(key, value);
		}

		public string GetValue()
		{
			return Service.Get<KeyChainManager>().GetString(key);
		}

		public void Reset()
		{
			SetValue(defaultValue);
		}
	}
}
