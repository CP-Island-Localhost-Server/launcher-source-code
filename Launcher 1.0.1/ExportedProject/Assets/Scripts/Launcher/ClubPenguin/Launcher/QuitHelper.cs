using System.Collections;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public static class QuitHelper
	{
		public static void Quit()
		{
			Service.Get<EventDispatcher>().DispatchEvent(default(AppWillQuitEvent));
			CoroutineRunner.StartPersistent(delayQuit(), typeof(QuitHelper), "QuitHelper.delayQuit");
		}

		private static IEnumerator delayQuit()
		{
			yield return null;
			yield return null;
			Application.Quit();
		}
	}
}
