using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Analytics
{
	public class LogGameActionButtonInteracted : MonoBehaviour
	{
		[SerializeField]
		private string buttonName = string.Empty;

		[SerializeField]
		private string fromLocation = string.Empty;

		[SerializeField]
		private string toLocation = string.Empty;

		public void OnClicked()
		{
			Service.Get<ICPSwrveService>().NavigationAction(buttonName, fromLocation, toLocation);
		}
	}
}
