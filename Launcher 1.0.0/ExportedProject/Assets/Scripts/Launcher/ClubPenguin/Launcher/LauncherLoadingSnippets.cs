using System;
using System.Collections;
using System.Collections.Generic;
using DevonLocalization.Core;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	public class LauncherLoadingSnippets : MonoBehaviour
	{
		public Text SnippetText;

		public float SnippetDisplaySeconds;

		public string[] LoadingSnippetTokens;

		private Coroutine runDisplayTimerCoroutine;

		private List<string> loadingSnippetTokensList;

		private string currentSnippetToken;

		public void Show()
		{
			SnippetText.gameObject.SetActive(true);
			runDisplayTimerCoroutine = StartCoroutine(runDisplayTimer());
		}

		public void Hide()
		{
			SnippetText.gameObject.SetActive(false);
			if (runDisplayTimerCoroutine != null)
			{
				StopCoroutine(runDisplayTimerCoroutine);
			}
		}

		private IEnumerator runDisplayTimer()
		{
			System.Random random = new System.Random();
			while (true)
			{
				if (loadingSnippetTokensList == null || loadingSnippetTokensList.Count == 0)
				{
					loadingSnippetTokensList = new List<string>(LoadingSnippetTokens);
				}
				int randomIndex;
				string loadingSnippetToken;
				do
				{
					randomIndex = random.Next(0, loadingSnippetTokensList.Count);
					loadingSnippetToken = loadingSnippetTokensList[randomIndex];
				}
				while (currentSnippetToken == loadingSnippetToken);
				loadingSnippetTokensList.RemoveAt(randomIndex);
				currentSnippetToken = loadingSnippetToken;
				string loadingSnippet = Service.Get<Localizer>().GetTokenTranslation(loadingSnippetToken);
				SnippetText.text = loadingSnippet;
				yield return new WaitForSeconds(SnippetDisplaySeconds);
			}
		}
	}
}
