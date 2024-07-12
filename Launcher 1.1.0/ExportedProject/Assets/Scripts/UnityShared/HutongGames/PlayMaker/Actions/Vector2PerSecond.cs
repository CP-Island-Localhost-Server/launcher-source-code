using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector2)]
	[Tooltip("Multiplies a Vector2 variable by Time.deltaTime. Useful for frame rate independent motion.")]
	public class Vector2PerSecond : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The Vector2")]
		[RequiredField]
		public FsmVector2 vector2Variable;

		[Tooltip("Repeat every frame")]
		public bool everyFrame;

		public override void Reset()
		{
			vector2Variable = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			vector2Variable.Value *= Time.deltaTime;
			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			vector2Variable.Value *= Time.deltaTime;
		}
	}
}
