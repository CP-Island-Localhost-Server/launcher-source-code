using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Does tag match the tag of the active state in the statemachine")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorCurrentStateInfoIsTag : FsmStateActionAnimatorBase
	{
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		[Tooltip("The tag to check the layer against.")]
		public FsmString tag;

		[Tooltip("True if tag matches")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmBool tagMatch;

		[Tooltip("Event send if tag matches")]
		public FsmEvent tagMatchEvent;

		[Tooltip("Event send if tag matches")]
		public FsmEvent tagDoNotMatchEvent;

		private Animator _animator;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			layerIndex = null;
			tag = null;
			tagMatch = null;
			tagMatchEvent = null;
			tagDoNotMatchEvent = null;
			everyFrame = false;
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
			IsTag();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			IsTag();
		}

		private void IsTag()
		{
			if (_animator != null)
			{
				if (_animator.GetCurrentAnimatorStateInfo(layerIndex.Value).IsTag(tag.Value))
				{
					tagMatch.Value = true;
					base.Fsm.Event(tagMatchEvent);
				}
				else
				{
					tagMatch.Value = false;
					base.Fsm.Event(tagDoNotMatchEvent);
				}
			}
		}
	}
}
