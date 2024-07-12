namespace UnityTest
{
	public enum TestResultState : byte
	{
		Inconclusive = 0,
		NotRunnable = 1,
		Skipped = 2,
		Ignored = 3,
		Success = 4,
		Failure = 5,
		Error = 6,
		Cancelled = 7
	}
}
