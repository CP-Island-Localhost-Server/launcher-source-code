using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Animator)]
	[Tooltip("Gets the avatar body mass center position and rotation. Optionally accepts a GameObject to get the body transform. \nThe position and rotation are local to the gameobject")]
	public class GetAnimatorBody : FsmStateActionAnimatorBase
	{
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The avatar body mass center")]
		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 bodyPosition;

		[UIHint(UIHint.Variable)]
		[Tooltip("The avatar body mass center")]
		public FsmQuaternion bodyRotation;

		[Tooltip("If set, apply the body mass center position and rotation to this gameObject")]
		public FsmGameObject bodyGameObject;

		private Animator _animator;

		private Transform _transform;

		public override void Reset()
		{
			gameObject = null;
			bodyPosition = null;
			bodyRotation = null;
			bodyGameObject = null;
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
			GameObject value = bodyGameObject.Value;
			if (value != null)
			{
				_transform = value.transform;
			}
			DoGetBodyPosition();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnActionUpdate()
		{
			DoGetBodyPosition();
		}

		private void DoGetBodyPosition()
		{
			if (!(_animator == null))
			{
				bodyPosition.Value = _animator.bodyPosition;
				bodyRotation.Value = _animator.bodyRotation;
				if (_transform != null)
				{
					_transform.position = _animator.bodyPosition;
					_transform.rotation = _animator.bodyRotation;
				}
			}
		}
	}
}
