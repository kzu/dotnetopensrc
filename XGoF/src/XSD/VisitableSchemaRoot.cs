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
	/// Represents the root schema object, which contains all the elements.
	/// </summary>
	public class VisitableSchemaRoot : BaseCompositeSchemaElement
	{
		private XmlDocument _doc;
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="schema">The schema element.</param>
		/// <param name="schemaDocument">The schema element in XmlDocument format.</param>
		/// <param name="name">The name the element will have.</param>
		public VisitableSchemaRoot(XmlSchemaObject schema, 
			XmlDocument schemaDocument, string name) :
			base(schema, null, name, string.Empty, null)
		{
			_doc = schemaDocument;
		}

		/// <summary>
		/// New implementation of the base <c>SchemaObject</c> property.
		/// </summary>
		public new XmlSchema SchemaObject
		{
			get { return (XmlSchema)base.SchemaObject; }
		}

		/// <summary>
		/// Provides access to the schema in XmlDocument format.
		/// </summary>
		public XmlDocument SchemaDocument
		{
			get { return _doc; }
		}
	}

}
