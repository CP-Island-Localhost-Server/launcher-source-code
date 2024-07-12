using System.Collections.Generic;
using DCPI.Platforms.SwrveManager.Analytics;

namespace DCPI.Platforms.SwrveManager
{
	public interface ISwrveManager
	{
		SwrveSDK GetSDK();

		string GetLibVersion();

		void InitWithAnalyticsKeySecret(int appId, string apiKey);

		void InitWithAnalyticsKeySecretAndUserId(int appId, string apiKey, string userId);

		void InitWithAnalyticsKeySecretAndUserIdAndAppVersion(int appId, string apiKey, string userId, string appVersion);

		void RegisterPlayer(string playerId);

		void RegisterPlayer(int appId, string apiKey, string playerId);

		void LogSwrvePurchase(string itemId, int cost, int quantity, string currency);

		void LogSwrvePurchase(string itemId, int cost, string currency);

		void LogSwrveCurrencyGiven(string givenCurrency, double givenAmount);

		void LogAdAction(AdActionAnalytics analytics);

		void LogFunnelAction(FunnelStepsAnalytics analytics);

		void LogFailedReceiptAction(FailedReceiptAnalytics analytics);

		void LogIAPAction(IAPAnalytics analytics);

		void LogPurchaseAction(PurchaseAnalytics analytics);

		void LogCurrencyGivenAction(CurrencyGivenAnalytics analytics);

		void LogAction(ActionAnalytics analytics);

		void LogTestImpressionAction(TestImpressionAnalytics analytics);

		void LogTimingAction(TimingAnalytics analytics);

		void LogNavigationAction(NavigationActionAnalytics analytics);

		void LogErrorAction(ErrorAnalytics analytics);

		void LogGenericAction(string action);

		void LogGenericAction(string action, Dictionary<string, object> messageDetails);

		void LogAnalyticsAction(IAnalytics analytics);
	}
}
