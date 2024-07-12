using ClubPenguin.Analytics;
using DevonLocalization.Core;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(Button))]
	public class LauncherPlayButton : MonoBehaviour
	{
		[LocalizationToken]
		public string PlayToken;

		private Button button;

		private Text text;

		private OpenClientCommand openClientCommand;

		public bool Interactable
		{
			set
			{
				button.interactable = value;
			}
		}

		public string Text
		{
			set
			{
				text.text = value;
			}
		}

		public void SetIsReady()
		{
			Interactable = true;
			Text = Service.Get<Localizer>().GetTokenTranslation(PlayToken);
		}

		private void Awake()
		{
			button = GetComponent<Button>();
			text = GetComponentInChildren<Text>();
		}

		private void OnEnable()
		{
			button.onClick.AddListener(onClicked);
		}

		private void OnDisable()
		{
			button.onClick.RemoveListener(onClicked);
		}

		private void onClicked()
		{
			button.interactable = false;
			Service.Get<ICPSwrveService>().Action("cpi_play", "start");
			LoadingHandler loadingHandler = Service.Get<LoadingHandler>();
			if (loadingHandler == null)
			{
				Log.LogError(this, "LauncherPlayButton: Service.Get<LoadingHandler>() returned null!");
				openClientCommand = new OpenClientCommand();
				openClientCommand.Execute();
				QuitHelper.Quit();
			}
			else
			{
				loadingHandler.RunTheClient();
			}
		}
	}
}
