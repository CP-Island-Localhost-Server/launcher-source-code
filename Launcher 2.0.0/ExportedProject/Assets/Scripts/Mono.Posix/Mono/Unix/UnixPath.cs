using System;
using System.Text;
using Mono.Unix.Native;

namespace Mono.Unix
{
	public sealed class UnixPath
	{
		public static readonly char DirectorySeparatorChar = '/';

		public static readonly char AltDirectorySeparatorChar = '/';

		public static readonly char PathSeparator = ':';

		public static readonly char VolumeSeparatorChar = '/';

		private static readonly char[] _InvalidPathChars = new char[0];

		private UnixPath()
		{
		}

		public static char[] GetInvalidPathChars()
		{
			return (char[])_InvalidPathChars.Clone();
		}

		public static string Combine(string path1, params string[] paths)
		{
			if (path1 == null)
			{
				throw new ArgumentNullException("path1");
			}
			if (paths == null)
			{
				throw new ArgumentNullException("paths");
			}
			if (path1.IndexOfAny(_InvalidPathChars) != -1)
			{
				throw new ArgumentException("Illegal characters in path", "path1");
			}
			int num = path1.Length;
			int num2 = -1;
			for (int i = 0; i < paths.Length; i++)
			{
				if (paths[i] == null)
				{
					throw new ArgumentNullException("paths[" + i + "]");
				}
				if (paths[i].IndexOfAny(_InvalidPathChars) != -1)
				{
					throw new ArgumentException("Illegal characters in path", "paths[" + i + "]");
				}
				if (IsPathRooted(paths[i]))
				{
					num = 0;
					num2 = i;
				}
				num += paths[i].Length + 1;
			}
			StringBuilder stringBuilder = new StringBuilder(num);
			if (num2 == -1)
			{
				stringBuilder.Append(path1);
				num2 = 0;
			}
			for (int j = num2; j < paths.Length; j++)
			{
				Combine(stringBuilder, paths[j]);
			}
			return stringBuilder.ToString();
		}

		private static void Combine(StringBuilder path, string part)
		{
			if (path.Length > 0 && part.Length > 0)
			{
				char c = path[path.Length - 1];
				if (c != DirectorySeparatorChar && c != AltDirectorySeparatorChar && c != VolumeSeparatorChar)
				{
					path.Append(DirectorySeparatorChar);
				}
			}
			path.Append(part);
		}

		public static string GetDirectoryName(string path)
		{
			CheckPath(path);
			int num = path.LastIndexOf(DirectorySeparatorChar);
			if (num > 0)
			{
				return path.Substring(0, num);
			}
			if (num == 0)
			{
				return "/";
			}
			return string.Empty;
		}

		public static string GetFileName(string path)
		{
			if (path == null || path.Length == 0)
			{
				return path;
			}
			int num = path.LastIndexOf(DirectorySeparatorChar);
			if (num >= 0)
			{
				return path.Substring(num + 1);
			}
			return path;
		}

		public static string GetFullPath(string path)
		{
			path = _GetFullPath(path);
			return GetCanonicalPath(path);
		}

		private static string _GetFullPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (!IsPathRooted(path))
			{
				path = UnixDirectoryInfo.GetCurrentDirectory() + DirectorySeparatorChar + path;
			}
			return path;
		}

		public static string GetCanonicalPath(string path)
		{
			string[] components;
			int lastIndex;
			GetPathComponents(path, out components, out lastIndex);
			string text = string.Join("/", components, 0, lastIndex);
			return (!IsPathRooted(path)) ? text : ("/" + text);
		}

		private static void GetPathComponents(string path, out string[] components, out int lastIndex)
		{
			string[] array = path.Split(DirectorySeparatorChar);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] == ".") && !(array[i] == string.Empty))
				{
					if (array[i] == "..")
					{
						num = ((num == 0) ? (num + 1) : (num - 1));
					}
					else
					{
						array[num++] = array[i];
					}
				}
			}
			components = array;
			lastIndex = num;
		}

		public static string GetPathRoot(string path)
		{
			if (path == null)
			{
				return null;
			}
			if (!IsPathRooted(path))
			{
				return string.Empty;
			}
			return "/";
		}

		public static string GetCompleteRealPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string[] components;
			int lastIndex;
			GetPathComponents(path, out components, out lastIndex);
			StringBuilder stringBuilder = new StringBuilder();
			if (components.Length > 0)
			{
				string text = ((!IsPathRooted(path)) ? string.Empty : "/");
				text += components[0];
				stringBuilder.Append(GetRealPath(text));
			}
			for (int i = 1; i < lastIndex; i++)
			{
				stringBuilder.Append("/").Append(components[i]);
				string realPath = GetRealPath(stringBuilder.ToString());
				stringBuilder.Remove(0, stringBuilder.Length);
				stringBuilder.Append(realPath);
			}
			return stringBuilder.ToString();
		}

		public static string GetRealPath(string path)
		{
			while (true)
			{
				string text = ReadSymbolicLink(path);
				if (text == null)
				{
					break;
				}
				if (IsPathRooted(text))
				{
					path = text;
					continue;
				}
				path = GetDirectoryName(path) + DirectorySeparatorChar + text;
				path = GetCanonicalPath(path);
			}
			return path;
		}

		internal static string ReadSymbolicLink(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			int num;
			while (true)
			{
				num = Syscall.readlink(path, stringBuilder);
				if (num < 0)
				{
					Errno lastError;
					Errno errno = (lastError = Stdlib.GetLastError());
					if (errno == Errno.EINVAL)
					{
						return null;
					}
					UnixMarshal.ThrowExceptionForError(lastError);
				}
				else
				{
					if (num != stringBuilder.Capacity)
					{
						break;
					}
					stringBuilder.Capacity *= 2;
				}
			}
			return stringBuilder.ToString(0, num);
		}

		private static string ReadSymbolicLink(string path, out Errno errno)
		{
			errno = (Errno)0;
			StringBuilder stringBuilder = new StringBuilder(256);
			int num;
			while (true)
			{
				num = Syscall.readlink(path, stringBuilder);
				if (num < 0)
				{
					errno = Stdlib.GetLastError();
					return null;
				}
				if (num != stringBuilder.Capacity)
				{
					break;
				}
				stringBuilder.Capacity *= 2;
			}
			return stringBuilder.ToString(0, num);
		}

		public static string TryReadLink(string path)
		{
			Errno errno;
			return ReadSymbolicLink(path, out errno);
		}

		public static string ReadLink(string path)
		{
			Errno errno;
			path = ReadSymbolicLink(path, out errno);
			if (errno != 0)
			{
				UnixMarshal.ThrowExceptionForError(errno);
			}
			return path;
		}

		public static bool IsPathRooted(string path)
		{
			if (path == null || path.Length == 0)
			{
				return false;
			}
			return path[0] == DirectorySeparatorChar;
		}

		internal static void CheckPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException();
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Path cannot contain a zero-length string", "path");
			}
			if (path.IndexOfAny(_InvalidPathChars) != -1)
			{
				throw new ArgumentException("Invalid characters in path.", "path");
			}
		}
	}
}
