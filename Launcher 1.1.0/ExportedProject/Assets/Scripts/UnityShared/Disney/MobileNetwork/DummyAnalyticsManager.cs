using System.Collections.Generic;

namespace Disney.MobileNetwork
{
	public class DummyAnalyticsManager : IAnalyticsManager
	{
		private string playerId { get; set; }

		public void SetPlayerId(string playerId)
		{
			this.playerId = playerId;
		}

		public string GetPlayerId()
		{
			return playerId;
		}

		public void SetKeys(string analyticsKey, string analyticsSecret)
		{
		}

		public void SetAutoLogLifetimeEvents(bool autoLogLifetimeEvents)
		{
		}

		public void SetLogEventsInEditor(bool logEventsWhenRunningInEditor)
		{
		}

		public void Init()
		{
		}

		public void LogEvent(string EventInfo)
		{
		}

		public void LogGameAction(Dictionary<string, object> GameData)
		{
		}

		public void LogMoneyAction(Dictionary<string, object> MoneyData)
		{
		}

		public void LogAnalyticsEventWithContext(string eventName, Dictionary<string, object> dataDetails)
		{
		}

		public void FlushAnalyticsQueue()
		{
		}
	}
}
