//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// BaseSchemaVisitor.cs
// Contains three classes, which are the base classes used by the SchemaVisitor.cs
// file. They provide implementations of the IVisitableComponent interface for
// leafs and composites elements.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml.Schema;
using System.Collections;
using NMatrix.Core.Patterns;

namespace NMatrix.XGoF.XSD
{
	/// <summary>
	/// The abstract base class for all the visitable composites.
	/// Implements common functionality among composite components.
	/// </summary>
	/// <remarks>Adds IVisitableComponent implementation for composites.</remarks>
	public abstract class BaseCompositeSchemaElement : BaseSchemaTypedElement, IVisitableComponent
	{
		/// <summary>
		/// Keep the list of children elements.
		/// </summary>
		private ArrayList _children = new ArrayList();

		/// <summary>
		/// Protected constructor for use by the descendents.
		/// </summary>
		/// <param name="element">The schema object.</param>
		/// <param name="type">The schema type of the object.</param>
		/// <param name="name">The name the element will have.</param>
		/// <param name="typeName">The type name, which can be different from the 
		/// schema type name because of TypeNaming conventions.</param>
		/// <param name="parent">The parent of the element.</param>
		protected BaseCompositeSchemaElement(XmlSchemaObject element, 
			XmlSchemaType type, string name, string typeName, BaseSchemaTypedElement parent) : 
			base (element, type, name, typeName, parent)
		{
		}

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
		/// First visits itself, and then traverses all the children elements and passes the visitor to them.
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