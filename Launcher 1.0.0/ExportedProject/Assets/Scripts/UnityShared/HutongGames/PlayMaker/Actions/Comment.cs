namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a text area to the action list. NOTE: Doesn't do anything, just for notes...")]
	[ActionCategory(ActionCategory.Debug)]
	public class Comment : FsmStateAction
	{
		[UIHint(UIHint.Comment)]
		public string comment;

		public override void Reset()
		{
			comment = "";
		}

		public override void OnEnter()
		{
			Finish();
		}
	}
}
