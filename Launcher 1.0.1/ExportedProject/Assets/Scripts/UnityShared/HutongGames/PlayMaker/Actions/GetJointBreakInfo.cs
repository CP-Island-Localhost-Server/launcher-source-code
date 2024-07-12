namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets info on the last joint break event.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetJointBreakInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the force that broke the joint.")]
		public FsmFloat breakForce;

		public override void Reset()
		{
			breakForce = null;
		}

		public override void OnEnter()
		{
			breakForce.Value = base.Fsm.JointBreakForce;
			Finish();
		}
	}
}
