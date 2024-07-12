using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class SiblingDisabler : MonoBehaviour
	{
		private void OnEnable()
		{
			for (int i = 0; i < base.transform.parent.childCount; i++)
			{
				Transform child = base.transform.parent.GetChild(i);
				if (child != base.transform)
				{
					child.gameObject.SetActive(false);
				}
			}
		}
	}
}
