using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("PlayerPrefs")]
	[Tooltip("Sets the value of the preference identified by key.")]
	public class PlayerPrefsSetFloat : FsmStateAction
	{
		[Tooltip("Case sensitive key.")]
		[CompoundArray("Count", "Key", "Value")]
		public FsmString[] keys;

		public FsmFloat[] values;

		public override void Reset()
		{
			keys = new FsmString[1];
			values = new FsmFloat[1];
		}

		public override void OnEnter()
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (!keys[i].IsNone || !keys[i].Value.Equals(""))
				{
					PlayerPrefs.SetFloat(keys[i].Value, values[i].IsNone ? 0f : values[i].Value);
				}
			}
			Finish();
		}
	}
}
