//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// IO.cs
// Provides general utilities related to files/directories.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;

namespace NMatrix.Core.Utility
{
	/// <summary>
	/// Provides global utility methods.
	/// </summary>
	public class IO
	{
		private IO()
		{
		}

		/// <summary>
		/// Returns the path where the assembly was loaded from.
		/// </summary>
		/// <remarks>Can be used either for EXEs and DLLs. Usefull to load
		/// files located in the same directory as a loaded DLL, and not
		/// necessary the EXE.</remarks>
		public static string LoadedPath
		{
			get
			{
				return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			}
		}

		/// <summary>
		/// Appends to the file name the local path of the executing assembly.
		/// </summary>
		/// <param name="fileName">The file to use to build the path.</param>
		/// <remarks>Uses the <c>LoadedPath</c> property of this class.</remarks>
		public static string AppendLocalPath(string fileName)
		{
			string path = LoadedPath;
			if (!fileName.StartsWith(Path.DirectorySeparatorChar.ToString()))
				path += Path.DirectorySeparatorChar;
			path += fileName;
			return path;
		}
	}
}
