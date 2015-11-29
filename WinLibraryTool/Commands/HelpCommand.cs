using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WinLibraryTool.ViewModel;
using WinLibraryTool.UserControls;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Diagnostics;

namespace WinLibraryTool.Commands
{
	public class HelpCommand : ICommand
	{
		public HelpCommand()
		{
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			TextBlock infoText = new TextBlock();
			infoText.Inlines.Add("This tool provides the following features not available in Windows:\n\n" +
			                        "      - Add network (UNC or mapped drive) and any other un-indexed folders to libraries.\n" +
									"      - Backup library configuration, such that a saved set of libraries can be instantly\n" + 
									"        restored at any point.\n" +
									"      - Create a mirror of all libraries (using symbolic links) in [SystemDrive]:\\libraries.\n" +
									"      - Change a library's icon.\n\n");

			Run webAddress = new Run("http://zornsoftware.codenature.info");
			Hyperlink link = new Hyperlink(webAddress);
			link.NavigateUri = new Uri(webAddress.Text);
			link.Click += new RoutedEventHandler(link_Click);
			infoText.Inlines.Add(link);

			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
			options.DialogType = WpfDialog.DialogType.Information;
			options.DialogIcon = ((Window)parameter).Icon;
			options.PossibleResponses = new WpfDialog.UserResponses(new string[] { "OK" });
			options.TitleBarIcon = ((Window)parameter).Icon;
			options.CustomContent = infoText;

			WpfDialog dialog = new WpfDialog(
				Helpers.AssemblyProperties.AssemblyTitle, 
				String.Format("{0} v{1}\n{2}\n{3}", 
					Helpers.AssemblyProperties.AssemblyTitle, 
					Helpers.AssemblyProperties.AssemblyVersion, 
					Helpers.AssemblyProperties.AssemblyDescription, 
					Helpers.AssemblyProperties.AssemblyCopyright), 
				options);

			dialog.Owner = (Window)parameter;
			dialog.ShowDialog();
		}

		void link_Click(object sender, RoutedEventArgs e)
		{
			// open URL
			Hyperlink source = sender as Hyperlink;
			if (source != null)
			{
				Process.Start(source.NavigateUri.ToString());
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
