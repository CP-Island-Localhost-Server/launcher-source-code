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
		private const string CMD_BRING_OTHER_TO_FRONT = "BringOtherToFront";

		private const string CMD_CLIENT_IS_RUNNING = "ClientIsRunning";

		private const int BRING_OTHER_TO_FRONT_TIMEOUT = 500;

		public GameObject ServicesContainer;

		public void Start()
		{
			Assert.IsNotNull(ServicesContainer, "Init manager requires the ServicesContainer game object to be linked from the Scene.");
			Service.Set(ServicesContainer.AddComponent<CoroutineRunner>());
			SingleInstanceApplication singleInstanceApplication = SingleInstanceApplication.CreateServerInstance("MUTEX_620CCF48-01A3-453C-A5ED-C18A8D1724E6");
			if (singleInstanceApplication.IsAnotherProcessRunning())
			{
				QuitHelper.Quit();
				return;
			}
			Service.Set(singleInstanceApplication);
			CoroutineRunner.StartPersistent(init(), this, "InitManagerComponent");
		}
	}
}
