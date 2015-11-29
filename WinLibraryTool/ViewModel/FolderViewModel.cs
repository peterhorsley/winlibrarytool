using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WinLibraryTool.ViewModel
{
	public class FolderViewModel : TreeViewItemViewModel
	{
		readonly string _folder;
		private bool _isSaveLocation;

		public FolderViewModel(string folder, LibraryViewModel ownerLibrary)
			: base (ownerLibrary, false)
		{
			_folder = folder;
		}

		public string FolderName
		{
			get { return _folder; }
		}

		public bool IsSaveLocation
		{
			get { return _isSaveLocation; }
			set
			{
				if (value != _isSaveLocation)
				{
					_isSaveLocation = value;
					this.OnPropertyChanged("IsSaveLocation");
				}
			}
		}
	}
}
