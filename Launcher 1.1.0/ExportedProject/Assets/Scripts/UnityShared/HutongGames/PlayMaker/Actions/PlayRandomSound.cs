using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Plays a Random Audio Clip at a position defined by a Game Object or a Vector3. If a position is defined, it takes priority over the game object. You can set the relative weight of the clips to control how often they are selected.")]
	[ActionCategory(ActionCategory.Audio)]
	public class PlayRandomSound : FsmStateAction
	{
		public FsmOwnerDefault gameObject;

		public FsmVector3 position;

		[CompoundArray("Audio Clips", "Audio Clip", "Weight")]
		public AudioClip[] audioClips;

		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		[HasFloatSlider(0f, 1f)]
		public FsmFloat volume = 1f;

		public override void Reset()
		{
			gameObject = null;
			position = new FsmVector3
			{
				UseVariable = true
			};
			audioClips = new AudioClip[3];
			weights = new FsmFloat[3] { 1f, 1f, 1f };
			volume = 1f;
		}

		public override void OnEnter()
		{
			DoPlayRandomClip();
			Finish();
		}

		private void DoPlayRandomClip()
		{
			if (audioClips.Length == 0)
			{
				return;
			}
			int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(weights);
			if (randomWeightedIndex == -1)
			{
				return;
			}
			AudioClip audioClip = audioClips[randomWeightedIndex];
			if (!(audioClip != null))
			{
				return;
			}
			if (!position.IsNone)
			{
				AudioSource.PlayClipAtPoint(audioClip, position.Value, volume.Value);
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null))
			{
				AudioSource.PlayClipAtPoint(audioClip, ownerDefaultTarget.transform.position, volume.Value);
			}
		}
	}
}
