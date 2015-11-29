using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BusinessLib.DataModel;
using BusinessLib.DataAccess;
using BusinessLib.DataOperations;
using System.Windows;
using Microsoft.WindowsAPICodePack.Shell;

namespace WinLibraryTool.ViewModel
{
    /// <summary>
	/// The ViewModel for WinLibraryTool.  This simply
	/// exposes a read-only collection of WinLibraries.
    /// </summary>
    public class LibrarySetViewModel
    {
        #region Data

		readonly ObservableCollection<LibraryViewModel> _libraries;
		private LibraryViewModel _currentLibrary = null;
		private readonly Window _userInterface = null;
		readonly ICommand _createLibraryCommand;
		readonly ICommand _editLibraryCommand;
		readonly ICommand _deleteLibraryCommand;
		readonly ICommand _loadCommand;
		readonly ICommand _loadExistingCommand;
		readonly ICommand _saveCommand;
		readonly ICommand _applyChangesCommand;
		readonly ICommand _helpCommand;

        #endregion // Data

        #region Constructor

		public LibrarySetViewModel(WinLibrary[] libraries, Window userInterface)
        {
			_userInterface = userInterface;

			_libraries = new ObservableCollection<LibraryViewModel>(
				(from library in libraries
				 select new LibraryViewModel(library, _userInterface))
				.ToList());

			_createLibraryCommand = new Commands.CreateLibraryCommand(this);
			_editLibraryCommand = new Commands.EditLibraryCommand(this);
			_deleteLibraryCommand = new Commands.DeleteLibraryCommand(this);
			_loadCommand = new Commands.LoadCommand(this);
			_saveCommand = new Commands.SaveCommand(this);
			_applyChangesCommand = new Commands.ApplyChangesCommand(this);
			_loadExistingCommand = new Commands.LoadExistingCommand(this);
			_helpCommand = new Commands.HelpCommand();
		}

        #endregion // Constructor

		#region Properties

		public Window UserInterface
		{
			get { return _userInterface; }
		}

		public ICommand HelpCommand
		{
			get { return _helpCommand; }
		} 

		public ICommand LoadCommand
		{
			get { return _loadCommand; }
		}

		public ICommand LoadExistingCommand
		{
			get { return _loadExistingCommand; }
		}

		public ICommand SaveCommand
		{
			get { return _saveCommand; }
		}

		public ICommand ApplyChangesCommand
		{
			get { return _applyChangesCommand; }
		}

		public ICommand CreateLibraryCommand
		{
			get { return _createLibraryCommand; }
		}

		public ICommand EditLibraryCommand
		{
			get { return _editLibraryCommand; }
		}

		public ICommand DeleteLibraryCommand
		{
			get { return _deleteLibraryCommand; }
		}

		public ObservableCollection<LibraryViewModel> Libraries
		{
			get { return _libraries; }
		}

		public LibraryViewModel CurrentLibrary
		{
			get
			{
				return _currentLibrary;
			}
			set
			{
				_currentLibrary = value;
				if (_currentLibrary != null)
				{
					_currentLibrary.IsSelected = true;
					_currentLibrary.IsExpanded = true;
				}
			}
		}

		#endregion // Properties

		#region Public Methods

		public void ApplyChanges(bool createSystemRootMirror)
		{
			WinLibraryOSHelper.CreateLibraries(createSystemRootMirror);
		}

		public void CreateLibrary(string libraryName)
		{
			WinLibrary winLibrary = new WinLibrary(libraryName);
			LibraryViewModel library = new LibraryViewModel(winLibrary, _userInterface);
			library.IsSelected = true;

			WinLibrarySetStorage.Libraries.Add(winLibrary);
			_libraries.Add(library);
			CurrentLibrary = library;
		}

		public void DeleteLibrary(LibraryViewModel library)
		{
			WinLibrarySetStorage.Libraries.Remove(library.WinLibrary);
			_libraries.Remove(library);
			if (_libraries.Count == 0)
			{
				_currentLibrary = null;
			}
		}

		public void Save(string filePath)
		{
			WinLibrarySetStorage.SaveLibraries(filePath);
		}

		public void Load(string filePath)
		{
			_libraries.Clear();
			WinLibrarySetStorage.Libraries.Clear();
			WinLibrarySetStorage.LoadLibraries(filePath);
			AddLibraries();
		}

		public void LoadExisting()
		{
			_libraries.Clear();
			WinLibraryOSHelper.EnumerateLibraries();
			AddLibraries();
		}

		private void AddLibraries()
		{
			foreach (WinLibrary library in WinLibrarySetStorage.Libraries)
			{
				string iconPath = library.IconReference.ModuleName;
				if (Path.IsPathRooted(iconPath))
				{
					if (!File.Exists(iconPath))
					{
						var options = new WpfDialog.WpfDialogOptions();
						options.DialogType = WpfDialog.DialogType.Error;
						var dialog = new WpfDialog("Windows Library Tool", String.Format("The icon for library '{0}' cannot be found, reverting to default icon.  Path:\n\n{1}", library.Name, iconPath), options);
						dialog.ShowDialog();
						library.IconReference = new IconReference(WinLibrary.DefaultIconReference);
					}
				}

				LibraryViewModel viewModel = new LibraryViewModel(library, _userInterface);
				viewModel.IsExpanded = true;
				_libraries.Add(viewModel);
			}
		}

		public bool CanDeleteLibrary() { return (this.CurrentLibrary != null); }
		public bool CanEditLibrary() { return (this.CurrentLibrary != null); }
		public bool CanSave() { return (this.Libraries.Count > 0); }
		public bool CanApplyChanges() { return (this.Libraries.Count > 0); }
		
		#endregion // Public Methods
	}
}
