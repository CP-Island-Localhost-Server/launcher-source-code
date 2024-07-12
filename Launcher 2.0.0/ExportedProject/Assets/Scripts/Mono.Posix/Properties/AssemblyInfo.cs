using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

[assembly: AssemblyDescription("Unix Integration Classes")]
[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]
[assembly: AssemblyDelaySign(true)]
[assembly: AssemblyKeyFile("../mono.pub")]
[assembly: AssemblyTitle("Mono.Posix.dll")]
[assembly: PermissionSet(SecurityAction.RequestMinimum, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SkipVerification\"/>\n</PermissionSet>\n")]
[assembly: AssemblyVersion("2.0.0.0")]
