using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WinLibraryTool.Converters
{
	public class DebuggingConverter : ConverterMarkupExtension<DebuggingConverter>, IValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value; // Add the breakpoint here!!
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException("This method should never be called");
		}
	}
}
