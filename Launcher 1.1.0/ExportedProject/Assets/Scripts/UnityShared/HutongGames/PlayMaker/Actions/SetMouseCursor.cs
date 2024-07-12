namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Controls the appearance of Mouse Cursor.")]
	[ActionCategory(ActionCategory.GUI)]
	public class SetMouseCursor : FsmStateAction
	{
		public FsmTexture cursorTexture;

		public FsmBool hideCursor;

		public FsmBool lockCursor;

		public override void Reset()
		{
			cursorTexture = null;
			hideCursor = false;
			lockCursor = false;
		}

		public override void OnEnter()
		{
			PlayMakerGUI.LockCursor = lockCursor.Value;
			PlayMakerGUI.HideCursor = hideCursor.Value;
			PlayMakerGUI.MouseCursor = cursorTexture.Value;
			Finish();
		}
	}
}
