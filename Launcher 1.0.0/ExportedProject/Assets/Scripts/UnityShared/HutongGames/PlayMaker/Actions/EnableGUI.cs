namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Enables/Disables the PlayMakerGUI component in the scene. Note, you need a PlayMakerGUI component in the scene to see OnGUI actions. However, OnGUI can be very expensive on mobile devices. This action lets you turn OnGUI on/off (e.g., turn it on for a menu, and off during gameplay).")]
	[ActionCategory(ActionCategory.GUI)]
	public class EnableGUI : FsmStateAction
	{
		[Tooltip("Set to True to enable, False to disable.")]
		public FsmBool enableGUI;

		public override void Reset()
		{
			enableGUI = true;
		}

		public override void OnEnter()
		{
			PlayMakerGUI.Instance.enabled = enableGUI.Value;
			Finish();
		}
	}
}
