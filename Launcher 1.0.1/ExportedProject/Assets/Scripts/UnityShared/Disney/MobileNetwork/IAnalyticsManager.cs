using System.Collections.Generic;

namespace Disney.MobileNetwork
{
	public interface IAnalyticsManager
	{
		void SetPlayerId(string playerId);

		string GetPlayerId();

		void SetKeys(string analyticsKey, string analyticsSecret);

		void SetAutoLogLifetimeEvents(bool autoLogLifetimeEvents);

		void SetLogEventsInEditor(bool logEventsWhenRunningInEditor);

		void Init();

		void LogEvent(string EventInfo);

		void LogGameAction(Dictionary<string, object> GameData);

		void LogMoneyAction(Dictionary<string, object> MoneyData);

		void LogAnalyticsEventWithContext(string eventName, Dictionary<string, object> dataDetails);

		void FlushAnalyticsQueue();
	}
}
