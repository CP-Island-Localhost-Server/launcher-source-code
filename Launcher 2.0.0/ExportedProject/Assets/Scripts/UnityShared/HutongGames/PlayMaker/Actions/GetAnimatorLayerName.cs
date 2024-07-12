using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Returns the name of a layer from its index")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorLayerName : FsmStateAction
	{
		[Tooltip("The Target. An Animator component is required")]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The layer index")]
		public FsmInt layerIndex;

		[Tooltip("The layer name")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[ActionSection("Results")]
		public FsmString layerName;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			layerIndex = null;
			layerName = null;
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
			DoGetLayerName();
			Finish();
		}

		private void DoGetLayerName()
		{
			if (!(_animator == null))
			{
				layerName.Value = _animator.GetLayerName(layerIndex.Value);
			}
		}
	}
}
