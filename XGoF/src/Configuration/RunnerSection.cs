//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// RunnerSection.cs
// Represents the runner section in the configuration file. Mantains the collection
// of customizations and schemas to be run.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Represents the runner section in the configuration file. 
	/// Mantains the collection of customizations and schemas to be run.
	/// </summary>
	public class RunnerSection : ICloneable
	{
		private SortedList _customizations = new SortedList();
		private Hashtable _sources = new Hashtable();

		/// <summary>
		/// Default constructor.
		/// </summary>
		public RunnerSection()
		{
		}

		/// <summary>
		/// Returns the sorted list of customizations, according to their runOrder attribute.
		/// </summary>
		public SortedList Customizations
		{
			get { return _customizations; }
			set { _customizations = value; }
		}

		/// <summary>
		/// Returns the list of sources configured in the section.
		/// </summary>
		public Hashtable Sources
		{
			get { return _sources; }
		}

		/// <summary>
		/// <c>ICloneable</c> implementation. Returns a shallow copy of the object.
		/// </summary>
		public object Clone()
		{
			RunnerSection sec = new RunnerSection();
			sec.Customizations = Customizations.Clone() as SortedList;
			foreach (RunnerSource sch in Sources)
				sec.Sources.Add(sch.FileName, sch.Clone());
			return sec;
		}
	}
}
