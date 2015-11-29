using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Shell;
using BusinessLib.DataModel;
using BusinessLib.DataAccess;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Xml.Linq;

namespace BusinessLib.DataOperations
{
	public class WinLibraryOSHelper
	{
		private static string _backupPath;

		public static List<WinLibrary> EnumerateLibraries()
		{
			foreach (string libraryFilePath in WinLibraryOSHelper.GetAllLibraries())
			{
				// Library name is the actual file name!
				string libraryName = Path.GetFileNameWithoutExtension(libraryFilePath);
				using (ShellLibrary shellLibrary = ShellLibrary.Load(libraryName, false))
				{
					WinLibrarySetStorage.Libraries.Add(WinLibraryFromShellLibrary(shellLibrary));
				}
			}

			return WinLibrarySetStorage.Libraries;
		}

		private static WinLibrary WinLibraryFromShellLibrary(ShellLibrary shellLibrary)
		{
			WinLibrary winLibrary = new WinLibrary(shellLibrary.Name);

			foreach (ShellFolder folder in shellLibrary)
			{
				winLibrary.Folders.Add(folder.ParsingName);
			}

			try
			{
				winLibrary.SaveFolder = shellLibrary.DefaultSaveFolder;
			}
			catch
			{
				// Ignore - accessing a property should never throw an exception!
				// Hopefully this will be fixed by MS in the future.
				// Just pick the first folder as the default save folder (this is how
				// libraries work in Windows).

				if (winLibrary.Folders.Count > 0)
				{
					winLibrary.SaveFolder = winLibrary.Folders[0];
				}
			}

            try
            {
                winLibrary.IconReference = shellLibrary.IconResourceId;
            }
            catch
            {
                winLibrary.IconReference = new IconReference(WinLibrary.DefaultIconReference);
            }

			try
			{
				winLibrary.LibraryType = shellLibrary.LibraryType;
			}
			catch
			{
				// Ignore - accessing a property should never throw an exception!
				// Hopefully this will be fixed by MS in the future.
				// Just pick the first folder as the default save folder (this is how
				// libraries work in Windows).

				winLibrary.LibraryType = LibraryFolderType.Generic;
			}

			return winLibrary;
		}

		private static string[] GetAllLibraries()
		{
			return Directory.GetFiles(GetLibrariesStoragePath(), "*.library-ms");
		}

		private static string GetLibrariesStoragePath()
		{
			return Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				ShellLibrary.LibrariesKnownFolder.RelativePath);
		}

		private static void DeleteLibrary(string name)
		{
			string libraryPath = Path.Combine(GetLibrariesStoragePath(), name);
			string libraryFullPath = Path.ChangeExtension(libraryPath, "library-ms");

			File.Delete(libraryFullPath);
		}

		private static void BackupAllLibraries()
		{
			_backupPath = Path.Combine(Path.GetTempPath(), "Win7LibraryToolBackup");

			if (Directory.Exists(_backupPath))
			{
				Directory.Delete(_backupPath, true);
			}
			Directory.CreateDirectory(_backupPath);

			foreach (string file in GetAllLibraries())
			{
				File.Copy(file, Path.Combine(_backupPath, Path.GetFileName(file)));
			}
		}

		private static void RestoreAllLibraries()
		{
			if (Directory.Exists(_backupPath))
			{
				DeleteAllLibraries();
				foreach (string file in Directory.GetFiles(_backupPath))
				{
					File.Copy(file, Path.Combine(GetLibrariesStoragePath(), Path.GetFileName(file)));
				}
			}
		}

		private static void DeleteAllLibraries()
		{
			foreach (string file in GetAllLibraries())
			{
				File.Delete(file);
			}
		}

		public static void CreateLibraries(bool createSystemRootMirror)
		{
			BackupAllLibraries();
			DeleteAllLibraries();

			if (createSystemRootMirror)
			{
			    string mirrorRoot = Path.Combine(Environment.SystemDirectory.Substring(0, 3), "libraries");

			    // remove any existing mirror
			    if (Directory.Exists(mirrorRoot))
			    {
					// From MSDN / Directory.Delete:
					//
					// The behavior of this method differs slightly when deleting a directory that contains a 
					// reparse point, such as a symbolic link or a mount point. If the reparse point is a directory, 
					// such as a mount point, it is unmounted and the mount point is deleted. This method does not 
					// recurse through the reparse point. If the reparse point is a symbolic link to a file, the 
					// reparse point is deleted and not the target of the symbolic link.

					Directory.Delete(mirrorRoot, true);	// This must not delete any user data!
			    }

			    Directory.CreateDirectory(mirrorRoot);

			    foreach (WinLibrary library in WinLibrarySetStorage.Libraries)
			    {
					string libraryPath = Path.Combine(mirrorRoot, library.Name);
					Directory.CreateDirectory(libraryPath);

					for (int folderNum = 0; folderNum < library.Folders.Count; folderNum++)
					{
						string folderTarget = library.Folders[folderNum];
						string folderName = Path.GetFileName(folderTarget);
						string folderSource = Path.Combine(libraryPath, folderName);

						NativeMethods.CreateSymbolicLink(folderSource, folderTarget, NativeMethods.SYMLINK_FLAG_DIRECTORY);
					}
			    }
			}

			foreach (WinLibrary library in WinLibrarySetStorage.Libraries)
			{
				try
				{
					AddLibraryToSystem(library);
				}
				catch (System.Exception ex)
				{
					RestoreAllLibraries();	// Return library structure to original state if any error occurs.
					throw new LibraryCreationException(String.Format("Error creating library '{0}': {1}", library.Name, ex.Message));
				}
			}
		}

		private static void AddLibraryToSystem(WinLibrary library)
		{
			using (ShellLibrary shellLibrary = new ShellLibrary(library.Name, true))
			{
				shellLibrary.LibraryType = library.LibraryType;

				foreach (string folderPath in library.Folders)
				{
					try
					{
						shellLibrary.Add(folderPath);
					}
					catch (System.Exception ex)
					{
						throw new LibraryCreationException(String.Format("Failed to add folder '{0}'.\n\nError: {1}", folderPath, ex.Message));
					}
				}

				if (!String.IsNullOrEmpty(library.SaveFolder))
				{
					try
					{
						shellLibrary.DefaultSaveFolder = library.SaveFolder;
					}
					catch
					{
						// Certain folders cannot be used as the save folder (e.g. read-only folders).
						// Ignore - setting a property should never throw an exception!
						// Hopefully this will be fixed by MS in the future.
					}
				}

				if (!String.IsNullOrEmpty(library.IconReference.ReferencePath))
				{
					shellLibrary.IconResourceId  = library.IconReference;
				}

				// Library is now created in OS.
			}

            // Note: This is a temporary *hack* whilst I wait for feedback for how to set
            // properties on the library.  See this thread:
            // http://code.msdn.microsoft.com/WindowsAPICodePack/Thread/View.aspx?ThreadId=2998

            string libraryFilePath = Path.Combine(GetLibrariesStoragePath(), library.Name + ".library-ms");

			var doc = XDocument.Load(libraryFilePath);

			XNamespace ns = "http://schemas.microsoft.com/windows/2009/library";
			XElement xe1 = new XElement(ns + "property", new XCData("false"));
			xe1.Add(new XAttribute("name", "ShowNonIndexedLocationsInfoBar"));
			xe1.Add(new XAttribute("type", "boolean"));
			doc.Descendants(ns + "propertyStore").First().Add(xe1);

			doc.Save(libraryFilePath);
		}
	}
}
