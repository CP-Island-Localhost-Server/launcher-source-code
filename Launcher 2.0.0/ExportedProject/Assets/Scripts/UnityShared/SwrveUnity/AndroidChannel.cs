namespace SwrveUnity
{
	public class AndroidChannel
	{
		public enum ImportanceLevel
		{
			Default = 0,
			High = 1,
			Low = 2,
			Max = 3,
			Min = 4,
			None = 5
		}

		public readonly string Id;

		public readonly string Name;

		public readonly ImportanceLevel Importance;

		public AndroidChannel(string id, string name, ImportanceLevel importance)
		{
			Id = id;
			Name = name;
			Importance = importance;
		}
	}
}
