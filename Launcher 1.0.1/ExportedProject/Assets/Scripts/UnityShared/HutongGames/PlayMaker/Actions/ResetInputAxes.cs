using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Resets all Input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame")]
	[ActionCategory(ActionCategory.Input)]
	public class ResetInputAxes : FsmStateAction
	{
		public override void Reset()
		{
		}

		public override void OnEnter()
		{
			Input.ResetInputAxes();
			Finish();
		}
	}
}
