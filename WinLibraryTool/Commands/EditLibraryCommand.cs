using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WinLibraryTool.ViewModel;
using WinLibraryTool.UserControls;
using System.Windows.Controls;
using System.Windows;

namespace WinLibraryTool.Commands
{
	public class EditLibraryCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public EditLibraryCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			LibraryPropertiesControl userControl = new LibraryPropertiesControl(_viewModel.CurrentLibrary);
			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
			options.DialogType = WpfDialog.DialogType.Information;
			options.PossibleResponses = new WpfDialog.UserResponses(new string[] { "Close" }, 0);
			options.CustomContent = userControl;
			options.TitleBarIcon = ((Window)parameter).Icon;

			WpfDialog dialog = new WpfDialog("Edit library", "You can add local and network folders to this library.", options);
			dialog.Owner = (Window)parameter;
			dialog.ShowDialog();
		}

		public bool CanExecute(object parameter)
		{
			return (_viewModel.CanEditLibrary());
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
