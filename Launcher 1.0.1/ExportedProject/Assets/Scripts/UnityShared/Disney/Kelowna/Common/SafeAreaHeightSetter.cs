using UnityEngine;

namespace Disney.Kelowna.Common
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class SafeAreaHeightSetter : AbstractSafeAreaComponent
	{
		public SafeArea SafeArea;

		private void Start()
		{
			float verticalOffset = getVerticalOffset(SafeArea);
			RectTransform component = GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, verticalOffset);
		}
	}
}
