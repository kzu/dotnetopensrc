//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// RunnerSchema.cs
// Represents a single schema row in the configuration file, which is used as 
// a source for the generator.
// Can contain RunnerCustomization rows in turn.
// Validates the input schema file at initialization time.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml.Schema;
using System.Collections;
using NMatrix.Core.Xml;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Represents a single schema row in the configuration file. Schema is validated
	/// at initialization time.
	/// </summary>
	public class RunnerSource
	{
		private SortedList _customizations = new SortedList();
		private string _filename;

		private RunnerSource()
		{
		}

		/// <summary>
		/// Initialize the class with the received parameters.
		/// </summary>
		/// <param name="sourceFile"></param>
		public RunnerSource(string sourceFile)
		{
			_filename = sourceFile;
		}

		/// <summary>
		/// Returns the sorted list of customizations, according to their runOrder attribute.
		/// </summary>
		public SortedList Customizations
		{
			get { return _customizations; }
		}

		/// <summary>
		/// Returns the file name.
		/// </summary>
		public string FileName
		{
			get { return _filename; }
		}

		/// <summary>
		/// <c>ICloneable</c> implementation. Returns a shallow copy of the object.
		/// </summary>
		public RunnerSource Clone()
		{
            RunnerSource sch = new RunnerSource();
            sch._customizations = Customizations.Clone() as SortedList;
			sch._filename = _filename;
			return sch;
		}
	}
}
