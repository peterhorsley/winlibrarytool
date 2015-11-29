using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WinLibraryTool.ViewModel;
using WinLibraryTool.UserControls;
using System.Windows.Controls;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WinLibraryTool.Commands
{
	public class ChooseIconCommand : ICommand
	{
		readonly LibraryViewModel _viewModel;

		public ChooseIconCommand(LibraryViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
			options.DialogType = WpfDialog.DialogType.Information;
			options.PossibleResponses = new WpfDialog.UserResponses(new string[] { "OK", "Cancel" }, 1);
			options.TitleBarIcon = ((Window)parameter).Icon;
			LibraryIconSelectorControl selectorControl = new LibraryIconSelectorControl(_viewModel, (Window)parameter);
			options.CustomContent = selectorControl;

			WpfDialog dialog = new WpfDialog(Helpers.AssemblyProperties.AssemblyTitle, "Choose an icon for this library.", options);
			dialog.Owner = (Window)parameter;
			dialog.ShowDialog();

			if (dialog.UserResponse.Equals("OK", StringComparison.CurrentCultureIgnoreCase))
			{
				_viewModel.SetIcon(selectorControl.CurrentIconReference);
			}		
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged
		{
			// I intentionally left these empty because
			// this command never raises the event, and
			// not using the WeakEvent pattern here can
			// cause memory leaks.  WeakEvent pattern is
			// not simple to implement, so why bother.
			add { }
			remove { }
		}

		#endregion
	}
}
