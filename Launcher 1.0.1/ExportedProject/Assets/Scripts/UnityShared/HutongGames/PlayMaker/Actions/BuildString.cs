namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.String)]
	[Tooltip("Builds a String from other Strings.")]
	public class BuildString : FsmStateAction
	{
		[Tooltip("Array of Strings to combine.")]
		[RequiredField]
		public FsmString[] stringParts;

		[Tooltip("Separator to insert between each String. E.g. space character.")]
		public FsmString separator;

		[Tooltip("Add Separator to end of built string.")]
		public FsmBool addToEnd;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the final String in a variable.")]
		[RequiredField]
		public FsmString storeResult;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		private string result;

		public override void Reset()
		{
			stringParts = new FsmString[3];
			separator = null;
			addToEnd = true;
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoBuildString();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoBuildString();
		}

		private void DoBuildString()
		{
			if (storeResult != null)
			{
				result = "";
				for (int i = 0; i < stringParts.Length - 1; i++)
				{
					result += stringParts[i];
					result += separator.Value;
				}
				result += stringParts[stringParts.Length - 1];
				if (addToEnd.Value)
				{
					result += separator.Value;
				}
				storeResult.Value = result;
			}
		}
	}
}
