using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Gets the Left n characters from a String Variable.")]
	public class GetStringLeft : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString stringVariable;

		[Tooltip("Number of characters to get.")]
		public FsmInt charCount;

		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			stringVariable = null;
			charCount = 0;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetStringLeft();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetStringLeft();
		}

		private void DoGetStringLeft()
		{
			if (!stringVariable.IsNone && !storeResult.IsNone)
			{
				storeResult.Value = stringVariable.Value.Substring(0, Mathf.Clamp(charCount.Value, 0, stringVariable.Value.Length));
			}
		}
	}
}
