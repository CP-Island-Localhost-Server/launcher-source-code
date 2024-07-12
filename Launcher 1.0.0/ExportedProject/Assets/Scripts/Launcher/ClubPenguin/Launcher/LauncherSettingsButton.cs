using Disney.Kelowna.Common;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(Button))]
	public class LauncherSettingsButton : MonoBehaviour
	{
		public PrefabContentKey SettingsContentKey;

		private void Start()
		{
			Button component = GetComponent<Button>();
			component.onClick.AddListener(onClicked);
		}

		private void onClicked()
		{
			Service.Get<LauncherPopupManager>().ShowPopup(SettingsContentKey);
		}
	}
}
