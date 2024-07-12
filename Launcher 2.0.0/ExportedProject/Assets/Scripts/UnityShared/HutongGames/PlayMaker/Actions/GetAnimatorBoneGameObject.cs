using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the GameObject mapped to this human bone id")]
	[ActionCategory(ActionCategory.Animator)]
	public class GetAnimatorBoneGameObject : FsmStateAction
	{
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		[Tooltip("The target. An Animator component is required")]
		public FsmOwnerDefault gameObject;

		[ObjectType(typeof(HumanBodyBones))]
		[Tooltip("The bone reference")]
		public FsmEnum bone;

		[ActionSection("Results")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Bone's GameObject")]
		public FsmGameObject boneGameObject;

		private Animator _animator;

		public override void Reset()
		{
			gameObject = null;
			bone = HumanBodyBones.Hips;
			boneGameObject = null;
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
			GetBoneTransform();
			Finish();
		}

		private void GetBoneTransform()
		{
			boneGameObject.Value = _animator.GetBoneTransform((HumanBodyBones)(object)bone.Value).gameObject;
		}
	}
}
