using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the layer's current weight")]
	[ActionCategory(ActionCategory.Animator)]
	public class SetAnimatorLayerWeight : FsmStateAction
	{
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The layer's index")]
		public FsmInt layerIndex;

		[RequiredField]
		[Tooltip("Sets the layer's current weight")]
		public FsmFloat layerWeight;

		[Tooltip("Repeat every frame. Useful for changing over time.")]
		public bool everyFrame;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			layerIndex = null;
			layerWeight = null;
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
			DoLayerWeight();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoLayerWeight();
		}

		private void DoLayerWeight()
		{
			if (!(_animator == null))
			{
				_animator.SetLayerWeight(layerIndex.Value, layerWeight.Value);
			}
		}
	}
}
