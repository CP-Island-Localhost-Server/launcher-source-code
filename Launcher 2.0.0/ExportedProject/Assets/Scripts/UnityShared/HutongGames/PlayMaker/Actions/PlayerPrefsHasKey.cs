using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns true if key exists in the preferences.")]
	[ActionCategory("PlayerPrefs")]
	public class PlayerPrefsHasKey : FsmStateAction
	{
		[RequiredField]
		public FsmString key;

		[UIHint(UIHint.Variable)]
		[Title("Store Result")]
		public FsmBool variable;

		[Tooltip("Event to send if key exists.")]
		public FsmEvent trueEvent;

		[Tooltip("Event to send if key does not exist.")]
		public FsmEvent falseEvent;

		public override void Reset()
		{
			key = "";
		}

		public override void OnEnter()
		{
			Finish();
			if (!key.IsNone && !key.Value.Equals(""))
			{
				variable.Value = PlayerPrefs.HasKey(key.Value);
			}
			base.Fsm.Event(variable.Value ? trueEvent : falseEvent);
		}
	}
}
