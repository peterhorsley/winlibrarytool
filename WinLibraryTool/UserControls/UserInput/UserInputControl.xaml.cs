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

namespace WinLibraryTool.UserControls
{
	/// <summary>
	/// Interaction logic for UserInputControl.xaml
	/// </summary>
	public partial class UserInputControl : UserControl
	{
		public UserInputControl()
			:this(String.Empty, string.Empty)
		{
		}

		public UserInputControl(string label, string inputText)
		{
			InitializeComponent();

			LabelText = label;
			InputText = inputText;

			this.DataContext = this;
		}

		public string InputText
		{
			get { return (string)GetValue(InputTextProperty); }
			set { SetValue(InputTextProperty, value); }
		}

		// Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty InputTextProperty =
			DependencyProperty.Register("InputText", typeof(string), typeof(UserInputControl), new UIPropertyMetadata(""));

		public string LabelText
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register("LabelText", typeof(string), typeof(UserInputControl), new UIPropertyMetadata(""));

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			txtInput.Focus();
		}
	}
}
