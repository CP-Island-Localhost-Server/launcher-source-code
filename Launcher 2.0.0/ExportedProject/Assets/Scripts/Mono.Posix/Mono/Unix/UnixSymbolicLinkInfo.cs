using System;
using System.Text;
using Mono.Unix.Native;

namespace Mono.Unix
{
	public sealed class UnixSymbolicLinkInfo : UnixFileSystemInfo
	{
		public override string Name
		{
			get
			{
				return UnixPath.GetFileName(base.FullPath);
			}
		}

		[Obsolete("Use GetContents()")]
		public UnixFileSystemInfo Contents
		{
			get
			{
				return GetContents();
			}
		}

		public string ContentsPath
		{
			get
			{
				return ReadLink();
			}
		}

		public bool HasContents
		{
			get
			{
				return TryReadLink() != null;
			}
		}

		public UnixSymbolicLinkInfo(string path)
			: base(path)
		{
		}

		internal UnixSymbolicLinkInfo(string path, Stat stat)
			: base(path, stat)
		{
		}

		public UnixFileSystemInfo GetContents()
		{
			string text = ReadLink();
			return UnixFileSystemInfo.GetFileSystemEntry(UnixPath.Combine(UnixPath.GetDirectoryName(base.FullPath), ContentsPath));
		}

		public void CreateSymbolicLinkTo(string path)
		{
			int retval = Syscall.symlink(path, FullName);
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
		}

		public void CreateSymbolicLinkTo(UnixFileSystemInfo path)
		{
			int retval = Syscall.symlink(path.FullName, FullName);
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
		}

		public override void Delete()
		{
			int retval = Syscall.unlink(base.FullPath);
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
			Refresh();
		}

		public override void SetOwner(long owner, long group)
		{
			int retval = Syscall.lchown(base.FullPath, Convert.ToUInt32(owner), Convert.ToUInt32(group));
			UnixMarshal.ThrowExceptionForLastErrorIf(retval);
		}

		protected override bool GetFileStatus(string path, out Stat stat)
		{
			return Syscall.lstat(path, out stat) == 0;
		}

		private string ReadLink()
		{
			string text = TryReadLink();
			if (text == null)
			{
				UnixMarshal.ThrowExceptionForLastError();
			}
			return text;
		}

		private string TryReadLink()
		{
			StringBuilder stringBuilder = new StringBuilder((int)base.Length + 1);
			int num = Syscall.readlink(base.FullPath, stringBuilder);
			if (num == -1)
			{
				return null;
			}
			return stringBuilder.ToString(0, num);
		}
	}
}
