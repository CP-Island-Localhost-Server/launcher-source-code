using System;

namespace Mono.Unix.Native
{
	[Map("struct pollfd")]
	public struct Pollfd : IEquatable<Pollfd>
	{
		public int fd;

		[CLSCompliant(false)]
		public PollEvents events;

		[CLSCompliant(false)]
		public PollEvents revents;

		public override int GetHashCode()
		{
			return events.GetHashCode() ^ revents.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			Pollfd pollfd = (Pollfd)obj;
			return pollfd.events == events && pollfd.revents == revents;
		}

		public bool Equals(Pollfd value)
		{
			return value.events == events && value.revents == revents;
		}

		public static bool operator ==(Pollfd lhs, Pollfd rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Pollfd lhs, Pollfd rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
