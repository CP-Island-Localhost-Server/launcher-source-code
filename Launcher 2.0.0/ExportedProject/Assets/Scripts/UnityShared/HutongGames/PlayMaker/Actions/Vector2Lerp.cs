using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Linearly interpolates between 2 vectors.")]
	public class Vector2Lerp : FsmStateAction
	{
		[RequiredField]
		[Tooltip("First Vector.")]
		public FsmVector2 fromVector;

		[Tooltip("Second Vector.")]
		[RequiredField]
		public FsmVector2 toVector;

		[Tooltip("Interpolate between From Vector and ToVector by this amount. Value is clamped to 0-1 range. 0 = From Vector; 1 = To Vector; 0.5 = half way between.")]
		[RequiredField]
		public FsmFloat amount;

		[Tooltip("Store the result in this vector variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector2 storeResult;

		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			fromVector = new FsmVector2
			{
				UseVariable = true
			};
			toVector = new FsmVector2
			{
				UseVariable = true
			};
			storeResult = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoVector2Lerp();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector2Lerp();
		}

		private void DoVector2Lerp()
		{
			storeResult.Value = Vector2.Lerp(fromVector.Value, toVector.Value, amount.Value);
		}
	}
}
