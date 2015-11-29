using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BusinessLib.DataModel;
using System.Xml;
using Microsoft.WindowsAPICodePack.Shell;

namespace BusinessLib.DataAccess
{
	public class WinLibrarySetStorage
	{
		private static List<WinLibrary> _libraries = new List<WinLibrary>();

		public static List<WinLibrary> Libraries
		{
			get { return _libraries; }
		}

		public static void SaveLibraries(string filePath)
		{
			// xml serialization
			XmlWriter xmlWriter = XmlWriter.Create(filePath);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WinLibrary>));
			xmlSerializer.Serialize(xmlWriter, _libraries);
			xmlWriter.Close();
		}

		public static List<WinLibrary> LoadLibraries(string filePath)
		{
			// xml de-serialization
			XmlReader xmlReader = XmlReader.Create(filePath);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<WinLibrary>));
			_libraries = (List<WinLibrary>)xmlSerializer.Deserialize(xmlReader);
			xmlReader.Close();

			UpgradeLibraries();

			return Libraries;
		}

		/// <summary>
		/// When the WinLibrary class changes, we need to make some modifications.
		/// </summary>
		private static void UpgradeLibraries()
		{
			foreach (WinLibrary lib in _libraries)
			{
				// Make sure that the icon reference is valid.
				if (String.IsNullOrEmpty(lib.IconReference.ReferencePath))
				{
					lib.IconReference = new IconReference(WinLibrary.DefaultIconReference);
				}
			}
		}
	}
}
