namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last joint break event.")]
	public class GetJointBreakInfo : FsmStateAction
	{
		[Tooltip("Get the force that broke the joint.")]
		[UIHint(UIHint.Variable)]
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
