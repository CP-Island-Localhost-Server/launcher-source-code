using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Stops the animator playback mode. When playback stops, the avatar resumes getting control from game logic")]
	[ActionCategory(ActionCategory.Animator)]
	public class AnimatorStopPlayback : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Finish();
				return;
			}
			Animator component = ownerDefaultTarget.GetComponent<Animator>();
			if (component != null)
			{
				component.StopPlayback();
			}
			Finish();
		}
	}
}
