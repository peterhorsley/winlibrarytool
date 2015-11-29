using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace WinLibraryTool
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			if (!Helpers.OSHelper.IsWindows7OrHigher())
			{
				WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
				options.DialogType = WpfDialog.DialogType.Error;
				WpfDialog dialog = new WpfDialog("Windows Library Tool", "Sorry, this program is only useful on >= Windows 7.", options);
				dialog.ShowDialog();
				Environment.Exit(-1);
			}
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			DateTime now = DateTime.Now;
			string logFileName = String.Format("winlibraryTool_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}.log", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
			string logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WinLibraryTool");
			string logFilePath = Path.Combine(logFolder, logFileName);

			if (!Directory.Exists(logFolder))
			{
				Directory.CreateDirectory(logFolder);
			}
			File.WriteAllText(logFilePath, e.ExceptionObject.ToString());

			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions();
			options.DialogType = WpfDialog.DialogType.Error;
			WpfDialog dialog = new WpfDialog("Windows Library Tool", String.Format("Sorry, an unexpected error has occurred.\n\nInformation that will help solve this problem has been saved to:\n\n{0}\n\nPlease send this problem report file to peter.g.horsley@gmail.com for assistance.", logFilePath), options);
			dialog.ShowDialog();
			Process.Start(Path.GetDirectoryName(logFilePath));
			Environment.Exit(-1);
		}
	}
}
