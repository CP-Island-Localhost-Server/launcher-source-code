using System.Collections;
using ClubPenguin.Analytics;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class InitLauncherAction : InitActionComponent
	{
		[SerializeField]
		private GameObject[] OnObjects = null;

		[SerializeField]
		private GameObject[] OffObjects = null;

		public override bool HasSecondPass
		{
			get
			{
				return false;
			}
		}

		public override bool HasCompletedPass
		{
			get
			{
				return true;
			}
		}

		public override IEnumerator PerformFirstPass()
		{
			yield break;
		}

		public override void OnInitializationComplete()
		{
			GameObject[] onObjects = OnObjects;
			foreach (GameObject gameObject in onObjects)
			{
				gameObject.SetActive(true);
			}
			onObjects = OffObjects;
			foreach (GameObject gameObject2 in onObjects)
			{
				gameObject2.SetActive(false);
			}
			Service.Get<ICPSwrveService>().EndTimer("init_launcher");
		}
	}
}
