#define UNITY_ASSERTIONS
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;
using UnityEngine.Assertions;

namespace ClubPenguin.Launcher
{
	public class LauncherInitManagerComponent : InitManagerComponent
	{
		public GameObject ServicesContainer;

		public void Start()
		{
			Assert.IsNotNull(ServicesContainer, "Init manager requires the ServicesContainer game object to be linked from the Scene.");
			Service.Set(ServicesContainer.AddComponent<CoroutineRunner>());
			CoroutineRunner.StartPersistent(init(), this, "InitManagerComponent");
		}
	}
}