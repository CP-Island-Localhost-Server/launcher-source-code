using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(InitConfiguratorAction), typeof(InitCoreServicesAction))]
	public class InitLocalizerSetupAction : InitActionComponent
	{
		[LocalizationToken]
		public string AppTitleToken;

		private bool tokensLoaded;

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
			Localizer localizer = Localizer.Instance;
			Service.Set(localizer);
			Localizer localizer2 = localizer;
			localizer2.TokensUpdated = (Localizer.TokensUpdatedDelegate)Delegate.Combine(localizer2.TokensUpdated, (Localizer.TokensUpdatedDelegate)delegate
			{
				AppWindowUtil.SetTitle(localizer.GetTokenTranslation(AppTitleToken));
			});
			Language savedLanguage = Service.Get<GameSettings>().SavedLanguage;
			if (savedLanguage == Language.none)
			{
				localizer.Language = LocalizationLanguage.GetLanguage();
			}
			else
			{
				localizer.Language = savedLanguage;
			}
			localizer.LanguageString = LocalizationLanguage.GetLanguageString(localizer.Language);
			string languageStringOneID = LocalizationLanguage.GetOneIDLanguageString(localizer.Language);
			localizer.LanguageStringOneID = languageStringOneID;
			Thread.CurrentThread.CurrentCulture = new CultureInfo(LocalizationLanguage.GetCultureLanguageString(localizer.Language));
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(LocalizationLanguage.GetCultureLanguageString(localizer.Language));
			tokensLoaded = false;
			ILocalizedTokenFilePath tokensFilePath = new AppTokensFilePath(Localizer.DEFAULT_TOKEN_LOCATION, Platform.global);
			DateTime cutOffTime = DateTime.Now.AddMinutes(1.0);
			localizer.LoadTokensFromLocalJSON(tokensFilePath, onTokensLoaded);
			while (!tokensLoaded && !(DateTime.Now > cutOffTime))
			{
				yield return null;
			}
			if (!tokensLoaded)
			{
				Log.LogError(this, "Unable to load tokens for " + localizer.LanguageString);
			}
		}

		private void onTokensLoaded(bool tokensUpdated)
		{
			tokensLoaded = true;
		}
	}
}
