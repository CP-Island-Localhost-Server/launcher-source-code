using System;

namespace Mono.Unix.Native
{
	[Map]
	[CLSCompliant(false)]
	[Flags]
	public enum MsyncFlags
	{
		MS_ASYNC = 1,
		MS_SYNC = 4,
		MS_INVALIDATE = 2
	}
}
