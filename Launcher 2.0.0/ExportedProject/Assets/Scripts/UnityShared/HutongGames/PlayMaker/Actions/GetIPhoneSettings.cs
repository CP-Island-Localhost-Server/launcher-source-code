namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Get various iPhone settings.")]
	public class GetIPhoneSettings : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Allows device to fall into 'sleep' state with screen being dim if no touches occurred. Default value is true.")]
		public FsmBool getScreenCanDarken;

		[Tooltip("A unique device identifier string. It is guaranteed to be unique for every device (Read Only).")]
		[UIHint(UIHint.Variable)]
		public FsmString getUniqueIdentifier;

		[Tooltip("The user defined name of the device (Read Only).")]
		[UIHint(UIHint.Variable)]
		public FsmString getName;

		[UIHint(UIHint.Variable)]
		[Tooltip("The model of the device (Read Only).")]
		public FsmString getModel;

		[UIHint(UIHint.Variable)]
		[Tooltip("The name of the operating system running on the device (Read Only).")]
		public FsmString getSystemName;

		[Tooltip("The generation of the device (Read Only).")]
		[UIHint(UIHint.Variable)]
		public FsmString getGeneration;

		public override void Reset()
		{
			getScreenCanDarken = null;
			getUniqueIdentifier = null;
			getName = null;
			getModel = null;
			getSystemName = null;
			getGeneration = null;
		}

		public override void OnEnter()
		{
			Finish();
		}
	}
}
