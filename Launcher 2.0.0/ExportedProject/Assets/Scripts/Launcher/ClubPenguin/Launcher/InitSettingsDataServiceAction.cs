using System.Collections;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;

namespace ClubPenguin.Launcher
{
	public class InitSettingsDataServiceAction : InitActionComponent
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
			Service.Set(new SettingsDataService());
			yield break;
		}
	}
}
