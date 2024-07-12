using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the next State information on a specified layer")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorNextStateInfo : FsmStateActionAnimatorBase
	{
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The layer's index")]
		[RequiredField]
		public FsmInt layerIndex;

		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's name.")]
		public FsmString name;

		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's name Hash. Obsolete in Unity 5, use fullPathHash or shortPathHash instead, nameHash will be the same as shortNameHash for legacy")]
		public FsmInt nameHash;

		[Tooltip("The full path hash for this state.")]
		[UIHint(UIHint.Variable)]
		public FsmInt fullPathHash;

		[UIHint(UIHint.Variable)]
		[Tooltip("The name Hash. Doest not include the parent layer's name")]
		public FsmInt shortPathHash;

		[Tooltip("The layer's tag hash")]
		[UIHint(UIHint.Variable)]
		public FsmInt tagHash;

		[Tooltip("Is the state looping. All animations in the state must be looping")]
		[UIHint(UIHint.Variable)]
		public FsmBool isStateLooping;

		[UIHint(UIHint.Variable)]
		[Tooltip("The Current duration of the state. In seconds, can vary when the State contains a Blend Tree ")]
		public FsmFloat length;

		[Tooltip("The integer part is the number of time a state has been looped. The fractional part is the % (0-1) of progress in the current loop")]
		[UIHint(UIHint.Variable)]
		public FsmFloat normalizedTime;

		[Tooltip("The integer part is the number of time a state has been looped. This is extracted from the normalizedTime")]
		[UIHint(UIHint.Variable)]
		public FsmInt loopCount;

		[UIHint(UIHint.Variable)]
		[Tooltip("The progress in the current loop. This is extracted from the normalizedTime")]
		public FsmFloat currentLoopProgress;

		private Animator _animator;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			layerIndex = null;
			name = null;
			nameHash = null;
			fullPathHash = null;
			shortPathHash = null;
			tagHash = null;
			length = null;
			normalizedTime = null;
			isStateLooping = null;
			loopCount = null;
			currentLoopProgress = null;
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
			GetLayerInfo();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			GetLayerInfo();
		}

		private void GetLayerInfo()
		{
			if (_animator != null)
			{
				AnimatorStateInfo nextAnimatorStateInfo = _animator.GetNextAnimatorStateInfo(layerIndex.Value);
				if (!fullPathHash.IsNone)
				{
					fullPathHash.Value = nextAnimatorStateInfo.fullPathHash;
				}
				if (!shortPathHash.IsNone)
				{
					shortPathHash.Value = nextAnimatorStateInfo.shortNameHash;
				}
				if (!nameHash.IsNone)
				{
					nameHash.Value = nextAnimatorStateInfo.shortNameHash;
				}
				if (!name.IsNone)
				{
					name.Value = _animator.GetLayerName(layerIndex.Value);
				}
				if (!tagHash.IsNone)
				{
					tagHash.Value = nextAnimatorStateInfo.tagHash;
				}
				if (!length.IsNone)
				{
					length.Value = nextAnimatorStateInfo.length;
				}
				if (!isStateLooping.IsNone)
				{
					isStateLooping.Value = nextAnimatorStateInfo.loop;
				}
				if (!normalizedTime.IsNone)
				{
					normalizedTime.Value = nextAnimatorStateInfo.normalizedTime;
				}
				if (!loopCount.IsNone || !currentLoopProgress.IsNone)
				{
					loopCount.Value = (int)Math.Truncate(nextAnimatorStateInfo.normalizedTime);
					currentLoopProgress.Value = nextAnimatorStateInfo.normalizedTime - (float)loopCount.Value;
				}
			}
		}
	}
}
