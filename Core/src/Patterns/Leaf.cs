//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// Leaf.cs
// Defines base class with default implementation of the leaf elements.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// The abstract base class for all the visitable leafs.
	/// </summary>
	public abstract class Leaf : IComponent
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Leaf() { }

		/// <summary>
		/// Unimplemented method. Can't add a children to a leaf component.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Add(IComponent component)
		{
			throw new InvalidOperationException("Impossible to add children to a leaf.");
		}
		/// <summary>
		/// Unimplemented method. Can't remove a children from a leaf component.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Remove(IComponent component)
		{
			throw new InvalidOperationException("Impossible to remove children from a leaf.");
		}
		/// <summary>
		/// Unimplemented method. Leaf component doesn't have children.
		/// </summary>
		public IComponent this[int index]
		{
			get { throw new InvalidOperationException("Impossible to index a leaf."); }
			set { throw new InvalidOperationException("Impossible to index a leaf."); }
		}
	}
}
