using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public static class FsmTime
	{
		private static bool firstUpdateHasHappened;

		private static float totalEditorPlayerPausedTime;

		private static float realtimeLastUpdate;

		private static int frameCountLastUpdate;

		public static float RealtimeSinceStartup
		{
			get
			{
				if (firstUpdateHasHappened)
				{
					if (Time.realtimeSinceStartup < totalEditorPlayerPausedTime)
					{
						totalEditorPlayerPausedTime = 0f;
					}
					return Time.realtimeSinceStartup - totalEditorPlayerPausedTime;
				}
				totalEditorPlayerPausedTime = Time.realtimeSinceStartup;
				return 0f;
			}
		}

		public static void RealtimeBugFix()
		{
			firstUpdateHasHappened = true;
		}

		public static void Update()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (Time.frameCount == frameCountLastUpdate)
			{
				totalEditorPlayerPausedTime += realtimeSinceStartup - realtimeLastUpdate;
			}
			frameCountLastUpdate = Time.frameCount;
			realtimeLastUpdate = Time.realtimeSinceStartup;
		}

		public static string FormatTime(float time)
		{
			return new DateTime((long)(time * 10000000f)).ToString("mm:ss:ff");
		}

		public static void DebugLog()
		{
			Debug.Log("LastFrameCount: " + frameCountLastUpdate);
			Debug.Log("PausedTime: " + totalEditorPlayerPausedTime);
			Debug.Log("Realtime: " + RealtimeSinceStartup);
		}
	}
}
