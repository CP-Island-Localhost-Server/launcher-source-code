using UnityEngine;

namespace HutongGames.PlayMaker
{
	public class FsmDebugUtility
	{
		public static void Log(Fsm fsm, string text, bool frameCount = false)
		{
			if (fsm.GameObject != null)
			{
				text = text + " : " + fsm.GameObject.name + " : " + fsm.Name;
			}
			Log(text, frameCount);
		}

		public static void Log(string text, bool frameCount = false)
		{
			if (frameCount)
			{
				text = Time.frameCount + " : " + text;
			}
			Debug.Log(text);
		}

		public static void Log(Object obj, string text)
		{
			text = obj.name + " : " + text;
			Log(text);
		}
	}
}
