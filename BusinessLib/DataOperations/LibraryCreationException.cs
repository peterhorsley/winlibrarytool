using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BusinessLib.DataOperations
{
	/// <summary>
	/// Exception that is thrown when creation of a Windows library fails.
	/// </summary>
	[Serializable]
	public class LibraryCreationException : Exception
	{
		public LibraryCreationException()
		{
		}

		public LibraryCreationException(string message)
			: base(message)
		{
		}

		public LibraryCreationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected LibraryCreationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
