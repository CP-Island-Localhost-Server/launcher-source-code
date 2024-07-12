using System;

namespace Mono.Unix.Native
{
	[CLSCompliant(false)]
	[Map]
	public enum LockType : short
	{
		F_RDLCK = 0,
		F_WRLCK = 1,
		F_UNLCK = 2
	}
}
