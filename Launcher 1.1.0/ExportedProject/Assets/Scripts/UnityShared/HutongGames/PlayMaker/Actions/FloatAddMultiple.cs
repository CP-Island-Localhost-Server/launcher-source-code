namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds multipe float variables to float variable.")]
	[ActionCategory(ActionCategory.Math)]
	public class FloatAddMultiple : FsmStateAction
	{
		[Tooltip("The float variables to add.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat[] floatVariables;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Add to this variable.")]
		public FsmFloat addTo;

		[Tooltip("Repeat every frame while the state is active.")]
		public bool everyFrame;

		public override void Reset()
		{
			floatVariables = null;
			addTo = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoFloatAdd();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatAdd();
		}

		private void DoFloatAdd()
		{
			for (int i = 0; i < floatVariables.Length; i++)
			{
				addTo.Value += floatVariables[i].Value;
			}
		}
	}
}
