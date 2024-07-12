using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Removes key and its corresponding value from the preferences.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsDeleteKey : FsmStateAction
	{
		public FsmString key;

		public override void Reset()
		{
			key = "";
		}

		public override void OnEnter()
		{
			if (!key.IsNone && !key.Value.Equals(""))
			{
				PlayerPrefs.DeleteKey(key.Value);
			}
			Finish();
		}
	}
}
