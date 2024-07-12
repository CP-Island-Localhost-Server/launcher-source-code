namespace Mono.Unix.Native
{
	internal class XPrintfFunctions
	{
		internal delegate object XPrintf(object[] parameters);

		internal static XPrintf printf;

		internal static XPrintf fprintf;

		internal static XPrintf snprintf;

		internal static XPrintf syslog;

		static XPrintfFunctions()
		{
			CdeclFunction @object = new CdeclFunction("msvcrt", "printf", typeof(int));
			printf = @object.Invoke;
			CdeclFunction object2 = new CdeclFunction("msvcrt", "fprintf", typeof(int));
			fprintf = object2.Invoke;
			CdeclFunction object3 = new CdeclFunction("MonoPosixHelper", "Mono_Posix_Stdlib_snprintf", typeof(int));
			snprintf = object3.Invoke;
			CdeclFunction object4 = new CdeclFunction("MonoPosixHelper", "Mono_Posix_Stdlib_syslog2", typeof(int));
			syslog = object4.Invoke;
		}
	}
}
