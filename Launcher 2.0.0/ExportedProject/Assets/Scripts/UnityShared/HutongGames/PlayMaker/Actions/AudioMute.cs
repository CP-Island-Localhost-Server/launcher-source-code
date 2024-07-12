using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Mute/unmute the Audio Clip played by an Audio Source component on a Game Object.")]
	[ActionCategory(ActionCategory.Audio)]
	public class AudioMute : FsmStateAction
	{
		[Tooltip("The GameObject with an Audio Source component.")]
		[RequiredField]
		[CheckForComponent(typeof(AudioSource))]
		public FsmOwnerDefault gameObject;

		[Tooltip("Check to mute, uncheck to unmute.")]
		[RequiredField]
		public FsmBool mute;

		public override void Reset()
		{
			gameObject = null;
			mute = false;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget != null)
			{
				AudioSource component = ownerDefaultTarget.GetComponent<AudioSource>();
				if (component != null)
				{
					component.mute = mute.Value;
				}
			}
			Finish();
		}
	}
}
