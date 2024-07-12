using System;

namespace ClubPenguin.Launcher
{
	[Flags]
	public enum ButtonFlags
	{
		None = 0,
		Cancel = 1,
		Ok = 2,
		No = 4,
		Yes = 8,
		Close = 0x10,
		Retry = 0x20
	}
}
