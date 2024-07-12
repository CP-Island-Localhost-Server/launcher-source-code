using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	public abstract class FsmStateAction : IFsmStateAction
	{
		private string name;

		private bool enabled = true;

		private bool isOpen = true;

		private bool active;

		private bool finished;

		private bool autoName;

		private GameObject owner;

		[NonSerialized]
		private FsmState fsmState;

		[NonSerialized]
		private Fsm fsm;

		[NonSerialized]
		private PlayMakerFSM fsmComponent;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public Fsm Fsm
		{
			get
			{
				return fsm;
			}
			set
			{
				fsm = value;
			}
		}

		public GameObject Owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
			}
		}

		public FsmState State
		{
			get
			{
				return fsmState;
			}
			set
			{
				fsmState = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		public bool IsOpen
		{
			get
			{
				return isOpen;
			}
			set
			{
				isOpen = value;
			}
		}

		public bool IsAutoNamed
		{
			get
			{
				return autoName;
			}
			set
			{
				autoName = value;
			}
		}

		public bool Entered { get; set; }

		public bool Finished
		{
			get
			{
				return finished;
			}
			set
			{
				if (value)
				{
					active = false;
				}
				finished = value;
			}
		}

		public bool Active
		{
			get
			{
				return active;
			}
			set
			{
				active = value;
			}
		}

		public virtual void Init(FsmState state)
		{
			fsmState = state;
			fsm = state.Fsm;
			owner = fsm.GameObject;
			fsmComponent = fsm.FsmComponent;
		}

		public virtual void Reset()
		{
		}

		public virtual void OnPreprocess()
		{
		}

		public virtual void Awake()
		{
		}

		public virtual bool Event(FsmEvent fsmEvent)
		{
			return false;
		}

		public void Finish()
		{
			active = false;
			finished = true;
			State.FinishAction(this);
		}

		public Coroutine StartCoroutine(IEnumerator routine)
		{
			return fsmComponent.StartCoroutine("DoCoroutine", routine);
		}

		public void StopCoroutine(Coroutine routine)
		{
			fsmComponent.StopCoroutine(routine);
		}

		public virtual void OnEnter()
		{
		}

		public virtual void OnFixedUpdate()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void OnGUI()
		{
		}

		public virtual void OnLateUpdate()
		{
		}

		public virtual void OnExit()
		{
		}

		public virtual void OnDrawActionGizmos()
		{
		}

		public virtual void OnDrawActionGizmosSelected()
		{
		}

		public virtual string AutoName()
		{
			return null;
		}

		public virtual void OnActionTargetInvoked(object targetObject)
		{
		}

		public virtual void DoCollisionEnter(Collision collisionInfo)
		{
		}

		public virtual void DoCollisionStay(Collision collisionInfo)
		{
		}

		public virtual void DoCollisionExit(Collision collisionInfo)
		{
		}

		public virtual void DoTriggerEnter(Collider other)
		{
		}

		public virtual void DoTriggerStay(Collider other)
		{
		}

		public virtual void DoTriggerExit(Collider other)
		{
		}

		public virtual void DoParticleCollision(GameObject other)
		{
		}

		public virtual void DoCollisionEnter2D(Collision2D collisionInfo)
		{
		}

		public virtual void DoCollisionStay2D(Collision2D collisionInfo)
		{
		}

		public virtual void DoCollisionExit2D(Collision2D collisionInfo)
		{
		}

		public virtual void DoTriggerEnter2D(Collider2D other)
		{
		}

		public virtual void DoTriggerStay2D(Collider2D other)
		{
		}

		public virtual void DoTriggerExit2D(Collider2D other)
		{
		}

		public virtual void DoControllerColliderHit(ControllerColliderHit collider)
		{
		}

		public virtual void DoJointBreak(float force)
		{
		}

		public virtual void DoJointBreak2D(Joint2D joint)
		{
		}

		public virtual void DoAnimatorMove()
		{
		}

		public virtual void DoAnimatorIK(int layerIndex)
		{
		}

		public void Log(string text)
		{
			if (FsmLog.LoggingEnabled)
			{
				fsm.MyLog.LogAction(FsmLogType.Info, text);
			}
		}

		public void LogWarning(string text)
		{
			if (FsmLog.LoggingEnabled)
			{
				fsm.MyLog.LogAction(FsmLogType.Warning, text);
			}
		}

		public void LogError(string text)
		{
			if (FsmLog.LoggingEnabled)
			{
				fsm.MyLog.LogAction(FsmLogType.Error, text);
			}
		}

		public virtual string ErrorCheck()
		{
			return string.Empty;
		}
	}
}
