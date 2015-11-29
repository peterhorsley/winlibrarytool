using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinLibraryTool.Helpers
{
	class FileSystemHelper
	{
		/// <summary>
		/// Removes any read-only attributes from files in a directory, optionally recursively.
		/// </summary>
		/// <param name="path">The directory from which to remove read-only attributes from files.</param>
		/// <param name="bRecurse">If true, attributes are changed for all sub-folders recursively.</param>
		/// <remarks>
		/// The function can be useful when you want to delete a directory that may or may not
		/// contain file that are read-only.
		/// </remarks>
		public static void MakeDirectoryReadWrite(string path, bool bRecurse)
		{
			string[] files = System.IO.Directory.GetFiles(path);

			foreach (string file in files)
			{
				System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
			}

			if (bRecurse)
			{
				string[] directories = System.IO.Directory.GetDirectories(path);
				foreach (string directory in directories)
				{
					MakeDirectoryReadWrite(directory, bRecurse);
				}
			}
		}
	}
}
