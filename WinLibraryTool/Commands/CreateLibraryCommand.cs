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
	public class CreateLibraryCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public CreateLibraryCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			bool created = false;
			bool cancelled = false;
			while (!cancelled && !created)
			{
				UserInputControl userControl = new UserInputControl("Library name:", "");
				WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
				options.DialogType = WpfDialog.DialogType.Question;
				options.PossibleResponses = new WpfDialog.UserResponses(new string[] { "Create", "Cancel" }, 0);
				options.CustomContent = userControl;
				options.TitleBarIcon = ((Window)parameter).Icon;

				WpfDialog dialog = new WpfDialog(Helpers.AssemblyProperties.AssemblyTitle, "Enter the name of the new library.", options);
				dialog.Owner = (Window)parameter;
				dialog.ShowDialog();

				if (dialog.UserResponse.Equals("Create", StringComparison.CurrentCultureIgnoreCase))
				{
					if (userControl.InputText.Length > 0)
					{
						_viewModel.CreateLibrary(userControl.InputText);
						created = true;
					}
					else
					{
						WpfDialog.WpfDialogOptions errorOptions = new WpfDialog.WpfDialogOptions();
						errorOptions.DialogType = WpfDialog.DialogType.Error;
						errorOptions.TitleBarIcon = ((Window)parameter).Icon;

						WpfDialog errorDialog = new WpfDialog(Helpers.AssemblyProperties.AssemblyTitle, "You must enter a name for the new library.", errorOptions);
						errorDialog.Owner = (Window)parameter;
						errorDialog.ShowDialog();
					}
				}
				else
				{
					cancelled = true;
				}
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
