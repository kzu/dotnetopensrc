//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// BaseSchemaTypedElement.cs
// Contains the main class used by the other base classes for the SchemaVisitor.cs
// file. 
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
	/// Base class for schema elements with a schema type.
	/// </summary>
	public abstract class BaseSchemaTypedElement
	{
		private string _name;
		private string _typename;
		private XmlSchemaType _schematype;
		private XmlSchemaObject _schemaobject;
		private BaseSchemaTypedElement _parent;

		/// <summary>
		/// Protected constructor for use by the descendents.
		/// </summary>
		/// <param name="element">The schema object.</param>
		/// <param name="type">The schema type of the object.</param>
		/// <param name="name">The name the element will have.</param>
		/// <param name="typeName">The type name, which can be different from the 
		/// schema type name because of TypeNaming conventions.</param>
		/// <param name="parent">The parent of the element.</param>
		protected BaseSchemaTypedElement(XmlSchemaObject element, XmlSchemaType type, 
			string name, string typeName, BaseSchemaTypedElement parent)
		{
			_schemaobject = element;
			_name = name;
			_typename = typeName;
			_schematype = type;
			_parent = parent;
		}

		/// <summary>
		/// The parent element.
		/// </summary>
		public BaseSchemaTypedElement Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		/// <summary>
		/// The schema object represented by this class.
		/// </summary>
		public XmlSchemaObject SchemaObject
		{
			get { return _schemaobject; }
			set { _schemaobject = value; }
		}

		/// <summary>
		/// The schema type of the element.
		/// </summary>
		public XmlSchemaType SchemaType
		{
			get { return _schematype; }
			set { _schematype = value; }
		}

		/// <summary>
		/// Element name.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Element's type name.
		/// </summary>
		public string TypeName
		{
			get { return _typename; }
		}
	}
}