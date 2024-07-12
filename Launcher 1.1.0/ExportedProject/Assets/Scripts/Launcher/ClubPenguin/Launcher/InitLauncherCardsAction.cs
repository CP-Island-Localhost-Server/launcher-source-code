using System.Collections;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(InitContentSystemAction))]
	[RequireComponent(typeof(InitLocalizerSetupAction))]
	[RequireComponent(typeof(CheckInternetConnectionAction))]
	[RequireComponent(typeof(InitInstallerManifestServiceAction))]
	[RequireComponent(typeof(InitAnalyticsAction))]
	public class InitLauncherCardsAction : InitActionComponent
	{
		public LauncherCards LauncherCards;

		public override bool HasSecondPass
		{
			get
			{
				return false;
			}
		}

		public override bool HasCompletedPass
		{
			get
			{
				return false;
			}
		}

		public override IEnumerator PerformFirstPass()
		{
			string url = string.Format("{0}/{1}?lang={2}", ConfigHelper.GetEnvironmentProperty<string>("CPWebsiteAPIServicehost"), "wp-json/snowflake/v1/cards", LocalizationLanguage.GetISOLanguageString(Service.Get<Localizer>().Language));
			WWW www = new WWW(url);
			yield return www;
			CardsResponse cardsResponse = Service.Get<JsonService>().Deserialize<CardsResponse>(www.text);
			LauncherCards.InitializeCards(cardsResponse);
		}
	}
}
