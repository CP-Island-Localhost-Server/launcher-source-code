using System;

namespace LitJson
{
	[Flags]
	public enum JsonIgnoreWhen
	{
		Never = 0,
		Serializing = 1,
		Deserializing = 2
	}
}
