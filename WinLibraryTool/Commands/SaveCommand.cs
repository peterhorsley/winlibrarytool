using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WinLibraryTool.ViewModel;
using WinLibraryTool.UserControls;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WinLibraryTool.Commands
{
	public class SaveCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;
		private readonly string _fileExtension = ".winLibraries";

		public SaveCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			CommonSaveFileDialog fd = new CommonSaveFileDialog("Select where to save the library set:");
			fd.Filters.Add(new CommonFileDialogFilter("WinLibrary Sets", "*.winLibraries"));
			if (fd.ShowDialog() == CommonFileDialogResult.OK)
			{
				// ensure file extension is included.
				string filePath = fd.FileName;
				if (!filePath.EndsWith(_fileExtension, StringComparison.CurrentCultureIgnoreCase))
				{
					filePath = filePath + _fileExtension;
				}

				try
				{
					_viewModel.Save(filePath);
				}
				catch (System.Exception ex)
				{
					WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
					options.DialogType = WpfDialog.DialogType.Error;
					WpfDialog dialog = new WpfDialog("Win Library Tool",
						String.Format("An error occurred trying to save libraries to:\n\n{0}\n\nError:{1}", filePath, ex.Message),
						options);
				}
			}
		}

		public bool CanExecute(object parameter)
		{
			return (_viewModel.CanSave());
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
