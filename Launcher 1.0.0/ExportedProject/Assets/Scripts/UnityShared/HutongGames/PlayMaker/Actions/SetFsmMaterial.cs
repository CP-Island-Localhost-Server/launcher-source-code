using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Material Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmMaterial : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[UIHint(UIHint.FsmMaterial)]
		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		public FsmString variableName;

		[RequiredField]
		[Tooltip("Set the value of the variable.")]
		public FsmMaterial setValue;

		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		private GameObject goLastFrame;

		private string fsmNameLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			variableName = "";
			setValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoSetFsmBool();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmBool()
		{
			if (setValue == null)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (ownerDefaultTarget != goLastFrame || fsmName.Value != fsmNameLastFrame)
			{
				goLastFrame = ownerDefaultTarget;
				fsmNameLastFrame = fsmName.Value;
				fsm = ActionHelpers.GetGameObjectFsm(ownerDefaultTarget, fsmName.Value);
			}
			if (fsm == null)
			{
				LogWarning("Could not find FSM: " + fsmName.Value);
				return;
			}
			FsmMaterial fsmMaterial = fsm.FsmVariables.GetFsmMaterial(variableName.Value);
			if (fsmMaterial != null)
			{
				fsmMaterial.Value = setValue.Value;
			}
			else
			{
				LogWarning("Could not find variable: " + variableName.Value);
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmBool();
		}
	}
}
