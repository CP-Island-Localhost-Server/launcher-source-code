using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a value to Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2Add : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The vector2 target")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		[RequiredField]
		[Tooltip("The vector2 to add")]
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
