//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ExtenderSection.cs
// Keeps configuration from an Extenders section.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Keeps configuration from an Extenders section.
	/// </summary>
	public class ExtenderSection : ICloneable
	{
		private ArrayList _retrievers = new ArrayList();
		private ITraverser _traverser;
		private SortedList _visitors = new SortedList();

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExtenderSection()
		{
		}

		/// <summary>
		/// Retrievers defined in the section.
		/// </summary>
		public ArrayList Retrievers
		{
			get { return _retrievers; }
		}

		/// <summary>
		/// Active <c>ITraverser</c> to use.
		/// </summary>
		public ITraverser Traverser
		{
			get { return _traverser; }
			set { _traverser = value; }
		}

		/// <summary>
		/// Configured Visitor Plug-ins.
		/// </summary>
		public SortedList Visitors
		{
			get { return _visitors; }
		}


		/// <summary>
		/// <c>ICloneable</c> implementation. Returns a shallow copy of the object.
		/// </summary>
		public object Clone()
		{
			ExtenderSection ext = new ExtenderSection();
			ext._retrievers = Retrievers.Clone() as ArrayList;
            ext._traverser = Traverser;
			ext._visitors = Visitors.Clone() as SortedList;
			return ext;
		}
	}
}
