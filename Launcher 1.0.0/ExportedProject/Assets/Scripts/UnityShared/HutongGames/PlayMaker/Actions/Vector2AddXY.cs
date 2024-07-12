using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Adds a XY values to Vector2 Variable.")]
	[ActionCategory(ActionCategory.Vector2)]
	public class Vector2AddXY : FsmStateAction
	{
		[Tooltip("The vector2 target")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector2 vector2Variable;

		[Tooltip("The x component to add")]
		public FsmFloat addX;

		[Tooltip("The y component to add")]
		public FsmFloat addY;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		[Tooltip("Add the value on a per second bases.")]
		public bool perSecond;

		public override void Reset()
		{
			vector2Variable = null;
			addX = 0f;
			addY = 0f;
			everyFrame = false;
			perSecond = false;
		}

		public override void OnEnter()
		{
			DoVector2AddXYZ();
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoVector2AddXYZ();
		}

		private void DoVector2AddXYZ()
		{
			Vector2 vector = new Vector2(addX.Value, addY.Value);
			if (perSecond)
			{
				vector2Variable.Value += vector * Time.deltaTime;
			}
			else
			{
				vector2Variable.Value += vector;
			}
		}
	}
}
