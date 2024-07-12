using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Adds a value to Vector2 Variable.")]
	public class Vector2Add : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The vector2 target")]
		[RequiredField]
		public FsmVector2 vector2Variable;

		[Tooltip("The vector2 to add")]
		[RequiredField]
		public FsmVector2 addVector;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		[Tooltip("Add the value on a per second bases.")]
		public bool perSecond;

		public override void Reset()
		{
			vector2Variable = null;
			addVector = new FsmVector2
			{
				UseVariable = true
			};
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoVector2Add();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector2Add();
		}

		private void DoVector2Add()
		{
			if (perSecond)
			{
				vector2Variable.Value += addVector.Value * Time.deltaTime;
			}
			else
			{
				vector2Variable.Value += addVector.Value;
			}
		}
	}
}
