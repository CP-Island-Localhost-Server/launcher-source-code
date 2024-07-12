using System;

namespace Mono.Unix.Native
{
	[Map("struct timeval")]
	public struct Timeval : IEquatable<Timeval>
	{
		[time_t]
		public long tv_sec;

		[suseconds_t]
		public long tv_usec;

		public override int GetHashCode()
		{
			return tv_sec.GetHashCode() ^ tv_usec.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			Timeval timeval = (Timeval)obj;
			return timeval.tv_sec == tv_sec && timeval.tv_usec == tv_usec;
		}

		public bool Equals(Timeval value)
		{
			return value.tv_sec == tv_sec && value.tv_usec == tv_usec;
		}

		public static bool operator ==(Timeval lhs, Timeval rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Timeval lhs, Timeval rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
