using System;

namespace Mono.Unix.Native
{
	[Map("struct timespec")]
	public struct Timespec : IEquatable<Timespec>
	{
		[time_t]
		public long tv_sec;

		public long tv_nsec;

		public override int GetHashCode()
		{
			return tv_sec.GetHashCode() ^ tv_nsec.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			Timespec timespec = (Timespec)obj;
			return timespec.tv_sec == tv_sec && timespec.tv_nsec == tv_nsec;
		}

		public bool Equals(Timespec value)
		{
			return value.tv_sec == tv_sec && value.tv_nsec == tv_nsec;
		}

		public static bool operator ==(Timespec lhs, Timespec rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Timespec lhs, Timespec rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
