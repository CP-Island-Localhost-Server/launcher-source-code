namespace HutongGames.PlayMaker.Actions
{
	public abstract class QuaternionBaseAction : FsmStateAction
	{
		public enum everyFrameOptions
		{
			Update = 0,
			FixedUpdate = 1,
			LateUpdate = 2
		}

		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;

		[Tooltip("Defines how to perform the action when 'every Frame' is enabled.")]
		public everyFrameOptions everyFrameOption;

		public override void Awake()
		{
			if (everyFrame && everyFrameOption == everyFrameOptions.FixedUpdate)
			{
				base.Fsm.HandleFixedUpdate = true;
			}
		}
	}
}
