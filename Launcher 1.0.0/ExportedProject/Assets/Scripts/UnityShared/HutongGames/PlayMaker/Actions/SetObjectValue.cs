namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Sets the value of an Object Variable.")]
	[ActionCategory(ActionCategory.UnityObject)]
	public class SetObjectValue : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmObject objectVariable;

		[RequiredField]
		public FsmObject objectValue;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			objectVariable = null;
			objectValue = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			objectVariable.Value = objectValue.Value;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			objectVariable.Value = objectValue.Value;
		}
	}
}
