using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace Disney.MobileNetwork
{
	public class AnalyticsManager : MonoBehaviour, IPlugin, IInitializable, IAnalyticsManager
	{
		private const string kUnsetConfigValue = "TODO";

		private string m_analyticsKey = "TODO";

		private string m_analyticsSecret = "TODO";

		private bool m_autoLogLifetimeEvents = true;

		private bool m_logEventsInEditor = false;

		private string mPlayerId = null;

		private bool m_initialized = false;

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

		public void SetPlayerId(string playerId)
		{
			mPlayerId = playerId;
		}

		public string GetPlayerId()
		{
			return mPlayerId;
		}

		public void SetKeys(string analyticsKey, string analyticsSecret)
		{
			m_analyticsKey = analyticsKey;
			m_analyticsSecret = analyticsSecret;
		}

		public void SetAutoLogLifetimeEvents(bool autoLogLifetimeEvents)
		{
			m_autoLogLifetimeEvents = autoLogLifetimeEvents;
		}

		public void SetLogEventsInEditor(bool logEventsWhenRunningInEditor)
		{
			m_logEventsInEditor = logEventsWhenRunningInEditor;
		}

		private Dictionary<string, object> _AddPlayerIdToDictionary(Dictionary<string, object> dictionary)
		{
			if (mPlayerId != null && !dictionary.ContainsKey("player_id"))
			{
				dictionary.Add("player_id", mPlayerId);
			}
			return dictionary;
		}

		public void Start()
		{
			if (m_autoLogLifetimeEvents)
			{
				logAppStart();
			}
		}

		public void OnApplicationPause(bool paused)
		{
			if (m_autoLogLifetimeEvents)
			{
				if (paused)
				{
					logAppBackground();
				}
				else
				{
					logAppForeground();
				}
			}
		}

		public void OnApplicationQuit()
		{
			if (m_autoLogLifetimeEvents)
			{
				logAppEnd();
			}
		}

		public void Init()
		{
			if (!m_analyticsKey.Equals("TODO") && !m_analyticsSecret.Equals("TODO"))
			{
				string bundleIdentifier = EnvironmentManager.BundleIdentifier;
				Version bundleVersion = EnvironmentManager.BundleVersion;
				m_logger.LogDebug(this, "AnalyticsManager Initialize() called with Key [" + m_analyticsKey + "] Secret [" + m_analyticsSecret + "]");
				if (!string.IsNullOrEmpty(bundleIdentifier) && !string.IsNullOrEmpty(m_analyticsKey) && !string.IsNullOrEmpty(m_analyticsSecret))
				{
					m_initialized = true;
				}
			}
			else
			{
				m_logger.LogDebug(this, "One or more DMOAnalytics configuration settings has not been set");
			}
		}

		public void LogEvent(string EventInfo)
		{
			m_logger.LogDebug(typeof(AnalyticsManager), "LogEvent[" + EventInfo + "]");
			if (EventInfo != null && Application.isEditor && m_logEventsInEditor && !m_initialized)
			{
			}
		}

		private void logAppStart()
		{
			m_logger.LogDebug(typeof(AnalyticsManager), "LogAppStart Called.");
			if (Application.isEditor && m_logEventsInEditor && !m_initialized)
			{
			}
		}

		private void logAppForeground()
		{
			m_logger.LogDebug(typeof(AnalyticsManager), "LogAppForeground Called.");
			if (Application.isEditor && m_logEventsInEditor && !m_initialized)
			{
			}
		}

		private void logAppBackground()
		{
			m_logger.LogDebug(typeof(AnalyticsManager), "LogAppBackground Called.");
			if (Application.isEditor && m_logEventsInEditor && !m_initialized)
			{
			}
		}

		private void logAppEnd()
		{
			m_logger.LogDebug(typeof(AnalyticsManager), "LogAppEnd Called.");
			if (Application.isEditor && m_logEventsInEditor && !m_initialized)
			{
			}
		}

		public void LogGameAction(Dictionary<string, object> GameData)
		{
			if (GameData != null)
			{
				GameData = _AddPlayerIdToDictionary(GameData);
				m_logger.LogDebug(typeof(AnalyticsManager), "LogGameAction[" + JsonMapper.ToJson(GameData) + "]");
				if (Application.isEditor && m_logEventsInEditor && !m_initialized)
				{
				}
			}
		}

		public void LogMoneyAction(Dictionary<string, object> MoneyData)
		{
			if (MoneyData != null)
			{
				MoneyData = _AddPlayerIdToDictionary(MoneyData);
				m_logger.LogDebug(typeof(AnalyticsManager), "LogMoneyAction[" + JsonMapper.ToJson(MoneyData) + "]");
				if (Application.isEditor && m_logEventsInEditor && !m_initialized)
				{
				}
			}
		}

		public void LogAnalyticsEventWithContext(string eventName, Dictionary<string, object> dataDetails)
		{
			if (eventName != null && dataDetails != null)
			{
				dataDetails = _AddPlayerIdToDictionary(dataDetails);
				m_logger.LogDebug(typeof(AnalyticsManager), "Log " + eventName + "[" + JsonMapper.ToJson(dataDetails) + "]");
				if (Application.isEditor && m_logEventsInEditor && !m_initialized)
				{
				}
			}
		}

		public void FlushAnalyticsQueue()
		{
			if (!Application.isEditor || (m_logEventsInEditor && m_initialized))
			{
				m_logger.LogDebug(typeof(AnalyticsManager), "FlushAnalyticsQueue called");
			}
		}
	}
}
