using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a value to Vector3 Variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class Vector3Add : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		[RequiredField]
		public FsmVector3 addVector;

		public bool everyFrame;

		public bool perSecond;

		public override void Reset()
		{
			vector3Variable = null;
			addVector = new FsmVector3
			{
				UseVariable = true
			};
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoVector3Add();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector3Add();
		}

		private void DoVector3Add()
		{
			if (perSecond)
			{
				vector3Variable.Value += addVector.Value * Time.deltaTime;
			}
			else
			{
				vector3Variable.Value += addVector.Value;
			}
		}
	}
}
