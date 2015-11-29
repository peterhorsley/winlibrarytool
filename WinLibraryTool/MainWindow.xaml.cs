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
using System.Collections.ObjectModel;
using BusinessLib.DataAccess;
using BusinessLib.DataModel;
using WinLibraryTool.ViewModel;
using System.Xml.Linq;

namespace WinLibraryTool
{
	/// <summary>
    /// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			// Get raw library data.
			WinLibrary[] libraries = new WinLibrary[] { };

			// Create UI-friendly wrappers around the 
			// raw data objects (i.e. the view-model).
			LibrarySetViewModel _viewModel = new LibrarySetViewModel(libraries, this);

			// Let the UI bind to the view-model.
			base.DataContext = _viewModel;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			// This can't be done any earlier than the SourceInitialized event:
			if (!Helpers.WpfGlassHelper.ExtendGlassFrame(this, new Thickness(-1)))
			{
				this.Background = Brushes.WhiteSmoke;
				mainPanel.Margin = new Thickness(5);
			}
		}

		private void Window_StateChanged(object sender, EventArgs e)
		{
			if (this.WindowState == WindowState.Maximized)
			{
				mainPanel.Margin = new Thickness(5);
			}
			else
			{
				if (Helpers.WpfGlassHelper.IsGlassEnabled())
				{
					mainPanel.Margin = new Thickness(0);
				}
			}
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void libraryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			LibraryViewModel selectedLibrary = libraryTree.SelectedItem as LibraryViewModel;
			if (selectedLibrary == null)
			{
				FolderViewModel selectedFolder = libraryTree.SelectedItem as FolderViewModel;
				if (selectedFolder != null)
				{
					selectedLibrary = selectedFolder.Parent as LibraryViewModel;
				}
			}

			if (selectedLibrary != null)
			{
				((LibrarySetViewModel)DataContext).CurrentLibrary = selectedLibrary;
			}
		}

		private void btnDelete_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			btnDelete.Opacity = btnDelete.IsEnabled ? 1.0 : 0.5;
		}

		private void btnEdit_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			btnEdit.Opacity = btnEdit.IsEnabled ? 1.0 : 0.5;
		}

		private void btnSave_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			btnSave.Opacity = btnSave.IsEnabled ? 1.0 : 0.5;
		}
	}
}
