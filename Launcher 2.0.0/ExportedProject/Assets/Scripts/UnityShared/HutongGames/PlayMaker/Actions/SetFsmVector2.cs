using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Set the value of a Vector2 Variable in another FSM.")]
	[ActionCategory(ActionCategory.StateMachine)]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmVector2 : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmVector2)]
		[Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

		[Tooltip("Set the value of the variable.")]
		[RequiredField]
		public FsmVector2 setValue;

		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		private GameObject goLastFrame;

		private string fsmNameLastFrame;

		private PlayMakerFSM fsm;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			setValue = null;
		}

		public override void OnEnter()
		{
			DoSetFsmVector2();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmVector2()
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
			FsmVector2 fsmVector = fsm.FsmVariables.GetFsmVector2(variableName.Value);
			if (fsmVector != null)
			{
				fsmVector.Value = setValue.Value;
			}
			else
			{
				LogWarning("Could not find variable: " + variableName.Value);
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmVector2();
		}
	}
}
