using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Clamps the Magnitude of Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2ClampMagnitude : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The Vector2")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		[Tooltip("The maximum Magnitude")]
		[RequiredField]
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
