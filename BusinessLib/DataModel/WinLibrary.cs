using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Shell;

namespace BusinessLib.DataModel
{
	public class WinLibrary
	{
		private string _name;
		private List<string> _folders;
		private string _saveFolder;
		private IconReference _iconReference;
		private LibraryFolderType _libraryType;

		public static string DefaultIconReference = "imageres.dll,-1001";

		public static Dictionary<LibraryFolderType, string> LibraryTypes;

		static WinLibrary()
		{
			LibraryTypes = new Dictionary<LibraryFolderType, string>();
			LibraryTypes.Add(LibraryFolderType.Generic, "General Items");
			LibraryTypes.Add(LibraryFolderType.Documents, "Documents");
			LibraryTypes.Add(LibraryFolderType.Pictures, "Pictures");
			LibraryTypes.Add(LibraryFolderType.Music, "Music");
			LibraryTypes.Add(LibraryFolderType.Videos, "Videos");
		}

		// Parameterless constructor provided for serialization only.
		public WinLibrary()
		{
		}

		public WinLibrary(string name)
			: this(name, null, String.Empty, new IconReference(DefaultIconReference))
		{
		}

		public WinLibrary(string name, List<string> folders, string saveFolder, IconReference iconReference)
		{
			_name = name;
			_saveFolder = saveFolder;
			_iconReference = iconReference;
			_libraryType = LibraryFolderType.Generic;

			if (folders == null)
			{
				_folders = new List<string>();
			}
			else
			{
				_folders = folders;
			}
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public List<string> Folders
		{
			get { return _folders; }
			set { _folders = value; }
		}

		public string SaveFolder
		{
			get { return _saveFolder; }
			set { _saveFolder = value; }
		}

		public IconReference IconReference
		{
			get { return _iconReference; }
			set { _iconReference = value; }
		}

		public LibraryFolderType LibraryType
		{
			get { return _libraryType; }
			set { _libraryType = value; }
		}
	}
}
