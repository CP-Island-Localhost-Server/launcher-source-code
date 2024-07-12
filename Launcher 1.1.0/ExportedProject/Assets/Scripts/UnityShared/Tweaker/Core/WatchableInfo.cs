namespace Tweaker.Core
{
	public abstract class WatchableInfo
	{
		public enum DisplayMode
		{
			Value = 0,
			ValueGraph = 1,
			Delta = 2,
			DeltaGraph = 3
		}

		public string Name { get; private set; }

		public DisplayMode Mode { get; private set; }

		public WatchableInfo(string name, DisplayMode mode = DisplayMode.Value)
		{
			Name = name;
			Mode = mode;
		}
	}
}
