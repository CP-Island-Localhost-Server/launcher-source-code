namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Select a Random Vector3 from a Vector3 array.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class SelectRandomVector3 : FsmStateAction
	{
		[CompoundArray("Vectors", "Vector", "Weight")]
		public FsmVector3[] vector3Array;

		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 storeVector3;

		public override void Reset()
		{
			vector3Array = new FsmVector3[3];
			weights = new FsmFloat[3] { 1f, 1f, 1f };
			storeVector3 = null;
		}

		public override void OnEnter()
		{
			DoSelectRandomColor();
			Finish();
		}

		private void DoSelectRandomColor()
		{
			if (vector3Array != null && vector3Array.Length != 0 && storeVector3 != null)
			{
				int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(weights);
				if (randomWeightedIndex != -1)
				{
					storeVector3.Value = vector3Array[randomWeightedIndex].Value;
				}
			}
		}
	}
}
