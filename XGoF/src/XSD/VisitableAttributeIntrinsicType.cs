//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// SchemaVisitor.cs
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
	/// An attribute element whose type is defined in the XSD Schema specification.
	/// </summary>
	public class VisitableAttributeIntrinsicType : BaseVisitableAttribute
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="attribute">The schema attribute.</param>
		/// <param name="name">The name the element will have.</param>
		/// <param name="parent">The parent of the element.</param>
		public VisitableAttributeIntrinsicType(XmlSchemaAttribute attribute,
			string name, BaseSchemaTypedElement parent) : 
			base(attribute, null, name, string.Empty, parent)
		{
			if (attribute.SchemaTypeName.Namespace != XmlSchema.Namespace)
				throw new ArgumentException("The attribute doesn't have an intrinsic XSD type.");
		}
	}

}
