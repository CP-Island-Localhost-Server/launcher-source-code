using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Quaternion Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmQuaternion : FsmStateAction
	{
		[Tooltip("The GameObject that owns the FSM.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		[UIHint(UIHint.FsmQuaternion)]
		public FsmString variableName;

		[Tooltip("Set the value of the variable.")]
		[RequiredField]
		public FsmQuaternion setValue;

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
			DoSetFsmQuaternion();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmQuaternion()
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
			FsmQuaternion fsmQuaternion = fsm.FsmVariables.GetFsmQuaternion(variableName.Value);
			if (fsmQuaternion != null)
			{
				fsmQuaternion.Value = setValue.Value;
			}
			else
			{
				LogWarning("Could not find variable: " + variableName.Value);
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmQuaternion();
		}
	}
}
