//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// Defines the classes that make up the tree of the parsed XmlSchema.
// There are classes for Attributes, Elements and Composites.
// These classes define a hierarchy used by the Traverser.cs file to 
// load the custom representation of the XmlSchema. All classes inherit from
// the classes defined in the BaseSchemaVisitor.cs file.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Xml.Schema;


namespace NMatrix.XGoF.XSD
{
	/// <summary>
	/// Abstract base class for visitable elements.
	/// </summary>
	public abstract class BaseVisitableElement : BaseLeafSchemaElement
	{
		/// <summary>
		/// Default constructor, for use by descendents.
		/// </summary>
		/// <param name="element">The schema element.</param>
		/// <param name="type">The schema type of the object.</param>
		/// <param name="name">The name the element will have.</param>
		/// <param name="typeName">The type name, which can be different from the 
		/// schema type name because of TypeNaming conventions.</param>
		/// <param name="parent">The parent of the element.</param>
		protected BaseVisitableElement(XmlSchemaElement element, XmlSchemaType type, 
			string name, string typeName, BaseSchemaTypedElement parent) :
			base(element, type, name, typeName, parent)
		{
		}

		/// <summary>
		/// New implementation of the base <c>SchemaObject</c> property.
		/// </summary>
		public new XmlSchemaElement SchemaObject
		{
			get { return (XmlSchemaElement)base.SchemaObject; }
		}
	}
}
