using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	public class LauncherLoadingBar : MonoBehaviour
	{
		[Serializable]
		private struct LoadingStage
		{
			public float Progress;

			public GameObject Emoji;

			public Color LoadingBarTint;
		}

		[SerializeField]
		private GameObject emojiIcon = null;

		[SerializeField]
		private RectTransform loadingBarTransform = null;

		[SerializeField]
		private Image loadingBarImage = null;

		[SerializeField]
		private RectTransform fillStripes = null;

		[SerializeField]
		private float fillStripesRepeatSeconds = 0f;

		[SerializeField]
		private int loadingStagesCompletedIndex = 0;

		[SerializeField]
		private LoadingStage[] loadingStages = null;

		private RectTransform emojiTransform;

		private RectTransform fillStripesParent;

		private float currentTime;

		private int currentLoadStage = -1;

		private float loadProgress;

		public bool IsLoadingBarAnimating;

		public float LoadProgress
		{
			get
			{
				return loadProgress;
			}
			set
			{
				loadProgress = value;
				updateBar();
			}
		}

		private void OnValidate()
		{
			if (loadingStages != null)
			{
				LoadingStage[] array = loadingStages;
				foreach (LoadingStage loadingStage in array)
				{
				}
			}
		}

		private void Awake()
		{
			emojiTransform = emojiIcon.GetComponent<RectTransform>();
			updateBar();
			fillStripesParent = (RectTransform)fillStripes.parent;
			currentTime = 0f;
		}

		private void Update()
		{
			if (IsLoadingBarAnimating)
			{
				currentTime += Time.deltaTime;
				currentTime %= fillStripesRepeatSeconds;
				float num = currentTime / fillStripesRepeatSeconds;
				Vector2 anchoredPosition = fillStripes.anchoredPosition;
				float x = num * fillStripesParent.rect.width;
				anchoredPosition.x = x;
				fillStripes.anchoredPosition = anchoredPosition;
			}
		}

		public void SetFinished()
		{
			IsLoadingBarAnimating = false;
			setActiveStage(loadingStagesCompletedIndex);
		}

		private void updateBar()
		{
			Vector2 anchorMax = loadingBarTransform.anchorMax;
			anchorMax.x = loadProgress;
			loadingBarTransform.anchorMax = anchorMax;
			Vector2 anchorMin = emojiTransform.anchorMin;
			Vector2 anchorMax2 = emojiTransform.anchorMax;
			anchorMin.x = loadProgress;
			anchorMax2.x = loadProgress;
			emojiTransform.anchorMin = anchorMin;
			emojiTransform.anchorMax = anchorMax2;
			if (loadingStages == null)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < loadingStages.Length; i++)
			{
				LoadingStage loadingStage = loadingStages[i];
				if (loadingStage.Progress <= loadProgress)
				{
					num = i;
				}
			}
			if (num >= 0 && num != currentLoadStage)
			{
				currentLoadStage = num;
				setActiveStage(num);
			}
		}

		private void setActiveStage(int index)
		{
			LoadingStage loadingStage = loadingStages[index];
			loadingStage.Emoji.SetActive(true);
			loadingBarImage.color = loadingStage.LoadingBarTint;
		}
	}
}
