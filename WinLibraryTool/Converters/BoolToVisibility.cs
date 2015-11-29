using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinLibraryTool.Converters
{
	public class BoolToVisibility : ConverterMarkupExtension<BoolToVisibility>
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null && value is bool)
			{
				return (bool)value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
			}
			else
			{
				return value;
			}
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
