using System;
using System.Collections;
using UnityEngine;

namespace ClubPenguin.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class SliderTweener : MonoBehaviour
	{
		public bool StartsVisible;

		public float SlideInSeconds;

		public float SlideOutSeconds;

		public iTween.EaseType SlideInEasing;

		public iTween.EaseType SlideOutEasing;

		private float visibilePosition;

		private float readyPosition;

		private float outPosition;

		protected bool isInit;

		private float totalSlideInRange;

		private float totalSlideOutRange;

		private Hashtable slideInParams;

		private Hashtable slideOutParams;

		private float currentPosition;

		private RectTransform Content;

		[HideInInspector]
		public bool IsTransitioning;

		private string slideInTweenNameTag = "slideIn";

		private string slideOutTweenNameTag = "slideOut";

		public bool IsVisible { get; private set; }

		public event Action OnComplete;

		public event Action<float> OnPositionChanged;

		private void Start()
		{
			Debug.Log("Running Start() on " + base.gameObject.name + " with SetVisible=" + StartsVisible);
			Content = base.transform as RectTransform;
			StartCoroutine(waitForLayout());
		}

		private IEnumerator waitForLayout()
		{
			while (Math.Abs(Content.rect.width) < float.Epsilon)
			{
				yield return null;
			}
			float visiblePosition = 0f;
			float readyPosition = 0f + Content.rect.width;
			float outPosition = 0f - Content.rect.width;
			Init(visiblePosition, readyPosition, outPosition);
			if (StartsVisible)
			{
				SetVisible();
			}
			else
			{
				SetReady();
			}
			Content.gameObject.SetActive(true);
		}

		private void setPosition(float value)
		{
			Vector2 anchoredPosition = Content.anchoredPosition;
			anchoredPosition.x = value;
			Content.anchoredPosition = anchoredPosition;
		}

		public void Init(float visibilePosition, float readyPosition, float outPosition)
		{
			Debug.Log("Running Init() on " + base.gameObject.name + " with visibilePosition=" + visibilePosition + " readyPosition=" + readyPosition + " outPosition=" + outPosition);
			slideInTweenNameTag += Guid.NewGuid().ToString();
			slideOutTweenNameTag += Guid.NewGuid().ToString();
			this.visibilePosition = visibilePosition;
			this.readyPosition = readyPosition;
			this.outPosition = outPosition;
			totalSlideInRange = visibilePosition - readyPosition;
			totalSlideOutRange = outPosition - visibilePosition;
			slideInParams = createTweenHash(readyPosition, visibilePosition, SlideInSeconds, SlideInEasing, slideInTweenNameTag);
			slideOutParams = createTweenHash(visibilePosition, outPosition, SlideOutSeconds, SlideOutEasing, slideOutTweenNameTag);
			isInit = true;
		}

		public void SlideIn()
		{
			if (base.gameObject.IsDestroyed())
			{
				return;
			}
			if (!IsTransitioning)
			{
				IsTransitioning = true;
				if (IsVisible && Math.Abs(currentPosition - visibilePosition) > float.Epsilon)
				{
					Hashtable args = createTransitionHash(currentPosition, visibilePosition, totalSlideInRange, SlideInSeconds, SlideInEasing, slideInTweenNameTag);
					iTween.ValueTo(base.gameObject, args);
				}
				else
				{
					iTween.ValueTo(base.gameObject, slideInParams);
				}
			}
			else
			{
				Hashtable args = createTransitionHash(currentPosition, visibilePosition, totalSlideInRange, SlideInSeconds, SlideInEasing, slideInTweenNameTag);
				iTween.StopByName(slideInTweenNameTag);
				iTween.StopByName(slideOutTweenNameTag);
				iTween.ValueTo(base.gameObject, args);
			}
			IsVisible = true;
		}

		public void SlideOut()
		{
			if (!IsTransitioning)
			{
				IsTransitioning = true;
				if (!IsVisible && Math.Abs(currentPosition - outPosition) > float.Epsilon)
				{
					Hashtable args = createTransitionHash(currentPosition, outPosition, totalSlideOutRange, SlideOutSeconds, SlideOutEasing, slideOutTweenNameTag);
					iTween.ValueTo(base.gameObject, args);
				}
				else
				{
					iTween.ValueTo(base.gameObject, slideOutParams);
				}
			}
			else
			{
				Hashtable args = createTransitionHash(currentPosition, outPosition, totalSlideOutRange, SlideOutSeconds, SlideOutEasing, slideOutTweenNameTag);
				iTween.StopByName(slideOutTweenNameTag);
				iTween.StopByName(slideInTweenNameTag);
				iTween.ValueTo(base.gameObject, args);
			}
			IsVisible = false;
		}

		public void SetVisible()
		{
			iTween.Stop(base.gameObject);
			updatePosition(visibilePosition);
			IsVisible = true;
			onComplete();
		}

		public void SetReady()
		{
			iTween.Stop(base.gameObject);
			updatePosition(readyPosition);
			IsVisible = false;
			onComplete();
		}

		private Hashtable createTweenHash(float from, float to, float seconds, iTween.EaseType easing, string tweenName)
		{
			return iTween.Hash("from", from, "to", to, "time", seconds, "onupdate", "updatePosition", "oncomplete", "onComplete", "easetype", easing, "name", tweenName);
		}

		private Hashtable createTransitionHash(float from, float to, float range, float seconds, iTween.EaseType easing, string tweenName)
		{
			float num = to - from;
			float seconds2 = Mathf.Abs(num / range * seconds);
			return createTweenHash(from, to, seconds2, easing, tweenName);
		}

		private void updatePosition(float value)
		{
			currentPosition = value;
			setPosition(value);
		}

		private void onComplete()
		{
			IsTransitioning = false;
			if (this.OnComplete != null)
			{
				this.OnComplete();
			}
		}

		private void OnDestroy()
		{
			iTween.Stop(base.gameObject);
			this.OnPositionChanged = null;
		}
	}
}
