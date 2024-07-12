using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmState : INameable
	{
		private bool active;

		private bool finished;

		private FsmStateAction activeAction;

		private int activeActionIndex;

		[NonSerialized]
		private Fsm fsm;

		[SerializeField]
		private string name;

		[SerializeField]
		private string description;

		[SerializeField]
		private byte colorIndex;

		[SerializeField]
		private Rect position;

		[SerializeField]
		private bool isBreakpoint;

		[SerializeField]
		private bool isSequence;

		[SerializeField]
		private bool hideUnused;

		[SerializeField]
		private FsmTransition[] transitions = new FsmTransition[0];

		[NonSerialized]
		private FsmStateAction[] actions;

		[SerializeField]
		private ActionData actionData = new ActionData();

		[NonSerialized]
		private List<FsmStateAction> activeActions;

		[NonSerialized]
		private List<FsmStateAction> _finishedActions;

		public float StateTime { get; private set; }

		public float RealStartTime { get; private set; }

		public int loopCount { get; private set; }

		public int maxLoopCount { get; private set; }

		public List<FsmStateAction> ActiveActions
		{
			get
			{
				return activeActions ?? (activeActions = new List<FsmStateAction>());
			}
		}

		private List<FsmStateAction> finishedActions
		{
			get
			{
				return _finishedActions ?? (_finishedActions = new List<FsmStateAction>());
			}
		}

		public bool Active
		{
			get
			{
				return active;
			}
		}

		public FsmStateAction ActiveAction
		{
			get
			{
				return activeAction;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return fsm != null;
			}
		}

		public Fsm Fsm
		{
			get
			{
				if (fsm == null)
				{
					Debug.LogError("get_fsm: Fsm not initialized: " + name);
				}
				return fsm;
			}
			set
			{
				if (value == null)
				{
					Debug.LogWarning("set_fsm: value == null: " + name);
				}
				fsm = value;
			}
		}

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

		public bool IsSequence
		{
			get
			{
				return isSequence;
			}
			set
			{
				isSequence = value;
			}
		}

		public int ActiveActionIndex
		{
			get
			{
				return activeActionIndex;
			}
		}

		public Rect Position
		{
			get
			{
				return position;
			}
			set
			{
				if (!float.IsNaN(value.x) && !float.IsNaN(value.y))
				{
					position = value;
				}
			}
		}

		public bool IsBreakpoint
		{
			get
			{
				return isBreakpoint;
			}
			set
			{
				isBreakpoint = value;
			}
		}

		public bool HideUnused
		{
			get
			{
				return hideUnused;
			}
			set
			{
				hideUnused = value;
			}
		}

		public FsmStateAction[] Actions
		{
			get
			{
				if (fsm == null)
				{
					Debug.LogError("get_actions: Fsm not initialized: " + name);
				}
				return actions ?? (actions = actionData.LoadActions(this));
			}
			set
			{
				actions = value;
			}
		}

		public bool ActionsLoaded
		{
			get
			{
				return actions != null;
			}
		}

		public ActionData ActionData
		{
			get
			{
				return actionData;
			}
		}

		public FsmTransition[] Transitions
		{
			get
			{
				return transitions;
			}
			set
			{
				transitions = value;
			}
		}

		public string Description
		{
			get
			{
				return description ?? (description = "");
			}
			set
			{
				description = value;
			}
		}

		public int ColorIndex
		{
			get
			{
				return colorIndex;
			}
			set
			{
				colorIndex = (byte)value;
			}
		}

		public static string GetFullStateLabel(FsmState state)
		{
			if (state == null)
			{
				return "None (State)";
			}
			return Fsm.GetFullFsmLabel(state.Fsm) + " : " + state.Name;
		}

		public FsmState(Fsm fsm)
		{
			this.fsm = fsm;
		}

		public FsmState(FsmState source)
		{
			fsm = source.Fsm;
			name = source.Name;
			description = source.description;
			colorIndex = source.colorIndex;
			position = new Rect(source.position);
			hideUnused = source.hideUnused;
			isBreakpoint = source.isBreakpoint;
			isSequence = source.isSequence;
			transitions = new FsmTransition[source.transitions.Length];
			for (int i = 0; i < source.Transitions.Length; i++)
			{
				transitions[i] = new FsmTransition(source.Transitions[i]);
			}
			actionData = source.actionData.Copy();
		}

		public void CopyActionData(FsmState state)
		{
			actionData = state.actionData.Copy();
		}

		public void LoadActions()
		{
			actions = actionData.LoadActions(this);
		}

		public void SaveActions()
		{
			if (actions != null)
			{
				actionData.SaveActions(this, actions);
			}
		}

		public void OnEnter()
		{
			loopCount++;
			if (loopCount > maxLoopCount)
			{
				maxLoopCount = loopCount;
			}
			active = true;
			finished = false;
			finishedActions.Clear();
			RealStartTime = FsmTime.RealtimeSinceStartup;
			StateTime = 0f;
			ActiveActions.Clear();
			if (ActivateActions(0))
			{
				CheckAllActionsFinished();
			}
		}

		private bool ActivateActions(int startIndex)
		{
			for (int i = startIndex; i < Actions.Length; i++)
			{
				activeActionIndex = i;
				FsmStateAction fsmStateAction = Actions[i];
				if (!fsmStateAction.Enabled)
				{
					fsmStateAction.Finished = true;
					continue;
				}
				ActiveActions.Add(fsmStateAction);
				activeAction = fsmStateAction;
				fsmStateAction.Active = true;
				fsmStateAction.Finished = false;
				fsmStateAction.Init(this);
				fsmStateAction.Entered = true;
				fsmStateAction.OnEnter();
				if (Fsm.IsSwitchingState)
				{
					return false;
				}
				if (!fsmStateAction.Finished && isSequence)
				{
					return false;
				}
			}
			return true;
		}

		public bool OnEvent(FsmEvent fsmEvent)
		{
			bool result = false;
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				result = fsmStateAction.Event(fsmEvent);
			}
			if (!fsm.IsSwitchingState)
			{
				return result;
			}
			return true;
		}

		public void OnFixedUpdate()
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.OnFixedUpdate();
			}
			CheckAllActionsFinished();
		}

		public void OnUpdate()
		{
			if (!finished)
			{
				StateTime += Time.deltaTime;
				for (int i = 0; i < ActiveActions.Count; i++)
				{
					FsmStateAction fsmStateAction = ActiveActions[i];
					fsmStateAction.Init(this);
					fsmStateAction.OnUpdate();
				}
				CheckAllActionsFinished();
			}
		}

		public void OnLateUpdate()
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.OnLateUpdate();
			}
			CheckAllActionsFinished();
		}

		public bool OnAnimatorMove()
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoAnimatorMove();
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnAnimatorIK(int layerIndex)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoAnimatorIK(layerIndex);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnCollisionEnter(Collision collisionInfo)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoCollisionEnter(collisionInfo);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnCollisionStay(Collision collisionInfo)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoCollisionStay(collisionInfo);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnCollisionExit(Collision collisionInfo)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoCollisionExit(collisionInfo);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnTriggerEnter(Collider other)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoTriggerEnter(other);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnTriggerStay(Collider other)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoTriggerStay(other);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnTriggerExit(Collider other)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoTriggerExit(other);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnParticleCollision(GameObject other)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoParticleCollision(other);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnCollisionEnter2D(Collision2D collisionInfo)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoCollisionEnter2D(collisionInfo);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnCollisionStay2D(Collision2D collisionInfo)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoCollisionStay2D(collisionInfo);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnCollisionExit2D(Collision2D collisionInfo)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoCollisionExit2D(collisionInfo);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnTriggerEnter2D(Collider2D other)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoTriggerEnter2D(other);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnTriggerStay2D(Collider2D other)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoTriggerStay2D(other);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnTriggerExit2D(Collider2D other)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoTriggerExit2D(other);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnControllerColliderHit(ControllerColliderHit collider)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoControllerColliderHit(collider);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnJointBreak(float force)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoJointBreak(force);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public bool OnJointBreak2D(Joint2D joint)
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.DoJointBreak2D(joint);
			}
			RemoveFinishedActions();
			return fsm.IsSwitchingState;
		}

		public void OnGUI()
		{
			for (int i = 0; i < ActiveActions.Count; i++)
			{
				FsmStateAction fsmStateAction = ActiveActions[i];
				fsmStateAction.Init(this);
				fsmStateAction.OnGUI();
			}
			RemoveFinishedActions();
		}

		public void FinishAction(FsmStateAction action)
		{
			finishedActions.Add(action);
		}

		private void RemoveFinishedActions()
		{
			for (int i = 0; i < finishedActions.Count; i++)
			{
				ActiveActions.Remove(finishedActions[i]);
			}
			finishedActions.Clear();
		}

		private void CheckAllActionsFinished()
		{
			if (!finished && active && !fsm.IsSwitchingState)
			{
				RemoveFinishedActions();
				if (ActiveActions.Count == 0 && (!isSequence || ++activeActionIndex >= actions.Length || ActivateActions(activeActionIndex)))
				{
					finished = true;
					fsm.Event(FsmEvent.Finished);
				}
			}
		}

		public void OnExit()
		{
			active = false;
			finished = false;
			FsmStateAction[] array = Actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (fsmStateAction.Entered)
				{
					activeAction = fsmStateAction;
					fsmStateAction.Init(this);
					fsmStateAction.OnExit();
				}
			}
		}

		public void ResetLoopCount()
		{
			loopCount = 0;
		}

		public FsmTransition GetTransition(int transitionIndex)
		{
			if (transitionIndex < 0 || transitionIndex > transitions.Length - 1)
			{
				return null;
			}
			return transitions[transitionIndex];
		}

		public int GetTransitionIndex(FsmTransition transition)
		{
			if (transition == null)
			{
				return -1;
			}
			for (int i = 0; i < transitions.Length; i++)
			{
				FsmTransition fsmTransition = transitions[i];
				if (fsmTransition == transition)
				{
					return i;
				}
			}
			return -1;
		}

		public static int GetStateIndex(FsmState state)
		{
			if (state.Fsm == null)
			{
				return -1;
			}
			for (int i = 0; i < state.Fsm.States.Length; i++)
			{
				FsmState fsmState = state.Fsm.States[i];
				if (fsmState == state)
				{
					return i;
				}
			}
			Debug.LogError("State not in FSM!");
			return -1;
		}
	}
}
