//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// VisitableLeaf.cs
// Abstract implementation of IVisitableComponent interface for leaf elements.
// elements.
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
	public abstract class VisitableLeaf : IVisitableComponent
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public VisitableLeaf() { }

		/// <summary>
		/// Unimplemented method. Can't add a children to a leaf component.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Add(IVisitableComponent component)
		{
			throw new InvalidOperationException("Impossible to add children to a leaf.");
		}
		/// <summary>
		/// Unimplemented method. Can't remove a children from a leaf component.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Remove(IVisitableComponent component)
		{
			throw new InvalidOperationException("Impossible to remove children from a leaf.");
		}
		/// <summary>
		/// Unimplemented method. Leaf component doesn't have children.
		/// </summary>
		public IVisitableComponent this[int index]
		{
			get { throw new InvalidOperationException("Impossible to index a leaf."); }
			set { throw new InvalidOperationException("Impossible to index a leaf."); }
		}
		/// <summary>
		/// Implements the visitable interface. 
		/// </summary>
		/// <param name="visitor">The visitor to call Visit on.</param>
		public virtual void Accept(IVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
