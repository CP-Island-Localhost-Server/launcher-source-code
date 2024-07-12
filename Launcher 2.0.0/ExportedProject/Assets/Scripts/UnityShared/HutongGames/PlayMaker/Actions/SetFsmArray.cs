using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Copy an Array Variable in another FSM.")]
	[ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName", false)]
	public class SetFsmArray : BaseFsmVariableAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("Optional name of FSM on Game Object.")]
		[UIHint(UIHint.FsmName)]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmArray)]
		[Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

		[Tooltip("Set the content of the array variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmArray setValue;

		[Tooltip("If true, makes copies. if false, values share the same reference and editing one array item value will affect the source and vice versa. Warning, this only affect the current items of the source array. Adding or removing items doesn't affect other FsmArrays.")]
		public bool copyValues;

		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			variableName = null;
			setValue = null;
			copyValues = true;
		}

		public override void OnEnter()
		{
			DoSetFsmArrayCopy();
			Finish();
		}

		private void DoSetFsmArrayCopy()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(gameObject);
			if (!UpdateCache(ownerDefaultTarget, fsmName.Value))
			{
				return;
			}
			FsmArray fsmArray = fsm.FsmVariables.GetFsmArray(variableName.Value);
			if (fsmArray != null)
			{
				if (fsmArray.ElementType != setValue.ElementType)
				{
					LogError(string.Concat("Can only copy arrays with the same elements type. Found <", fsmArray.ElementType, "> and <", setValue.ElementType, ">"));
				}
				else
				{
					fsmArray.Resize(0);
					if (copyValues)
					{
						fsmArray.Values = setValue.Values.Clone() as object[];
					}
					else
					{
						fsmArray.Values = setValue.Values;
					}
				}
			}
			else
			{
				DoVariableNotFound(variableName.Value);
			}
		}
	}
}
