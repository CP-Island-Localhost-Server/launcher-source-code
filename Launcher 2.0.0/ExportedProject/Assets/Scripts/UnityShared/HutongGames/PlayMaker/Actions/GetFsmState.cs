using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets the name of the specified FSMs current state. Either reference the fsm component directly, or find it on a game object.")]
	[ActionTarget(typeof(PlayMakerFSM), "fsmComponent", false)]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetFsmState : FsmStateAction
	{
		[Tooltip("Drag a PlayMakerFSM component here.")]
		public PlayMakerFSM fsmComponent;

		[Tooltip("If not specifyng the component above, specify the GameObject that owns the FSM")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of Fsm on Game Object. If left blank it will find the first PlayMakerFSM on the GameObject.")]
		public FsmString fsmName;

		[Tooltip("Store the state name in a string variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmString storeResult;

		[Tooltip("Repeat every frame. E.g.,  useful if you're waiting for the state to change.")]
		public bool everyFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			fsmComponent = null;
			gameObject = null;
			fsmName = "";
			storeResult = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoGetFsmState();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmState();
		}

		private void DoGetFsmState()
		{
			if (fsm == null)
			{
				if (fsmComponent != null)
				{
					fsm = fsmComponent;
				}
				else
				{
					GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
					if (ownerDefaultTarget != null)
					{
						fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
					}
				}
				if (fsm == null)
				{
					storeResult.Value = "";
					return;
				}
			}
			storeResult.Value = fsm.ActiveStateName;
		}
	}
}
