namespace DI.Threading
{
	public class SwitchTo
	{
		public enum TargetType
		{
			Main = 0,
			Thread = 1
		}

		public static readonly SwitchTo MainThread = new SwitchTo(TargetType.Main);

		public static readonly SwitchTo Thread = new SwitchTo(TargetType.Thread);

		public TargetType Target { get; private set; }

		private SwitchTo(TargetType target)
		{
			Target = target;
		}
	}
}
