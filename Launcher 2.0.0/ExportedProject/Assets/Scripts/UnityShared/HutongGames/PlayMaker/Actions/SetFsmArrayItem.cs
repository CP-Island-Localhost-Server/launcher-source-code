using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	[Tooltip("Set an item in an Array Variable in another FSM.")]
	public class SetFsmArrayItem : BaseFsmVariableIndexAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[Tooltip("The name of the FSM variable.")]
		[RequiredField]
		[UIHint(UIHint.FsmArray)]
		public FsmString variableName;

		[Tooltip("The index into the array.")]
		public FsmInt index;

		[Tooltip("Set the value of the array at the specified index.")]
		[RequiredField]
		public FsmVar value;

		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			value = null;
		}

		public override void OnEnter()
		{
			DoSetFsmArray();
			if (!everyFrame)
			{
				Finish();
			}
		}

		private void DoSetFsmArray()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(ownerDefaultTarget, fsmName.Value))
			{
				return;
			}
			FsmArray fsmArray = fsm.FsmVariables.GetFsmArray(variableName.Value);
			if (fsmArray != null)
			{
				if (index.Value < 0 || index.Value >= fsmArray.Length)
				{
					base.Fsm.Event(indexOutOfRange);
					Finish();
				}
				else if (fsmArray.ElementType == value.NamedVar.VariableType)
				{
					value.UpdateValue();
					fsmArray.Set(index.Value, value.GetValue());
				}
				else
				{
					LogWarning("Incompatible variable type: " + variableName.Value);
				}
			}
			else
			{
				DoVariableNotFound(variableName.Value);
			}
		}

		public override void OnUpdate()
		{
			DoSetFsmArray();
		}
	}
}
