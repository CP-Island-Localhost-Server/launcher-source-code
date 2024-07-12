using System;
using System.Collections.Generic;
using System.Linq;
using ClubPenguin.Analytics;
using ClubPenguin.UI;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class LauncherCards : MonoBehaviour
	{
		public GameObject LauncherCardsContainer;

		public LauncherCardPrefabController LauncherCardPrefab;

		private float changeTime = 5f;

		private int currentSlide = 0;

		private float timeSinceLast = 0f;

		private SliderTweener[] slides;

		private void Start()
		{
			Service.Get<InstallerManifestService>().OnInstallerManifestUpdated += onInstallerManifestUpdated;
			if (slides == null)
			{
				ResetSlides();
			}
		}

		private void onInstallerManifestUpdated()
		{
		}

		public bool NeedsUpdate()
		{
			InstallerManifestEntry latestLauncherEntry = Service.Get<InstallerManifestService>().GetLatestLauncherEntry();
			if (latestLauncherEntry != null)
			{
				Version version = new Version(Application.version);
				return version < latestLauncherEntry.GetVersion();
			}
			return false;
		}

		private void Update()
		{
			if (timeSinceLast > changeTime && slides.Length > 1)
			{
				int num = (currentSlide + 1) % slides.Length;
				LauncherCardPrefabController component = slides[num].gameObject.GetComponent<LauncherCardPrefabController>();
				slides[num].gameObject.SetActive(true);
				slides[num].StartsVisible = false;
				slides[num].SlideIn();
				slides[currentSlide].SlideOut();
				currentSlide = num;
				changeTime = component.Duration;
				CacheableType<string> viewedSlides = Service.Get<GameSettings>().ViewedSlides;
				viewedSlides.Value = viewedSlides.Value + "," + component.Id;
				Service.Get<ICPSwrveService>().Action("launcher_cards", component.Id);
				timeSinceLast = 0f;
			}
			timeSinceLast += Time.deltaTime;
		}

		private void ResetSlides(int initialCurrentSlide = 0)
		{
			slides = LauncherCardsContainer.GetComponentsInChildren<SliderTweener>(true);
			if (slides.Length > 0)
			{
				currentSlide = initialCurrentSlide % slides.Length;
				slides[currentSlide].StartsVisible = true;
				LauncherCardPrefabController component = slides[currentSlide].gameObject.GetComponent<LauncherCardPrefabController>();
				CacheableType<string> viewedSlides = Service.Get<GameSettings>().ViewedSlides;
				viewedSlides.Value = viewedSlides.Value + "," + component.Id;
				Service.Get<ICPSwrveService>().Action("launcher_cards", component.Id);
			}
		}

		public void InitializeCards(CardsResponse cardsResponse)
		{
			if (cardsResponse.cards == null)
			{
				return;
			}
			string value = Service.Get<GameSettings>().ViewedSlides.Value;
			List<string> list = (string.IsNullOrEmpty(value) ? new List<string>() : value.Split(',').ToList());
			int num = ((list.Count > 0) ? 1 : 0);
			bool flag = false;
			foreach (Card card in cardsResponse.cards)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(LauncherCardPrefab.gameObject, LauncherCardsContainer.transform);
				if (!flag && list.Contains(card.id))
				{
					num++;
				}
				else
				{
					flag = true;
				}
				LauncherCardPrefabController component = gameObject.GetComponent<LauncherCardPrefabController>();
				component.Id = card.id;
				component.MainImageURL = card.img;
				component.Title.text = card.title;
				component.Body.text = card.copy;
				try
				{
					component.Duration = int.Parse(card.duration);
				}
				catch
				{
					Log.LogError(this, "Unable to parse card.duration " + card.duration);
				}
				try
				{
					component.SetBackground(int.Parse(card.theme));
				}
				catch
				{
					Log.LogError(this, "Unable to parse card.theme " + card.theme);
				}
				SliderTweener component2 = gameObject.GetComponent<SliderTweener>();
				component2.StartsVisible = false;
			}
			ResetSlides(num);
		}

		private void OnDestroy()
		{
			Service.Get<InstallerManifestService>().OnInstallerManifestUpdated -= onInstallerManifestUpdated;
		}
	}
}
