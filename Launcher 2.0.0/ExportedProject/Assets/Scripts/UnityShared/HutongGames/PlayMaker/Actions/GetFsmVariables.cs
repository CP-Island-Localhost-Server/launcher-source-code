using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Get the values of multiple variables in another FSM and store in variables of the same name in this FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class GetFsmVariables : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the values of the FsmVariables")]
		[HideTypeFilter]
		public FsmVar[] getVariables;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		private GameObject cachedGO;

		private PlayMakerFSM sourceFsm;

		private INamedVariable[] sourceVariables;

		private NamedVariable[] targetVariables;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			getVariables = null;
		}

		private void InitFsmVars()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null || !(ownerDefaultTarget != cachedGO))
			{
				return;
			}
			sourceVariables = new INamedVariable[getVariables.Length];
			targetVariables = new NamedVariable[getVariables.Length];
			for (int i = 0; i < getVariables.Length; i++)
			{
				string variableName = getVariables[i].variableName;
				sourceFsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
				sourceVariables[i] = sourceFsm.FsmVariables.GetVariable(variableName);
				targetVariables[i] = base.Fsm.Variables.GetVariable(variableName);
				getVariables[i].Type = targetVariables[i].VariableType;
				if (!string.IsNullOrEmpty(variableName) && sourceVariables[i] == null)
				{
					LogWarning("Missing Variable: " + variableName);
				}
				cachedGO = ownerDefaultTarget;
			}
		}

		public override void OnEnter()
		{
			InitFsmVars();
			DoGetFsmVariables();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoGetFsmVariables();
		}

		private void DoGetFsmVariables()
		{
			InitFsmVars();
			for (int i = 0; i < getVariables.Length; i++)
			{
				getVariables[i].GetValueFrom(sourceVariables[i]);
				getVariables[i].ApplyValueTo(targetVariables[i]);
			}
		}
	}
}
