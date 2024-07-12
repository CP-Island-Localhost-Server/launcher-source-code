using System;
using Mono.Unix.Native;

namespace Mono.Unix
{
	public struct UnixPipes : IEquatable<UnixPipes>
	{
		public UnixStream Reading;

		public UnixStream Writing;

		public UnixPipes(UnixStream reading, UnixStream writing)
		{
			Reading = reading;
			Writing = writing;
		}

		public static UnixPipes CreatePipes()
		{
			int reading;
			int writing;
			int retval = Syscall.pipe(out reading, out writing);
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
			return new UnixPipes(new UnixStream(reading), new UnixStream(writing));
		}

		public override bool Equals(object value)
		{
			if (value == null || value.GetType() != GetType())
			{
				return false;
			}
			UnixPipes unixPipes = (UnixPipes)value;
			return Reading.Handle == unixPipes.Reading.Handle && Writing.Handle == unixPipes.Writing.Handle;
		}

		public bool Equals(UnixPipes value)
		{
			return Reading.Handle == value.Reading.Handle && Writing.Handle == value.Writing.Handle;
		}

		public override int GetHashCode()
		{
			return Reading.Handle.GetHashCode() ^ Writing.Handle.GetHashCode();
		}

		public static bool operator ==(UnixPipes lhs, UnixPipes rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(UnixPipes lhs, UnixPipes rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
