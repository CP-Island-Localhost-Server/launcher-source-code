using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the value of a variable in another FSM and store it in a variable of the same name in this FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class GetFsmVariable : FsmStateAction
	{
		[Tooltip("The GameObject that owns the FSM")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[Tooltip("Store the value of the FsmVariable")]
		[RequiredField]
		[HideTypeFilter]
		[UIHint(UIHint.Variable)]
		public FsmVar storeValue;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private GameObject cachedGO;

		private PlayMakerFSM sourceFsm;

		private INamedVariable sourceVariable;

		private NamedVariable targetVariable;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			storeValue = new FsmVar();
		}

		public override void OnEnter()
		{
			InitFsmVar();
			DoGetFsmVariable();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmVariable();
		}

		private void InitFsmVar()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!(ownerDefaultTarget == null) && ownerDefaultTarget != cachedGO)
			{
				sourceFsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
				sourceVariable = sourceFsm.FsmVariables.GetVariable(storeValue.variableName);
				targetVariable = base.Fsm.Variables.GetVariable(storeValue.variableName);
				storeValue.Type = targetVariable.VariableType;
				if (!string.IsNullOrEmpty(storeValue.variableName) && sourceVariable == null)
				{
					LogWarning("Missing Variable: " + storeValue.variableName);
				}
				cachedGO = ownerDefaultTarget;
			}
		}

		private void DoGetFsmVariable()
		{
			if (!storeValue.IsNone)
			{
				InitFsmVar();
				storeValue.GetValueFrom(sourceVariable);
				storeValue.ApplyValueTo(targetVariable);
			}
		}
	}
}
