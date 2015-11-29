using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BusinessLib.DataModel;
using System.Windows.Input;
using System.Drawing;
using System.Windows;
using WinLibraryTool.Helpers.CodeHelpers.IconHelper;
using Microsoft.WindowsAPICodePack.Shell;

namespace WinLibraryTool.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper around a WinLibrary object.
    /// </summary>
    public class LibraryViewModel :  TreeViewItemViewModel
    {
		#region Data

		private readonly WinLibrary _winLibrary;
		private readonly ICommand _setSaveCommand;
		private readonly ICommand _removeCommand;
		private readonly ICommand _includeFolderCommand;
		private readonly ICommand _chooseIconCommand;
		private readonly Window _userInterface = null;

		#endregion // Data

	   public LibraryViewModel(WinLibrary winLibrary, Window userInterface) 
            : base(null, true)
        {
			_winLibrary = winLibrary;
			_userInterface = userInterface;

			_setSaveCommand = new Commands.SetSaveCommand(this);
			_removeCommand = new Commands.RemoveFolderCommand(this);
			_includeFolderCommand = new Commands.IncludeFolderCommand(this);
			_chooseIconCommand = new Commands.ChooseIconCommand(this);
		}

		public LibraryFolderType LibraryType
		{
			get { return _winLibrary.LibraryType; }
			set
			{
				_winLibrary.LibraryType = value;
				this.OnPropertyChanged("LibraryType");
			}
		}

		public Dictionary<LibraryFolderType, string> LibraryTypes
		{
			get { return WinLibrary.LibraryTypes; }
		} 

		public Window UserInterface
		{
			get { return _userInterface; }
		}

		public ICommand ChooseIconCommand
		{
			get { return _chooseIconCommand; }
		}

		public ICommand SetSaveCommand
		{
			get { return _setSaveCommand; }
		}

		public ICommand RemoveCommand
		{
			get { return _removeCommand; }
		}

		public ICommand IncludeFolderCommand
		{
			get { return _includeFolderCommand; }
		}

		public WinLibrary WinLibrary
		{
			get { return _winLibrary; }
		}

		// todo: refactor out
        public string Name
        {
            get { return _winLibrary.Name; }
			set
			{
				if (value != _winLibrary.Name)
				{
					_winLibrary.Name = value;
					this.OnPropertyChanged("Name");
				}
			}
        }

		public IconReference IconReference
		{
			get
			{
				return _winLibrary.IconReference;
			}
			set
			{
				_winLibrary.IconReference = value;
				this.OnPropertyChanged("IconReference");
			}
		}

		//public ObservableCollection<FolderViewModel> Folders
		//{
		//    get { return _folders; }
		//}

		// todo: refactor out
		public string SaveFolder
		{
			get { return _winLibrary.SaveFolder; }
			set { _winLibrary.SaveFolder = value; }
		}

		public FolderViewModel CurrentFolder { get; set; }

		public bool CanSetSave() { return (this.CurrentFolder != null); }

        protected override void LoadChildren()
        {
            foreach (string folder in _winLibrary.Folders)
                base.Children.Add(new FolderViewModel(folder, this));

			SetSaveFolder(_winLibrary.SaveFolder);
        }

		public bool IncludeFolder(string path)
		{
			FolderViewModel viewModel = GetFolderViewModel(path);
			if (viewModel == null)
			{
				// todo: change folder collection to dictionary
				_winLibrary.Folders.Add(path);	// update data model
				viewModel = new FolderViewModel(path, this);
				viewModel.IsSelected = true;
				base.Children.Add(viewModel);
				return true;
			}

			return false;
		}

		public bool RemoveFolder(string path)
		{
			FolderViewModel viewModel = GetFolderViewModel(path);
			if (viewModel != null)
			{
				_winLibrary.Folders.Remove(path);	// update data model
				base.Children.Remove(viewModel);
				return true;
			}

			return false;
		}

		public void SetSaveFolder(string path)
		{
			foreach (FolderViewModel viewModel in base.Children)
			{
				viewModel.IsSaveLocation = (viewModel.FolderName.Equals(path, StringComparison.CurrentCultureIgnoreCase));
			}

			_winLibrary.SaveFolder = path;	// update data model
		}

		public void SetIcon(IconReference iconReference)
		{
			this.IconReference = iconReference;
		}

		private FolderViewModel GetFolderViewModel(string path)
		{
			// todo: use dictionary lookup / linq
			foreach (FolderViewModel viewModel in base.Children)
			{
				if (viewModel.FolderName.Equals(path, StringComparison.CurrentCultureIgnoreCase))
				{
					return viewModel;
				}
			}

			//IEnumerable<TreeViewItemViewModel> qry = from c in base.Children
			//                                   where c.FolderName.Equals(path, StringComparison.CurrentCultureIgnoreCase)
			//                                   select c;

			//ObservableCollection<FolderViewModel> queryResults = new ObservableCollection<FolderViewModel>(qry);

			//if (queryResults.Count == 1)
			//{
			//    return queryResults[0];
			//}

			return null;
		}
    }
}
