using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the Game Object as the Audio Source associated with the Movie Texture. The Game Object must have an AudioSource Component.")]
	[ActionCategory(ActionCategory.Movie)]
	public class MovieTextureAudioSettings : FsmStateAction
	{
		[RequiredField]
		[ObjectType(typeof(MovieTexture))]
		public FsmObject movieTexture;

		[CheckForComponent(typeof(AudioSource))]
		[RequiredField]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			movieTexture = null;
			gameObject = null;
		}

		public override void OnEnter()
		{
			MovieTexture movieTexture = this.movieTexture.Value as MovieTexture;
			if (movieTexture != null && gameObject.Value != null)
			{
				AudioSource component = gameObject.Value.GetComponent<AudioSource>();
				if (component != null)
				{
					component.clip = movieTexture.audioClip;
				}
			}
			Finish();
		}
	}
}
