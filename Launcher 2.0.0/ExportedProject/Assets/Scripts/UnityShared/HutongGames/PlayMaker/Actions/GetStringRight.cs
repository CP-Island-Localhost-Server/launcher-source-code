using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the Right n characters from a String.")]
	[ActionCategory(ActionCategory.String)]
	public class GetStringRight : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
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
			DoGetStringRight();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetStringRight();
		}

		private void DoGetStringRight()
		{
			if (!stringVariable.IsNone && !storeResult.IsNone)
			{
				string value = stringVariable.Value;
				int num = Mathf.Clamp(charCount.Value, 0, value.Length);
				storeResult.Value = value.Substring(value.Length - num, num);
			}
		}
	}
}
