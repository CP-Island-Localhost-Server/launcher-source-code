namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Convert)]
	[Tooltip("Converts a Material variable to an Object variable. Useful if you want to use Set Property (which only works on Object variables).")]
	public class ConvertMaterialToObject : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Material variable to convert to an Object.")]
		[RequiredField]
		public FsmMaterial materialVariable;

		[Tooltip("Store the result in an Object variable.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmObject objectVariable;

		[Tooltip("Repeat every frame. Useful if the Material variable is changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			materialVariable = null;
			objectVariable = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoConvertMaterialToObject();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoConvertMaterialToObject();
		}

		private void DoConvertMaterialToObject()
		{
			objectVariable.Value = materialVariable.Value;
		}
	}
}
