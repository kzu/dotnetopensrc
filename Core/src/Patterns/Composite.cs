//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// Composite.cs
// Defines the interface for the composite pattern implementation. 
// Defines base classes with default implementations of the leaf and composite
// elements.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// The abstract base class for all the composites.
	/// Implements common functionality among composite components using 
	/// an <c>ArrayList</c> object to keep children references.
	/// </summary>
	public abstract class Composite : IComponent
	{
		/// <summary>
		/// Keep the list of children elements.
		/// </summary>
		private ArrayList _children = new ArrayList();

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Composite() { }

		/// <summary>
		/// Adds a child element to the composite.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Add(IComponent component)
		{
			_children.Add(component);
		}
		/// <summary>
		/// Removes a child element from the composite.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Remove(IComponent component)
		{
			_children.Remove(component);
		}
		/// <summary>
		/// Provides access to the indexed child element.
		/// </summary>
		public IComponent this[int index]
		{
			get { return (IComponent)_children[index]; }
			set { _children[index] = value; }
		}
	}
}
