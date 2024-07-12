using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the avatar body mass center position and rotation.Optionally accept a GameObject to get the body transform. \nThe position and rotation are local to the gameobject")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorRoot : FsmStateActionAnimatorBase
	{
		[RequiredField]
		[Tooltip("The target.")]
		[CheckForComponent(typeof(Animator))]
		public FsmOwnerDefault gameObject;

		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The avatar body mass center")]
		public FsmVector3 rootPosition;

		[Tooltip("The avatar body mass center")]
		[UIHint(UIHint.Variable)]
		public FsmQuaternion rootRotation;

		[Tooltip("If set, apply the body mass center position and rotation to this gameObject")]
		public FsmGameObject bodyGameObject;

		private Animator _animator;

		private Transform _transform;

		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			rootPosition = null;
			rootRotation = null;
			bodyGameObject = null;
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
				rootPosition.Value = _animator.rootPosition;
				rootRotation.Value = _animator.rootRotation;
				if (_transform != null)
				{
					_transform.position = _animator.rootPosition;
					_transform.rotation = _animator.rootRotation;
				}
			}
		}
	}
}
