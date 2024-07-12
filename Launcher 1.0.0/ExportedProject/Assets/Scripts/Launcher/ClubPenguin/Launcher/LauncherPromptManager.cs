using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class LauncherPromptManager : MonoBehaviour
	{
		public LauncherPrompt PromptPrefab;

		public LauncherPrompt ErrorPrefab;

		public GameObject ShowPrompt(string titleToken, string bodyToken, ButtonFlags buttonFlags = ButtonFlags.None, OnButtonClickedDelegate onButtonClicked = null)
		{
			return show(PromptPrefab, titleToken, bodyToken, buttonFlags, onButtonClicked);
		}

		public GameObject ShowPrompt(PromptData promptData)
		{
			return show(PromptPrefab, promptData);
		}

		public GameObject ShowError(string titleToken, string bodyToken, ButtonFlags buttonFlags = ButtonFlags.None, OnButtonClickedDelegate onButtonClicked = null)
		{
			return show(ErrorPrefab, titleToken, bodyToken, buttonFlags, onButtonClicked);
		}

		public GameObject ShowError(PromptData promptData)
		{
			return show(ErrorPrefab, promptData);
		}

		private GameObject show(LauncherPrompt launcherPromptPrefab, PromptData promptData)
		{
			LauncherPrompt launcherPrompt = Object.Instantiate(launcherPromptPrefab, base.transform);
			launcherPrompt.Show(promptData);
			return launcherPrompt.gameObject;
		}

		private GameObject show(LauncherPrompt launcherPromptPrefab, string titleToken, string bodyToken, ButtonFlags buttonFlags, OnButtonClickedDelegate onButtonClicked)
		{
			PromptData promptData = new PromptData(titleToken, bodyToken, buttonFlags, onButtonClicked);
			promptData.IsTitleTranslated = false;
			promptData.IsBodyTranslated = false;
			LauncherPrompt launcherPrompt = Object.Instantiate(launcherPromptPrefab, base.transform);
			launcherPrompt.Show(promptData);
			return launcherPrompt.gameObject;
		}
	}
}
