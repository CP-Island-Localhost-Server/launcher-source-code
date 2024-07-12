using System;

namespace Mono.Unix.Native
{
	[Map("struct stat")]
	public struct Stat : IEquatable<Stat>
	{
		[dev_t]
		[CLSCompliant(false)]
		public ulong st_dev;

		[ino_t]
		[CLSCompliant(false)]
		public ulong st_ino;

		[CLSCompliant(false)]
		public FilePermissions st_mode;

		[NonSerialized]
		private uint _padding_;

		[CLSCompliant(false)]
		[nlink_t]
		public ulong st_nlink;

		[CLSCompliant(false)]
		[uid_t]
		public uint st_uid;

		[gid_t]
		[CLSCompliant(false)]
		public uint st_gid;

		[dev_t]
		[CLSCompliant(false)]
		public ulong st_rdev;

		[off_t]
		public long st_size;

		[blksize_t]
		public long st_blksize;

		[blkcnt_t]
		public long st_blocks;

		[time_t]
		public long st_atime;

		[time_t]
		public long st_mtime;

		[time_t]
		public long st_ctime;

		public override int GetHashCode()
		{
			return st_dev.GetHashCode() ^ st_ino.GetHashCode() ^ st_mode.GetHashCode() ^ st_nlink.GetHashCode() ^ st_uid.GetHashCode() ^ st_gid.GetHashCode() ^ st_rdev.GetHashCode() ^ st_size.GetHashCode() ^ st_blksize.GetHashCode() ^ st_blocks.GetHashCode() ^ st_atime.GetHashCode() ^ st_mtime.GetHashCode() ^ st_ctime.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			Stat stat = (Stat)obj;
			return stat.st_dev == st_dev && stat.st_ino == st_ino && stat.st_mode == st_mode && stat.st_nlink == st_nlink && stat.st_uid == st_uid && stat.st_gid == st_gid && stat.st_rdev == st_rdev && stat.st_size == st_size && stat.st_blksize == st_blksize && stat.st_blocks == st_blocks && stat.st_atime == st_atime && stat.st_mtime == st_mtime && stat.st_ctime == st_ctime;
		}

		public bool Equals(Stat value)
		{
			return value.st_dev == st_dev && value.st_ino == st_ino && value.st_mode == st_mode && value.st_nlink == st_nlink && value.st_uid == st_uid && value.st_gid == st_gid && value.st_rdev == st_rdev && value.st_size == st_size && value.st_blksize == st_blksize && value.st_blocks == st_blocks && value.st_atime == st_atime && value.st_mtime == st_mtime && value.st_ctime == st_ctime;
		}

		public static bool operator ==(Stat lhs, Stat rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(Stat lhs, Stat rhs)
		{
			return !lhs.Equals(rhs);
		}
	}
}
