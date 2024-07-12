using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Mono.Unix.Native;

namespace Mono.Unix
{
	public sealed class UnixMarshal
	{
		private UnixMarshal()
		{
		}

		[CLSCompliant(false)]
		public static string GetErrorDescription(Errno errno)
		{
			return ErrorMarshal.Translate(errno);
		}

		public static IntPtr AllocHeap(long size)
		{
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("size", "< 0");
			}
			return Stdlib.malloc((ulong)size);
		}

		public static IntPtr ReAllocHeap(IntPtr ptr, long size)
		{
			if (size < 0)
			{
				throw new ArgumentOutOfRangeException("size", "< 0");
			}
			return Stdlib.realloc(ptr, (ulong)size);
		}

		public static void FreeHeap(IntPtr ptr)
		{
			Stdlib.free(ptr);
		}

		public unsafe static string PtrToStringUnix(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			int length = checked((int)Stdlib.strlen(p));
			return new string((sbyte*)(void*)p, 0, length, UnixEncoding.Instance);
		}

		public static string PtrToString(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return PtrToString(p, UnixEncoding.Instance);
		}

		public unsafe static string PtrToString(IntPtr p, Encoding encoding)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			int stringByteLength = GetStringByteLength(p, encoding);
			string text = new string((sbyte*)(void*)p, 0, stringByteLength, encoding);
			stringByteLength = text.Length;
			while (stringByteLength > 0 && text[stringByteLength - 1] == '\0')
			{
				stringByteLength--;
			}
			if (stringByteLength == text.Length)
			{
				return text;
			}
			return text.Substring(0, stringByteLength);
		}

		private static int GetStringByteLength(IntPtr p, Encoding encoding)
		{
			Type type = encoding.GetType();
			int num = -1;
			num = ((typeof(UTF8Encoding).IsAssignableFrom(type) || typeof(UTF7Encoding).IsAssignableFrom(type) || typeof(UnixEncoding).IsAssignableFrom(type) || typeof(ASCIIEncoding).IsAssignableFrom(type)) ? checked((int)Stdlib.strlen(p)) : ((!typeof(UnicodeEncoding).IsAssignableFrom(type)) ? GetRandomBufferLength(p, encoding.GetMaxByteCount(1)) : GetInt16BufferLength(p)));
			if (num == -1)
			{
				throw new NotSupportedException("Unable to determine native string buffer length");
			}
			return num;
		}

		private static int GetInt16BufferLength(IntPtr p)
		{
			int i;
			for (i = 0; Marshal.ReadInt16(p, i * 2) != 0; i = checked(i + 1))
			{
			}
			return checked(i * 2);
		}

		private static int GetInt32BufferLength(IntPtr p)
		{
			int i;
			for (i = 0; Marshal.ReadInt32(p, i * 4) != 0; i = checked(i + 1))
			{
			}
			return checked(i * 4);
		}

		private static int GetRandomBufferLength(IntPtr p, int nullLength)
		{
			switch (nullLength)
			{
			case 1:
				return checked((int)Stdlib.strlen(p));
			case 2:
				return GetInt16BufferLength(p);
			case 4:
				return GetInt32BufferLength(p);
			default:
			{
				int result = 0;
				int num = 0;
				do
				{
					num = ((Marshal.ReadByte(p, result++) == 0) ? (num + 1) : 0);
				}
				while (num != nullLength);
				return result;
			}
			}
		}

		public static string[] PtrToStringArray(IntPtr stringArray)
		{
			return PtrToStringArray(stringArray, UnixEncoding.Instance);
		}

		public static string[] PtrToStringArray(IntPtr stringArray, Encoding encoding)
		{
			if (stringArray == IntPtr.Zero)
			{
				return new string[0];
			}
			int count = CountStrings(stringArray);
			return PtrToStringArray(count, stringArray, encoding);
		}

		private static int CountStrings(IntPtr stringArray)
		{
			int i;
			for (i = 0; Marshal.ReadIntPtr(stringArray, i * IntPtr.Size) != IntPtr.Zero; i++)
			{
			}
			return i;
		}

		public static string[] PtrToStringArray(int count, IntPtr stringArray)
		{
			return PtrToStringArray(count, stringArray, UnixEncoding.Instance);
		}

		public static string[] PtrToStringArray(int count, IntPtr stringArray, Encoding encoding)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (stringArray == IntPtr.Zero)
			{
				return new string[count];
			}
			string[] array = new string[count];
			for (int i = 0; i < count; i++)
			{
				IntPtr p = Marshal.ReadIntPtr(stringArray, i * IntPtr.Size);
				array[i] = PtrToString(p, encoding);
			}
			return array;
		}

		public static IntPtr StringToHeap(string s)
		{
			return StringToHeap(s, UnixEncoding.Instance);
		}

		public static IntPtr StringToHeap(string s, Encoding encoding)
		{
			return StringToHeap(s, 0, s.Length, encoding);
		}

		public static IntPtr StringToHeap(string s, int index, int count)
		{
			return StringToHeap(s, index, count, UnixEncoding.Instance);
		}

		public static IntPtr StringToHeap(string s, int index, int count, Encoding encoding)
		{
			if (s == null)
			{
				return IntPtr.Zero;
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			int maxByteCount = encoding.GetMaxByteCount(1);
			char[] array = s.ToCharArray(index, count);
			byte[] array2 = new byte[encoding.GetByteCount(array) + maxByteCount];
			int bytes = encoding.GetBytes(array, 0, array.Length, array2, 0);
			if (bytes != array2.Length - maxByteCount)
			{
				throw new NotSupportedException("encoding.GetBytes() doesn't equal encoding.GetByteCount()!");
			}
			IntPtr intPtr = AllocHeap(array2.Length);
			if (intPtr == IntPtr.Zero)
			{
				throw new UnixIOException(Errno.ENOMEM);
			}
			bool flag = false;
			try
			{
				Marshal.Copy(array2, 0, intPtr, array2.Length);
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					FreeHeap(intPtr);
				}
			}
			return intPtr;
		}

		public static bool ShouldRetrySyscall(int r)
		{
			if (r == -1 && Stdlib.GetLastError() == Errno.EINTR)
			{
				return true;
			}
			return false;
		}

		[CLSCompliant(false)]
		public static bool ShouldRetrySyscall(int r, out Errno errno)
		{
			errno = (Errno)0;
			if (r == -1 && (errno = Stdlib.GetLastError()) == Errno.EINTR)
			{
				return true;
			}
			return false;
		}

		internal static string EscapeFormatString(string message, char[] permitted)
		{
			if (message == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(message.Length);
			for (int i = 0; i < message.Length; i++)
			{
				char c = message[i];
				stringBuilder.Append(c);
				if (c == '%' && i + 1 < message.Length)
				{
					char c2 = message[i + 1];
					if (c2 == '%' || IsCharPresent(permitted, c2))
					{
						stringBuilder.Append(c2);
					}
					else
					{
						stringBuilder.Append('%').Append(c2);
					}
					i++;
				}
				else if (c == '%')
				{
					stringBuilder.Append('%');
				}
			}
			return stringBuilder.ToString();
		}

		private static bool IsCharPresent(char[] array, char c)
		{
			if (array == null)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == c)
				{
					return true;
				}
			}
			return false;
		}

		internal static Exception CreateExceptionForError(Errno errno)
		{
			string errorDescription = GetErrorDescription(errno);
			UnixIOException ex = new UnixIOException(errno);
			switch (errno)
			{
			case Errno.EBADF:
			case Errno.EINVAL:
				return new ArgumentException(errorDescription, ex);
			case Errno.ERANGE:
				return new ArgumentOutOfRangeException(errorDescription);
			case Errno.ENOTDIR:
				return new DirectoryNotFoundException(errorDescription, ex);
			case Errno.ENOENT:
				return new FileNotFoundException(errorDescription, ex);
			case Errno.EPERM:
			case Errno.EOPNOTSUPP:
				return new InvalidOperationException(errorDescription, ex);
			case Errno.ENOEXEC:
				return new InvalidProgramException(errorDescription, ex);
			case Errno.EIO:
			case Errno.ENXIO:
			case Errno.ENOSPC:
			case Errno.ESPIPE:
			case Errno.EROFS:
			case Errno.ENOTEMPTY:
				return new IOException(errorDescription, ex);
			case Errno.EFAULT:
				return new NullReferenceException(errorDescription, ex);
			case Errno.EOVERFLOW:
				return new OverflowException(errorDescription, ex);
			case Errno.ENAMETOOLONG:
				return new PathTooLongException(errorDescription, ex);
			case Errno.EACCES:
			case Errno.EISDIR:
				return new UnauthorizedAccessException(errorDescription, ex);
			default:
				return ex;
			}
		}

		internal static Exception CreateExceptionForLastError()
		{
			return CreateExceptionForError(Stdlib.GetLastError());
		}

		[CLSCompliant(false)]
		public static void ThrowExceptionForError(Errno errno)
		{
			throw CreateExceptionForError(errno);
		}

		public static void ThrowExceptionForLastError()
		{
			throw CreateExceptionForLastError();
		}

		[CLSCompliant(false)]
		public static void ThrowExceptionForErrorIf(int retval, Errno errno)
		{
			if (retval == -1)
			{
				ThrowExceptionForError(errno);
			}
		}

		public static void ThrowExceptionForLastErrorIf(int retval)
		{
			if (retval == -1)
			{
				ThrowExceptionForLastError();
			}
		}
	}
}
