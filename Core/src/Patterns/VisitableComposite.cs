//===============================================================================
// NMatrix Core - Patterns.
// http://sourceforge.net/projects/dotnetopensrc/patterns
//
// VisitableComposite.cs
// Abstract implementation of IVisitableComponent interface for composite elements.
// elements.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;

namespace NMatrix.Core.Patterns
{
	/// <summary>
	/// The abstract base class for all the visitable composites.
	/// Implements common functionality among composite components using 
	/// an <c>ArrayList</c> object to keep children references.
	/// </summary>
	public abstract class VisitableComposite : IVisitableComponent
	{
		/// <summary>
		/// Keep the list of children elements.
		/// </summary>
		private ArrayList _children = new ArrayList();

		/// <summary>
		/// Default constructor.
		/// </summary>
		public VisitableComposite() { }

		/// <summary>
		/// Adds a child element to the composite.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Add(IVisitableComponent component)
		{
			_children.Add(component);
		}
		/// <summary>
		/// Removes a child element from the composite.
		/// </summary>
		/// <param name="component">The child component.</param>
		public void Remove(IVisitableComponent component)
		{
			_children.Remove(component);
		}
		/// <summary>
		/// Provides access to the indexed child element.
		/// </summary>
		public IVisitableComponent this[int index]
		{
			get { return (IVisitableComponent)_children[index]; }
			set { _children[index] = value; }
		}
		/// <summary>
		/// Visits this component and then all the children elements.
		/// </summary>
		/// <param name="visitor">The visitor to pass to children elements.</param>
		public virtual void Accept(IVisitor visitor)
		{
			visitor.Visit(this);
			foreach (IVisitableComponent component in _children)
				component.Accept(visitor);
		}
	}
}
