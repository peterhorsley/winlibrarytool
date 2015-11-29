using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinLibraryTool.Helpers
{
	class OSHelper
	{
		public static bool IsWindows7OrHigher()
		{
			return (Environment.OSVersion.Version.Major >= 6 &&
				Environment.OSVersion.Version.Minor >= 1);
		}
	}
}
