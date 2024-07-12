using DevonLocalization.Core;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	public class LauncherPrompt : MonoBehaviour
	{
		public Text TitleText;

		public Text BodyText;

		public RectTransform ButtonParent;

		public ButtonDefinition[] Buttons;

		private Localizer localizer;

		public void Show(PromptData promptData)
		{
			localizer = Service.Get<Localizer>();
			showToken(TitleText, promptData.TitleToken, promptData.IsTitleTranslated);
			showToken(BodyText, promptData.BodyToken, promptData.IsBodyTranslated);
			for (int i = 0; i < Buttons.Length; i++)
			{
				ButtonFlags buttonFlag = (ButtonFlags)(1 << i);
				if ((promptData.ButtonFlags & buttonFlag) != buttonFlag)
				{
					continue;
				}
				Button button = Object.Instantiate(Buttons[i].ButtonPrefab, ButtonParent);
				if (promptData.OnButtonClicked != null)
				{
					button.onClick.AddListener(delegate
					{
						promptData.OnButtonClicked(buttonFlag);
					});
				}
				button.onClick.AddListener(destroyPrompt);
				if (!string.IsNullOrEmpty(Buttons[i].ButtonToken))
				{
					Text componentInChildren = button.GetComponentInChildren<Text>();
					if (componentInChildren != null)
					{
						showToken(componentInChildren, Buttons[i].ButtonToken, false);
						continue;
					}
					Log.LogErrorFormatted(this, "No text component in prefab {0} for button flag {1}", button.name, buttonFlag);
				}
			}
		}

		private void showToken(Text textComponent, string token, bool isTokenTranslated)
		{
			if (!string.IsNullOrEmpty(token))
			{
				if (isTokenTranslated)
				{
					textComponent.text = token;
				}
				else
				{
					textComponent.text = localizer.GetTokenTranslation(token);
				}
			}
			else
			{
				textComponent.text = "";
			}
		}

		private void destroyPrompt()
		{
			Object.Destroy(base.gameObject);
		}
	}
}
