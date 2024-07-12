using UnityEngine;

namespace Disney.Kelowna.Common
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	public class SafeAreaCameraViewportSetter : AbstractSafeAreaComponent
	{
		private void Start()
		{
			RectOffset safeAreaOffset = safeAreaService.GetSafeAreaOffset();
			float normalizedVerticalOffset = safeAreaService.GetNormalizedVerticalOffset(safeAreaOffset.top);
			Camera component = GetComponent<Camera>();
			Rect rect = component.rect;
			rect.height = 1f - normalizedVerticalOffset;
			component.rect = rect;
		}
	}
}
