using Disney.Kelowna.Common;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class LauncherPopupManager : MonoBehaviour
	{
		private RectTransform rectTransform;

		private void Start()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		public GameObject ShowPopup(PrefabContentKey popupContentKey)
		{
			GameObject original = Content.LoadImmediate(popupContentKey);
			return Object.Instantiate(original, rectTransform);
		}
	}
}
