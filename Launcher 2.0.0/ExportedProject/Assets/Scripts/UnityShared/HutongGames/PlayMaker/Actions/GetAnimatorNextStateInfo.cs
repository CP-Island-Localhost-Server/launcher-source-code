using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the next State information on a specified layer")]
	public class GetAnimatorNextStateInfo : FsmStateActionAnimatorBase
	{
		[Tooltip("The target. An Animator component is required")]
		[RequiredField]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The layer's index")]
		public FsmInt layerIndex;

		[ActionSection("Results")]
		[Tooltip("The layer's name.")]
		[UIHint(UIHint.Variable)]
		public FsmString name;

		[UIHint(UIHint.Variable)]
		[Tooltip("The layer's name Hash. Obsolete in Unity 5, use fullPathHash or shortPathHash instead, nameHash will be the same as shortNameHash for legacy")]
		public FsmInt nameHash;

		[Tooltip("The layer's tag hash")]
		[UIHint(UIHint.Variable)]
		public FsmInt tagHash;

		[Tooltip("Is the state looping. All animations in the state must be looping")]
		[UIHint(UIHint.Variable)]
		public FsmBool isStateLooping;

		[Tooltip("The Current duration of the state. In seconds, can vary when the State contains a Blend Tree ")]
		[UIHint(UIHint.Variable)]
		public FsmFloat length;

		[UIHint(UIHint.Variable)]
		[Tooltip("The integer part is the number of time a state has been looped. The fractional part is the % (0-1) of progress in the current loop")]
		public FsmFloat normalizedTime;

		[UIHint(UIHint.Variable)]
		[Tooltip("The integer part is the number of time a state has been looped. This is extracted from the normalizedTime")]
		public FsmInt loopCount;

		[Tooltip("The progress in the current loop. This is extracted from the normalizedTime")]
		[UIHint(UIHint.Variable)]
		public FsmFloat currentLoopProgress;

		private Animator _animator;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			layerIndex = null;
			name = null;
			nameHash = null;
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
				if (!nameHash.IsNone)
				{
					nameHash.Value = nextAnimatorStateInfo.nameHash;
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