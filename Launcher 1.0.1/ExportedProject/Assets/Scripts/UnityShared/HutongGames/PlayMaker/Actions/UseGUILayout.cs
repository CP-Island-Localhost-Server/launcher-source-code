namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Turn GUILayout on/off. If you don't use GUILayout actions you can get some performace back by turning GUILayout off. This can make a difference on iOS platforms.")]
	[ActionCategory(ActionCategory.GUILayout)]
	public class UseGUILayout : FsmStateAction
	{
		[RequiredField]
		public bool turnOffGUIlayout;

		public override void Reset()
		{
			turnOffGUIlayout = true;
		}

		public override void OnEnter()
		{
			base.Fsm.Owner.useGUILayout = !turnOffGUIlayout;
			Finish();
		}
	}
}
