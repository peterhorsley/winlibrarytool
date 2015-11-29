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
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Shell;

// todo: fix namespace
namespace WinLibraryTool
{
	/// <summary>
	/// Interaction logic for WpfDialog.xaml
	/// </summary>
	public partial class WpfDialog : Window
	{
		public enum DialogType
		{
			Information,
			Question,
			Warning,
			Error,
		}

		#region WpfDialogOptions class

		public class WpfDialogOptions
		{
			public UIElement CustomContent { get; set; }
			public UserResponses PossibleResponses { get; set; }
			public DialogType DialogType { get; set; }
			public ImageSource DialogIcon { get; set; }
			public ImageSource TitleBarIcon { get; set; }
		}

		#endregion // WpfDialogOptions class

		#region UserResponses class

		public class UserResponses
		{
			private string[] _responseNames;
			private int _defaultResponseIndex;

			public UserResponses(string[] responseNames)
				:this(responseNames, 0)
			{
			}

			public UserResponses(string[] responseNames, int defaultResponseIndex)
			{
				_responseNames = responseNames;
				_defaultResponseIndex = defaultResponseIndex;
			}

			public string[] ResponseNames
			{
				get { return _responseNames; }
			}

			public int DefaultResponseIndex
			{
				get { return _defaultResponseIndex; }
			}
		}

		#endregion // UserResponses class

		private string _userResponse = String.Empty;

		public WpfDialog()
			: this(String.Empty, String.Empty)
		{
		}

		public WpfDialog(string title, string description)
			: this(title, description, null)
		{
		}

		public WpfDialog(string title, string description, WpfDialogOptions options)
		{
			InitializeComponent();

			DialogTitle = title;
			Description = description;

			if (options != null)
			{
				if (options.CustomContent != null)
				{
					UserContentArea.Children.Clear();
					UserContentArea.Children.Add(options.CustomContent);
				}
				else
				{
					UserContentArea.Visibility = Visibility.Collapsed;
				}

				if (options.PossibleResponses != null)
				{
					buttonPanel.Children.Clear();

					UserResponses responses = options.PossibleResponses;
					for (int responseNum = 0; responseNum < responses.ResponseNames.Length; responseNum++)
					{
						Button btnResponse = new Button();
						btnResponse.Content = responses.ResponseNames[responseNum];
						btnResponse.Height = 23;
						btnResponse.MinWidth = 70;
						btnResponse.Click += new RoutedEventHandler(btnResponse_Click);

						if (responseNum == responses.DefaultResponseIndex)
						{
							btnResponse.IsDefault = true;
						}

						// Only set the margin on the 
						if (responseNum != responses.ResponseNames.Length)
						{
							btnResponse.Margin = new Thickness(0, 0, 5, 0);
						}

						buttonPanel.Children.Add(btnResponse);
					}
				}

				if (options.TitleBarIcon != null)
				{
					this.Icon = options.TitleBarIcon;
				}

				if (options.DialogIcon != null)
				{
					iconImage.Source = options.DialogIcon;
				}
				else
				{
					StockIcons icons = new StockIcons();
					StockIcon icon = icons.Info;

					if (options.DialogType == DialogType.Question)
					{
						icon = icons.Help;
					}
					else if (options.DialogType == DialogType.Error)
					{
						icon = icons.Error;
					}
					else if (options.DialogType == DialogType.Warning)
					{
						icon = icons.Warning;
					}

					// This is an example of how to load a bitmap using a pack formatted string.
					//resourceImage = @"Helpers\UIHelpers\Images\question.png";
					//BitmapImage bitmapImage = new BitmapImage(new Uri(String.Format("pack://application:,,,/{0}", resourceImage)));

					iconImage.Source = icon.BitmapSource;
				}
			}

			this.DataContext = this;
		}

		public string UserResponse
		{
			get { return _userResponse; }
		}

		public string DialogTitle
		{
			get { return (string)GetValue(DialogTitleProperty); }
			set { SetValue(DialogTitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DialogTitle.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DialogTitleProperty =
			DependencyProperty.Register("DialogTitle", typeof(string), typeof(WpfDialog), new UIPropertyMetadata(""));


		public string Description
		{
			get { return (string)GetValue(DescriptionProperty); }
			set { SetValue(DescriptionProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DescriptionProperty =
			DependencyProperty.Register("Description", typeof(string), typeof(WpfDialog), new UIPropertyMetadata(""));

		void btnResponse_Click(object sender, RoutedEventArgs e)
		{
			_userResponse = ((Button)sender).Content.ToString();	// button text;
			Close();
		}
	}
}
