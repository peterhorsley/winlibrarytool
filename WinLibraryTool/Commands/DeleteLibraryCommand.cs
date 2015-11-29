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
	public class DeleteLibraryCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public DeleteLibraryCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			_viewModel.DeleteLibrary(_viewModel.CurrentLibrary);
		}

		public bool CanExecute(object parameter)
		{
			return (_viewModel.CanDeleteLibrary());
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
