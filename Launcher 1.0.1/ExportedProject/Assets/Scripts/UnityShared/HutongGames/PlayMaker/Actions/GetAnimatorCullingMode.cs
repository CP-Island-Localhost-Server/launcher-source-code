using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Returns the culling of this Animator component. Optionnaly sends events.\nIf true ('AlwaysAnimate'): always animate the entire character. Object is animated even when offscreen.\nIf False ('BasedOnRenderers') animation is disabled when renderers are not visible.")]
	public class GetAnimatorCullingMode : FsmStateAction
	{
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		[Tooltip("If true, always animate the entire character, else animation is disabled when renderers are not visible")]
		[RequiredField]
		public FsmBool alwaysAnimate;

		[Tooltip("Event send if culling mode is 'AlwaysAnimate'")]
		public FsmEvent alwaysAnimateEvent;

		[Tooltip("Event send if culling mode is 'BasedOnRenders'")]
		public FsmEvent basedOnRenderersEvent;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			alwaysAnimate = null;
			alwaysAnimateEvent = null;
			basedOnRenderersEvent = null;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Finish();
				return;
			}
			_animator = ownerDefaultTarget.GetComponent<Animator>();
			if (_animator == null)
			{
				Finish();
				return;
			}
			DoCheckCulling();
			Finish();
		}

		private void DoCheckCulling()
		{
			if (!(_animator == null))
			{
				bool flag = _animator.cullingMode == AnimatorCullingMode.AlwaysAnimate;
				alwaysAnimate.Value = flag;
				if (flag)
				{
					base.Fsm.Event(alwaysAnimateEvent);
				}
				else
				{
					base.Fsm.Event(basedOnRenderersEvent);
				}
			}
		}
	}
}
