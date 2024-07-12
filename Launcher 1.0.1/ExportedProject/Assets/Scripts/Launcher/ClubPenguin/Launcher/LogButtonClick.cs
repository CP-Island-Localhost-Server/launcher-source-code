using ClubPenguin.Analytics;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.UI;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(Button))]
	public class LogButtonClick : MonoBehaviour
	{
		public string Tier1;

		public string Tier2;

		private void Awake()
		{
			Button component = GetComponent<Button>();
			component.onClick.AddListener(onClicked);
		}

		private void onClicked()
		{
			Service.Get<ICPSwrveService>().Action(Tier1, string.IsNullOrEmpty(Tier2) ? base.name : Tier2);
		}
	}
}
