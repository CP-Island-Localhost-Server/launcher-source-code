namespace ClubPenguin.Launcher
{
	public class PromptData
	{
		public string TitleToken;

		public string BodyToken;

		public bool IsTitleTranslated;

		public bool IsBodyTranslated;

		public ButtonFlags ButtonFlags;

		public OnButtonClickedDelegate OnButtonClicked;

		public PromptData(string titleToken, string bodyToken, ButtonFlags buttonFlags, OnButtonClickedDelegate onButtonClicked)
		{
			TitleToken = titleToken;
			BodyToken = bodyToken;
			ButtonFlags = buttonFlags;
			OnButtonClicked = onButtonClicked;
		}
	}
}
