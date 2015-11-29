using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.UserControls
{
	/// <summary>
	/// Interaction logic for LibraryPropertiesControl.xaml
	/// </summary>
	public partial class LibraryPropertiesControl : UserControl
	{
		readonly LibraryViewModel _viewModel;

		// todo remove default constructor?
		public LibraryPropertiesControl()
			:this(null)
		{
		}

		public LibraryPropertiesControl(LibraryViewModel viewModel)
		{
			InitializeComponent();
			_viewModel = viewModel;

			// Let the UI bind to the view-model.
			base.DataContext = _viewModel;

			// todo - which one
			folderListBox.DataContext = _viewModel.Children;
			folderListBox.ItemsSource = _viewModel.Children;
			//folderListBox.DataContext = _viewModel.Folders;
			//folderListBox.ItemsSource = _viewModel.Folders;
		}

		private void folderListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_viewModel.CurrentFolder = (FolderViewModel)folderListBox.SelectedItem;
		}
	}
}
