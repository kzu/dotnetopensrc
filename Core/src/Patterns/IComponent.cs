//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// IComponent.cs
// Defines the interface for the composite pattern implementation. 
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
	public interface IComponent
	{
		/// <summary>
		/// Adds a component to the composite.
		/// </summary>
		/// <param name="component">The component to add.</param>
		void Add(IComponent component);

		/// <summary>
		/// Removes a component from the composite.
		/// </summary>
		/// <param name="component">The component to remove.</param>
		void Remove(IComponent component);

		/// <summary>
		/// Indexer of the composite.
		/// </summary>
		IComponent this[int index]
		{
			get;
			set;
		}
	}
}
