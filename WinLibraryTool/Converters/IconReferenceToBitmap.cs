using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Shell;
using WinLibraryTool.Helpers.CodeHelpers.IconHelper;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WinLibraryTool.Converters
{
	public class IconReferenceToBitmap : ConverterMarkupExtension<IconReferenceToBitmap>
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null && value is IconReference)
			{
				return IconHelper.IconReferenceToImageSource((IconReference)value);
			}

			return value;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
