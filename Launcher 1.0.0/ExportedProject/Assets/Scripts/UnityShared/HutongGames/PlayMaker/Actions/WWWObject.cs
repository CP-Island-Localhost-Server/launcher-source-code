using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets data from a url and store it in variables. See Unity WWW docs for more details.")]
	[ActionCategory("Web Player")]
	public class WWWObject : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Url to download data from.")]
		public FsmString url;

		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		[Tooltip("Gets text from the url.")]
		public FsmString storeText;

		[UIHint(UIHint.Variable)]
		[Tooltip("Gets a Texture from the url.")]
		public FsmTexture storeTexture;

		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(MovieTexture))]
		[Tooltip("Gets a Texture from the url.")]
		public FsmObject storeMovieTexture;

		[UIHint(UIHint.Variable)]
		[Tooltip("Error message if there was an error during the download.")]
		public FsmString errorString;

		[Tooltip("How far the download progressed (0-1).")]
		[UIHint(UIHint.Variable)]
		public FsmFloat progress;

		[Tooltip("Event to send when the data has finished loading (progress = 1).")]
		[ActionSection("Events")]
		public FsmEvent isDone;

		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;

		private WWW wwwObject;

		public override void Reset()
		{
			url = null;
			storeText = null;
			storeTexture = null;
			errorString = null;
			progress = null;
			isDone = null;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(url.Value))
			{
				Finish();
			}
			else
			{
				wwwObject = new WWW(url.Value);
			}
		}

		public override void OnUpdate()
		{
			if (wwwObject == null)
			{
				errorString.Value = "WWW Object is Null!";
				Finish();
				return;
			}
			errorString.Value = wwwObject.error;
			if (!string.IsNullOrEmpty(wwwObject.error))
			{
				Finish();
				base.Fsm.Event(isError);
				return;
			}
			progress.Value = wwwObject.progress;
			if (progress.Value.Equals(1f))
			{
				storeText.Value = wwwObject.text;
				storeTexture.Value = wwwObject.texture;
				storeMovieTexture.Value = wwwObject.GetMovieTexture();
				errorString.Value = wwwObject.error;
				base.Fsm.Event(string.IsNullOrEmpty(errorString.Value) ? isDone : isError);
				Finish();
			}
		}
	}
}
