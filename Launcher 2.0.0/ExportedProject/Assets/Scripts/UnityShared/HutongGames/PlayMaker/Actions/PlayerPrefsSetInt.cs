using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of the preference identified by key.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsSetInt : FsmStateAction
	{
		[Tooltip("Case sensitive key.")]
		[CompoundArray("Count", "Key", "Value")]
		public FsmString[] keys;

		public FsmInt[] values;

		public override void Reset()
		{
			keys = new FsmString[1];
			values = new FsmInt[1];
		}

		public override void OnEnter()
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (!keys[i].IsNone || !keys[i].Value.Equals(""))
				{
					PlayerPrefs.SetInt(keys[i].Value, (!values[i].IsNone) ? values[i].Value : 0);
				}
			}
			Finish();
		}
	}
}
