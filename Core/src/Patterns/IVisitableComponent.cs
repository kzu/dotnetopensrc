//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// IVisitableComponent.cs
// Defines the interface for a composite pattern implementation which includes
// the Visitor pattern as well. 
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// Implements the interface to implement visitable 
	/// components allowing composites elements.
	/// </summary>
	public interface IVisitableComponent : IVisitable
	{
		/// <summary>
		/// Adds a component to the composite.
		/// </summary>
		/// <param name="component">The component to add.</param>
		void Add(IVisitableComponent component);
		/// <summary>
		/// Removes a component from the composite.
		/// </summary>
		/// <param name="component">The component to remove.</param>
		void Remove(IVisitableComponent component);
		/// <summary>
		/// Indexer of the composite.
		/// </summary>
		IVisitableComponent this[int index]
		{
			get;
			set;
		}
	}
}
