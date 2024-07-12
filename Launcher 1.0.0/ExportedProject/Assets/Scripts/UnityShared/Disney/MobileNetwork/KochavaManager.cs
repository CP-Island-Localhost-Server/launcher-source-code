using UnityEngine;

namespace Disney.MobileNetwork
{
	public class KochavaManager : MonoBehaviour, IPlugin, IInitializable
	{
		public class KochavaConfiguration
		{
			public string KochavaAppId = "TODO_KoDefault";

			public string KochavaAppIdIOS = "TODO_KoIOS";

			public string KochavaAppIdAndroid = "TODO_KoAndroid";

			public string KochavaAppIdKindle = "TODO_KoKindle";

			public string KochavaAppIdBlackberry = "TODO_KoBlackberry";

			public string KochavaAppIdWindowsPhone = "TODO_KoWin8";

			public bool IncognitoMode = false;

			public bool RequestAttribution = false;

			public string AppCurrency = "USD";
		}

		protected KochavaConfiguration m_configuration;

		private LoggerHelper m_logger = new LoggerHelper();

		public LoggerHelper Logger
		{
			get
			{
				return m_logger;
			}
		}

		public void SetLogger(LoggerHelper.LoggerDelegate loggerMessageHandler)
		{
			m_logger.LogMessageHandler += loggerMessageHandler;
		}

		public void Configure(KochavaConfiguration configuration)
		{
			m_configuration = configuration;
		}

		public void Init()
		{
			base.gameObject.name = "_Kochava Analytics";
			Kochava kochava = base.gameObject.AddComponent<Kochava>();
			kochava.kochavaAppId = m_configuration.KochavaAppId;
			kochava.kochavaAppIdIOS = m_configuration.KochavaAppIdIOS;
			kochava.kochavaAppIdAndroid = m_configuration.KochavaAppIdAndroid;
			kochava.kochavaAppIdKindle = m_configuration.KochavaAppIdKindle;
			kochava.kochavaAppIdBlackberry = m_configuration.KochavaAppIdBlackberry;
			kochava.kochavaAppIdWindowsPhone = m_configuration.KochavaAppIdWindowsPhone;
			kochava.incognitoMode = m_configuration.IncognitoMode;
			kochava.requestAttribution = m_configuration.RequestAttribution;
			kochava.appCurrency = m_configuration.AppCurrency;
			kochava.appVersion = EnvironmentManager.BundleVersion.ToString();
			kochava.appIdentifier = EnvironmentManager.BundleIdentifier;
			kochava.Init();
			Logger.LogDebug(this, "KochavaManager initialized!! id = " + kochava.appVersion + " ver = " + kochava.appIdentifier);
		}
	}
}
