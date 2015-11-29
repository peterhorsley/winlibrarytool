using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BusinessLib
{
	internal class NativeMethods
	{
		#region kernel32.dll

		[DllImport("kernel32.dll")]
		internal static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

		internal static int SYMLINK_FLAG_DIRECTORY = 1;

		#endregion // kernel32.dll
	}
}
