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
	public class BrowseCommand : ICommand
	{
		readonly LibraryIconSelectorControl _viewModel;

		public BrowseCommand(LibraryIconSelectorControl viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			CommonOpenFileDialog fd = new CommonOpenFileDialog("Select icon file or executable:");
			fd.Filters.Add(new CommonFileDialogFilter("Icon files", "*.ico,*.dll,*.exe"));

			if (fd.ShowDialog() == CommonFileDialogResult.OK)
			{
				_viewModel.AddIcons(fd.FileName);
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
