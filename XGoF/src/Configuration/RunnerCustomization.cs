//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// RunnerCustomization.cs
// Represents a single customization row in the configuration file.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Represents a single customization row in the configuration file.
	/// </summary>
	public class RunnerCustomization : ICloneable
	{
		private XmlDocument _file = new XmlDocument();
		private string _filename;
		private int _order = 0;

		private RunnerCustomization()
		{
		}

		/// <summary>
		/// Initialize the class with the received parameters.
		/// </summary>
		/// <param name="fileName"></param>
		public RunnerCustomization(string fileName) : 
			this(fileName, 0)
		{
		}

		/// <summary>
		/// Initialize the class with the received parameters.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="runOrder"></param>
		public RunnerCustomization(string fileName, int runOrder)
		{
			_file.PreserveWhitespace = true;
			_file.Load(fileName);
			_filename = fileName;
			_order = runOrder;
		}

		/// <summary>
		/// Returns the loaded <c>XmlDocument</c> corresponding to the file.
		/// </summary>
		public XmlDocument File
		{
			get { return _file; }
		}

		/// <summary>
		/// Returns the file name.
		/// </summary>
		public string FileName
		{
			get { return _filename; }
		}

		/// <summary>
		/// The order in which this customization will be run.
		/// </summary>
		public int RunOrder
		{
			get { return _order; }
		}

		/// <summary>
		/// <c>ICloneable</c> implementation. Returns a shallow copy of the object.
		/// </summary>
		public object Clone()
		{
			RunnerCustomization cust = new RunnerCustomization();
			cust._file = File;
			cust._filename = FileName;
			cust._order = RunOrder;
			return cust;
		}
	}
}
