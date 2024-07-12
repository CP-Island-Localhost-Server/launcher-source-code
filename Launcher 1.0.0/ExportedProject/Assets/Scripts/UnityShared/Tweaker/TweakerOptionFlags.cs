using System;

namespace Tweaker
{
	[Flags]
	public enum TweakerOptionFlags
	{
		None = 0,
		Default = 1,
		ScanForInvokables = 2,
		ScanForTweakables = 4,
		ScanForWatchables = 8,
		ScanInEverything = 0x10,
		ScanInEntryAssembly = 0x20,
		ScanInExecutingAssembly = 0x40,
		ScanInNonSystemAssemblies = 0x80,
		DoNotAutoScan = 0x100,
		IncludeTests = 0x200
	}
}
