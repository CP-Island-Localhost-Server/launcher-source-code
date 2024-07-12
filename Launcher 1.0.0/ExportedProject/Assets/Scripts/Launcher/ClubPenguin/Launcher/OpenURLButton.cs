using DevonLocalization.Core;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(Button))]
	public class OpenURLButton : MonoBehaviour
	{
		[LocalizationToken]
		[SerializeField]
		private string urlToken = string.Empty;

		private Button button;

		private void OnValidate()
		{
		}

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		private void OnEnable()
		{
			button.onClick.AddListener(onClicked);
		}

		private void OnDisable()
		{
			button.onClick.RemoveListener(onClicked);
		}

		public void OpenURL()
		{
			Application.OpenURL(Service.Get<Localizer>().GetTokenTranslation(urlToken));
		}

		private void onClicked()
		{
			OpenURL();
		}
	}
}
