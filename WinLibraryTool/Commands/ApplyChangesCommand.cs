using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;
using WinLibraryTool.ViewModel;
using WinLibraryTool.UserControls;
using System.Windows.Controls;
using System.Windows;

namespace WinLibraryTool.Commands
{
	public class ApplyChangesCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public ApplyChangesCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			CheckBox mirrorCheckBox = new CheckBox();
			string mirrorRoot = String.Format("{0}libraries", Environment.SystemDirectory.Substring(0, 3));
			mirrorCheckBox.Content = String.Format("Create a mirror of all libraries (using symbolic links) in {0}", mirrorRoot);
			if (System.IO.Directory.Exists(mirrorRoot))
			{
				mirrorCheckBox.Content += "\n(Existing directory will be erased.)";
			}
			mirrorCheckBox.IsChecked = false;

			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
			options.DialogType = WpfDialog.DialogType.Warning;
			options.CustomContent = mirrorCheckBox;
			options.PossibleResponses = new WpfDialog.UserResponses(new string[] { "Proceed", "Cancel" }, 1);
			options.TitleBarIcon = ((Window)parameter).Icon;

			WpfDialog dialog = new WpfDialog(Helpers.AssemblyProperties.AssemblyTitle, "Your existing library structure will now be backed-up, and the\nones defined in this tool will be created.\n\nIf any problem occurs during creation of the new libraries,\nthe backed-up copies will be restored.\n", options);
			dialog.Owner = (Window)parameter;
			dialog.ShowDialog();

			if (dialog.UserResponse.Equals("Proceed", StringComparison.CurrentCultureIgnoreCase))
			{
				bool appliedOK = false;

				try
				{
					Mouse.OverrideCursor = Cursors.Wait;
					_viewModel.ApplyChanges(mirrorCheckBox.IsChecked.Value);
					Mouse.OverrideCursor = Cursors.Arrow;
					appliedOK = true;
				}
				catch (System.Exception ex)
				{
					Mouse.OverrideCursor = Cursors.Arrow;	// revert cursor

					WpfDialog.WpfDialogOptions errorOptions = new WpfDialog.WpfDialogOptions();
					errorOptions.DialogType = WpfDialog.DialogType.Error;
					errorOptions.PossibleResponses = new WpfDialog.UserResponses(new string[] { "Bugger" });
					errorOptions.TitleBarIcon = ((Window)parameter).Icon;

					WpfDialog resultDialog = new WpfDialog(Helpers.AssemblyProperties.AssemblyTitle, "An error occurred whilst configuring the libraries.\n\n" + ex.Message, errorOptions);
					resultDialog.Owner = (Window)parameter;
					resultDialog.ShowDialog();
				}

				if (appliedOK)
				{
					CheckBox openCheckBox = new CheckBox();
					openCheckBox.Content = "Show me in Windows Explorer";
					openCheckBox.IsChecked = true;

					WpfDialog.WpfDialogOptions successOptions = new WpfDialog.WpfDialogOptions();
					successOptions.DialogType = WpfDialog.DialogType.Information;
					successOptions.PossibleResponses = new WpfDialog.UserResponses(new string[] { "Great" });
					successOptions.CustomContent = openCheckBox;
					successOptions.TitleBarIcon = ((Window)parameter).Icon;

					WpfDialog resultDialog = new WpfDialog(Helpers.AssemblyProperties.AssemblyTitle, "Your new libraries have been created successfully.\n", successOptions);
					resultDialog.Owner = (Window)parameter;
					resultDialog.ShowDialog();

					if (openCheckBox.IsChecked.Value)
					{
						Process.Start("explorer");
					}
				}
			}
		}

		public bool CanExecute(object parameter)
		{
			return _viewModel.CanApplyChanges();
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
