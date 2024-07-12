using System;

namespace Mono.Unix.Native
{
	[Map]
	[Flags]
	public enum WaitOptions
	{
		WNOHANG = 1,
		WUNTRACED = 2
	}
}
