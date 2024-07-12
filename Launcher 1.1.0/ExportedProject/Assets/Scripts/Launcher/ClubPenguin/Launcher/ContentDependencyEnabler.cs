using System.Collections;
using Disney.Kelowna.Common;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class ContentDependencyEnabler : MonoBehaviour
	{
		public GameObject[] ObjectsToEnable;

		private IEnumerator Start()
		{
			while (!Service.IsSet<Content>())
			{
				yield return null;
			}
			for (int i = 0; i < ObjectsToEnable.Length; i++)
			{
				if (ObjectsToEnable[i] != null)
				{
					ObjectsToEnable[i].SetActive(true);
				}
			}
		}
	}
}
