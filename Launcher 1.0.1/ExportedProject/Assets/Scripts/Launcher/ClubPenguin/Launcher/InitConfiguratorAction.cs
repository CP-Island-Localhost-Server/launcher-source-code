using System.Collections;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	[RequireComponent(typeof(InitCoreServicesAction))]
	public class InitConfiguratorAction : InitActionComponent
	{
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
			Configurator configurator = new Configurator();
			configurator.Init(false);
			Service.Set(configurator);
			LogConfigurator.Setup(configurator);
			yield break;
		}
	}
}
