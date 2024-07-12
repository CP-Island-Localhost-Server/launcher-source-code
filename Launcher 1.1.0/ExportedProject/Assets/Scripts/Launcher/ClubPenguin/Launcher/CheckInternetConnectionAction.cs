using System.Collections;
using DevonLocalization.Core;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(InitCoreServicesAction))]
	[RequireComponent(typeof(InitLocalizerSetupAction))]
	[RequireComponent(typeof(InitPopupManagerAction))]
	public class CheckInternetConnectionAction : InitActionComponent
	{
		public float RetryTime;

		public string Url;

		[LocalizationToken]
		public string ErrorPromptTitleToken;

		[LocalizationToken]
		public string ErrorPromptBodyToken;

		private GameObject popup;

		private bool isConnected;

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
			yield return checkInternetConnection(Url);
			if (!isConnected)
			{
				popup = Service.Get<LauncherPromptManager>().ShowPrompt(ErrorPromptTitleToken, ErrorPromptBodyToken);
			}
			while (!isConnected)
			{
				yield return new WaitForSeconds(RetryTime);
				yield return checkInternetConnection(Url);
			}
			if (popup != null)
			{
				Object.Destroy(popup);
			}
		}

		private IEnumerator checkInternetConnection(string url)
		{
			WWW www = new WWW(url);
			yield return www;
			isConnected = www.error == null;
		}
	}
}
