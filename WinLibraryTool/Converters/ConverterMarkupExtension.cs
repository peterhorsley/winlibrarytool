using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace WinLibraryTool.Converters
{
	public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
		where T : class, new()
	{
		private static T m_converter = null;

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (m_converter == null)
			{
				m_converter = new T();
			}
			return m_converter;
		}

		#region IValueConverter Members

		public abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);
		public abstract object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

		#endregion
	}
}
