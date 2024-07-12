namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Flips the value of a Bool Variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class BoolFlip : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("Bool variable to flip.")]
		public FsmBool boolVariable;

		public override void Reset()
		{
			boolVariable = null;
		}

		public override void OnEnter()
		{
			boolVariable.Value = !boolVariable.Value;
			Finish();
		}
	}
}
