﻿using System;
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
	public class SetSaveCommand : ICommand
	{
		readonly LibraryViewModel _viewModel;

		public SetSaveCommand(LibraryViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			_viewModel.SetSaveFolder(_viewModel.CurrentFolder.FolderName);
		}

		public bool CanExecute(object parameter)
		{
			return (_viewModel.CanSetSave());
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
