using System;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Replaces each format item in a specified string with the text equivalent of variable's value. Stores the result in a string variable.")]
	[ActionCategory(ActionCategory.String)]
	public class FormatString : FsmStateAction
	{
		[Tooltip("E.g. Hello {0} and {1}\nWith 2 variables that replace {0} and {1}\nSee C# string.Format docs.")]
		[RequiredField]
		public FsmString format;

		[Tooltip("Variables to use for each formatting item.")]
		public FsmVar[] variables;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the formatted result in a string variable.")]
		public FsmString storeResult;

		[Tooltip("Repeat every frame. This is useful if the variables are changing.")]
		public bool everyFrame;

		private object[] objectArray;

		public override void Reset()
		{
			format = null;
			variables = null;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			objectArray = new object[variables.Length];
			DoFormatString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFormatString();
		}

		private void DoFormatString()
		{
			for (int i = 0; i < variables.Length; i++)
			{
				variables[i].UpdateValue();
				objectArray[i] = variables[i].GetValue();
			}
			try
			{
				storeResult.Value = string.Format(format.Value, objectArray);
			}
			catch (FormatException ex)
			{
				LogError(ex.Message);
				Finish();
			}
		}
	}
}
