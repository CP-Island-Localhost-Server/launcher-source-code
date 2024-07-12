using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Clamps the Magnitude of Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2ClampMagnitude : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[Tooltip("The Vector2")]
		public FsmVector2 vector2Variable;

		[RequiredField]
		[Tooltip("The maximum Magnitude")]
		public FsmFloat maxLength;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			vector2Variable = null;
			maxLength = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoVector2ClampMagnitude();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector2ClampMagnitude();
		}

		private void DoVector2ClampMagnitude()
		{
			vector2Variable.Value = Vector2.ClampMagnitude(vector2Variable.Value, maxLength.Value);
		}
	}
}
