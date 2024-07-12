using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Clamps the Magnitude of Vector3 Variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3ClampMagnitude : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		[RequiredField]
		public FsmFloat maxLength;

		public bool everyFrame;

		public override void Reset()
		{
			vector3Variable = null;
			maxLength = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoVector3ClampMagnitude();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector3ClampMagnitude();
		}

		private void DoVector3ClampMagnitude()
		{
			vector3Variable.Value = Vector3.ClampMagnitude(vector3Variable.Value, maxLength.Value);
		}
	}
}
