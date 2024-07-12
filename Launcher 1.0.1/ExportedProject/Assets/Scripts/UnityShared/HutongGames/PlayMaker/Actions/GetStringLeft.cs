using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Left n characters from a String Variable.")]
	[ActionCategory(ActionCategory.String)]
	public class GetStringLeft : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString stringVariable;

		[Tooltip("Number of characters to get.")]
		public FsmInt charCount;

		[RequiredField]
		[UIHint(UIHint.Variable)]
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
