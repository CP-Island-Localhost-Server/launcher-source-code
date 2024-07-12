using System.Collections;
using Disney.Kelowna.Common;
using Disney.LaunchPadFramework;
using Disney.MobileNetwork;
using UnityEngine;

namespace ClubPenguin.Launcher
{
	public static class QuitHelper
	{
		private static bool isQuitting = false;

		public static void Quit()
		{
			if (!isQuitting)
			{
				isQuitting = true;
				EventDispatcher eventDispatcher = Service.Get<EventDispatcher>();
				if (eventDispatcher != null)
				{
					eventDispatcher.DispatchEvent(default(AppWillQuitEvent));
				}
				CoroutineRunner.StartPersistent(delayQuit(), typeof(QuitHelper), "QuitHelper.delayQuit");
			}
		}

		private static IEnumerator delayQuit()
		{
			yield return null;
			yield return null;
			AppWindowListener.QuitWithoutPrompt = true;
			yield return new WaitForEndOfFrame();
			Application.Quit();
		}
	}
}
