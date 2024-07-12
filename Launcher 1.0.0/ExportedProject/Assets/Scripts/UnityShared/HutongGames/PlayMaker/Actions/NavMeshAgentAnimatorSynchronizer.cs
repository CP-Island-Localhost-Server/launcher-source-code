using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Synchronize a NavMesh Agent velocity and rotation with the animator process.")]
	[ActionCategory(ActionCategory.Animator)]
	public class NavMeshAgentAnimatorSynchronizer : FsmStateAction
	{
		[Tooltip("The Agent target. An Animator component and a NavMeshAgent component are required")]
		[CheckForComponent(typeof(NavMeshAgent))]
		[CheckForComponent(typeof(Animator))]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		private Animator _animator;

		private NavMeshAgent _agent;

		private Transform _trans;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnPreprocess()
		{
			base.Fsm.HandleAnimatorMove = true;
		}

		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				Finish();
				return;
			}
			_agent = ownerDefaultTarget.GetComponent<NavMeshAgent>();
			_animator = ownerDefaultTarget.GetComponent<Animator>();
			if (_animator == null)
			{
				Finish();
			}
			else
			{
				_trans = ownerDefaultTarget.transform;
			}
		}

		public override void DoAnimatorMove()
		{
			_agent.velocity = _animator.deltaPosition / Time.deltaTime;
			_trans.rotation = _animator.rootRotation;
		}
	}
}
