using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(Button))]
	public class LauncherSettingsCloseButton : MonoBehaviour
	{
		public string ParentName;

		private Transform settings;

		private void Start()
		{
			settings = base.transform.FindParent(ParentName);
			if (settings != null)
			{
				Button component = GetComponent<Button>();
				component.onClick.AddListener(onClicked);
			}
			else
			{
				Log.LogError(this, "Could not find a parent transform named " + ParentName);
			}
		}

		private void onClicked()
		{
			Object.Destroy(settings.gameObject);
		}
	}
}
