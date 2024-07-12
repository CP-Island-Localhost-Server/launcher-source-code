using DevonLocalization.Core;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class AppWindowListener : MonoBehaviour
	{
		[LocalizationToken]
		public string DefaultTitleToken;

		[LocalizationToken]
		public string DefaultToken;

		[LocalizationToken]
		public string LauncherActionsRunningTitleToken;

		[LocalizationToken]
		public string LauncherActionsRunningToken;

		private bool isShowingPrompt;

		private bool isQuiting = false;

		private void Start()
		{
			Service.Get<EventDispatcher>().AddListener<AppWindowUtilEvents.WindowCloseClickedEvent>(onWindowCloseClicked);
		}

		private bool onWindowCloseClicked(AppWindowUtilEvents.WindowCloseClickedEvent evt)
		{
			showQuitPrompt();
			return true;
		}

		private void OnApplicationQuit()
		{
			Debug.LogWarning("AppWindowListener OnApplicationQuit()");
			if (showQuitPrompt() || !isQuiting)
			{
				Application.CancelQuit();
			}
		}

		private bool showQuitPrompt()
		{
			if (!isShowingPrompt)
			{
				isShowingPrompt = true;
				string titleToken;
				string bodyToken;
				if (Service.IsSet<LoadingHandler>() && Service.Get<LoadingHandler>().IsRunningLauncherActions)
				{
					titleToken = LauncherActionsRunningTitleToken;
					bodyToken = LauncherActionsRunningToken;
				}
				else
				{
					titleToken = DefaultTitleToken;
					bodyToken = DefaultToken;
				}
				Service.Get<LauncherPromptManager>().ShowPrompt(titleToken, bodyToken, ButtonFlags.No | ButtonFlags.Yes, onPromptButtonClicked);
				return true;
			}
			return false;
		}

		private void onPromptButtonClicked(ButtonFlags clickedButtonFlag)
		{
			if (clickedButtonFlag == ButtonFlags.Yes)
			{
				isQuiting = true;
				QuitHelper.Quit();
			}
			else
			{
				isShowingPrompt = false;
			}
		}
	}
}
