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
//

namespace NMatrix.XGoF.XSD
{
	/// <summary>
	/// The abstract base class for all the visitable leafs.
	/// </summary>
	/// <remarks>Implements IVisitableComponent for leaf components</remarks>
	public abstract class BaseLeafSchemaElement : BaseSchemaTypedElement, IVisitableComponent
	{

		/// <summary>
		/// Protected constructor for use by the descendents.
		/// </summary>
		/// <param name="element">The schema object.</param>
		/// <param name="type">The schema type of the object.</param>
		/// <param name="name">The name the element will have.</param>
		/// <param name="typeName">The type name, which can be different from the 
		/// schema type name because of TypeNaming conventions.</param>
		/// <param name="parent">The parent of the element.</param>
		protected BaseLeafSchemaElement(XmlSchemaObject element, XmlSchemaType type, 
			string name, string typeName, BaseSchemaTypedElement parent) : 
			base (element, type, name, typeName, parent)
		{
		}

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