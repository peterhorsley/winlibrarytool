using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;

namespace WinLibraryTool.Helpers.CodeHelpers.IconHelper
{
	class IconHelper
	{
		/// <summary>
		/// Extracts an icon from a givin icon file or an executable module (.dll or an .exe file).
		/// </summary>
		/// <param name="fileName">The path of the icon file or the executable module.</param>
		/// <param name="iconIndex">The index of the icon in the executable module.</param>
		/// <returns>A System.Drawing.Icon extracted from the file at the specified index in case of an executable module.</returns>
		public static Icon ExtractIcon(string fileName, int iconIndex)
		{
			//Try to load the file as icon file.
			if (System.IO.Path.GetExtension(fileName).Equals(".ico", StringComparison.InvariantCultureIgnoreCase))
			{
				return new Icon(fileName);
			}

			//Load the file as an executable module.
			using (IconExtractor extractor = new IconExtractor(fileName))
			{
				return extractor.GetIconAt(iconIndex);
			}
		}

		/// <summary>
		/// Extracts all the icons from a givin icon file or an executable module (.dll or an .exe file).
		/// </summary>
		/// <param name="fileName">The path of the icon file or the executable module.</param>
		/// <returns>
		/// A list of System.Drawing.Icon found in the file.
		/// If the file was an icon file, it will return a list containing a single icon.
		/// </returns>
		public static List<Icon> ExtractAllIcons(string fileName)
		{
			List<Icon> list = new List<Icon>();

			//Try to load the file as icon file.
			if (System.IO.Path.GetExtension(fileName).Equals(".ico", StringComparison.InvariantCultureIgnoreCase))
			{
				list.Add(new Icon(fileName));
				return list;
			}

			//Load the file as an executable module.
			using (IconExtractor extractor = new IconExtractor(fileName))
			{
				for (int i = 0; i < extractor.IconCount; i++)
				{
					list.Add(extractor.GetIconAt(i));
				}
			}
			return list;
		}

		/// <summary>
		/// Returns the number of icons in a file.
		/// </summary>
		/// <param name="fileName">The path to an .ico, .dll, or .exe file.</param>
		/// <returns></returns>
		public static int GetIconCount(string fileName)
		{
			//Try to load the file as icon file.
			if (System.IO.Path.GetExtension(fileName).Equals(".ico", StringComparison.InvariantCultureIgnoreCase))
			{
				return 1;
			}

			try
			{
				using (IconExtractor extractor = new IconExtractor(fileName))
				{
					return extractor.IconCount;
				}
			}
			catch { }

			return 0;
		}

		/// <summary>
		/// Extracts an icon (that best fits the current display device) from a givin icon file or an executable module (.dll or an .exe file).
		/// </summary>
		/// <param name="fileName">The path of the icon file or the executable module.</param>
		/// <param name="iconIndex">The index of the icon in the executable module.</param>
		/// <param name="desiredSize">Specifies the desired size of the icon.</param>
		/// <returns>A System.Drawing.Icon (that best fits the current display device) extracted from the file at the specified index in case of an executable module.</returns>
		public static Icon ExtractBestFitIcon(string fileName, int iconIndex, System.Drawing.Size desiredSize)
		{
			Icon icon = ExtractIcon(fileName, iconIndex);
			return GetBestFitIcon(icon, desiredSize);
		}

		/// <summary>
		/// Gets the System.Drawing.Icon that best fits the current display device.
		/// </summary>
		/// <param name="icon">System.Drawing.Icon to be searched.</param>
		/// <param name="desiredSize">Specifies the desired size of the icon.</param>
		/// <returns>System.Drawing.Icon that best fit the current display device.</returns>
		public static Icon GetBestFitIcon(Icon icon, System.Drawing.Size desiredSize)
		{
			IconInfo info = new IconInfo(icon);
			int index = info.GetBestFitIconIndex(desiredSize);
			return info.Images[index];
		}

		/// <summary>
		/// Converts an icon reference to a WPF bitmap source.
		/// </summary>
		/// <param name="iconRef"></param>
		/// <returns></returns>
		public static ImageSource IconReferenceToImageSource(IconReference iconRef)
		{
			Icon icon = IconHelper.ExtractBestFitIcon(iconRef.ModuleName, iconRef.ResourceId, new System.Drawing.Size(32, 32));
			//Icon icon = IconHelper.ExtractIcon(iconRef.ModuleName, iconRef.ResourceId);
			if (icon != null)
			{
				if (icon.Width <= 32)
				{
					Bitmap bitmap = icon.ToBitmap();
					IntPtr hBitmap = bitmap.GetHbitmap();

					ImageSource wpfBitmap =
					Imaging.CreateBitmapSourceFromHBitmap(
					hBitmap, IntPtr.Zero, Int32Rect.Empty,
					BitmapSizeOptions.FromEmptyOptions());

					return wpfBitmap;
				}
			}

			return null;
		}
	}
}
