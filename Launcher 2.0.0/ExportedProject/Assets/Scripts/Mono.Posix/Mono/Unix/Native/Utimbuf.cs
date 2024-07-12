using System;

namespace Mono.Unix.Native
{
	[Map("struct utimbuf")]
	public struct Utimbuf : IEquatable<Utimbuf>
	{
		[time_t]
		public long actime;

		[time_t]
		public long modtime;

		public override int GetHashCode()
		{
			return actime.GetHashCode() ^ modtime.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			Utimbuf utimbuf = (Utimbuf)obj;
			return utimbuf.actime == actime && utimbuf.modtime == modtime;
		}

		public bool Equals(Utimbuf value)
		{
			return value.actime == actime && value.modtime == modtime;
		}

		public static bool operator ==(Utimbuf lhs, Utimbuf rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Utimbuf lhs, Utimbuf rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
