using System.Collections;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public class InitCoreServicesAction : InitActionComponent
	{
		public GameObject ServicesContainer;

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
				return false;
			}
		}

		public override IEnumerator PerformFirstPass()
		{
			Service.Set(new EventDispatcher());
			Object.DontDestroyOnLoad(ServicesContainer);
			Service.Set(ServicesContainer);
			GameSettings instance = new GameSettings();
			Service.Set(instance);
			Service.Set((ICommonGameSettings)instance);
			Service.Set((JsonService)new LitJsonService());
			yield break;
		}
	}
}
