//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// Traverser.cs
// Class responsible for traversing the XmlSchema and building the visitable
// tree of objects.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using NMatrix.Core.ComponentModel;
using NMatrix.Core.Configuration;
using NMatrix.Core.Patterns;
using NMatrix.Core.Xml;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.Host;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.XSD
{
	/// <summary>
	/// Class responsible for building the visitable tree from a file containing an XSD Schema.
	/// </summary>
	public class Traverser : HostedComponent, ITraverser
	{
		private GeneratorSection _config;
		private System.Xml.Schema.XmlSchema _context;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Traverser()
		{
		}
		
		/// <summary>
		/// Initializes the traversing with the XmlSchema.
		/// </summary>
		/// <param name="sourceFile">The schema file to traverse.</param>
		/// <returns>An <c>IVisitableComponent</c> containing all the visitable nodes.</returns>
		public IVisitableComponent Traverse(string sourceFile)
		{
			try
			{
				Validator val = new Validator();
				using (FileStream fs = new FileStream(sourceFile, FileMode.Open))
					_context = System.Xml.Schema.XmlSchema.Read(fs, new ValidationEventHandler(val.OnValidation));
				if (val.HasErrors)
					throw new ArgumentException("There were errors in the schema "
					+ sourceFile + Environment.NewLine + val.Errors);
			}
			catch (Exception ex)
			{
				throw new ArgumentException("The schema couldn't be loaded: "
				+ sourceFile + Environment.NewLine + ex.Message, ex);
			}

			// Retrieve the current configuration from the Host.
			IConfigurationRetriever retriever = 
				(IConfigurationRetriever)Host.GetService(typeof(IConfigurationRetriever));
			if (retriever == null) GeneratorHost.ThrowInvalidHostResponse(typeof(IConfigurationRetriever));

			_config = retriever.GetConfig("generator") as GeneratorSection;
			if (_config == null) GeneratorHost.ThrowInvalidHostResponse("No <generator> section retrieved.");

			if (!_context.IsCompiled) _context.Compile(null);
			XmlDocument doc = new XmlDocument();
			doc.Load(sourceFile);
			VisitableSchemaRoot result = new VisitableSchemaRoot(_context, doc, _context.Id);
			
			foreach (XmlSchemaObject item in _context.Items)
			{
				if (item is XmlSchemaElement)
				{
					result.Add(Build(item as XmlSchemaElement, result));
				}
			}

			return result;
		}

		/// <summary>
		/// Builds an IVisitableComponent with the attribute passed.
		/// </summary>
		/// <param name="attribute">The attribute to use to build the visitable component.</param>
		/// <param name="parent">The parent element of the visitable component to build.</param>
		/// <returns>The visitable component.</returns>
		/// <remarks>If attribute type is a named simple type, that will be the typeName used for the component if
		/// IterationType.ComplexType is selected. Otherwise, the same naming convention for unnamed simpletypes is followed.
		/// If the simple type is unnamed, the attribute name plus the TypeNaming is used as the typeName parameter 
		/// for component.
		/// </remarks>
		private IVisitableComponent Build(XmlSchemaAttribute attribute, BaseSchemaTypedElement parent)
		{
			if (!attribute.SchemaTypeName.IsEmpty)
			{
				if (attribute.SchemaTypeName.Namespace == XmlSchema.Namespace)
					return new VisitableAttributeIntrinsicType(attribute, attribute.Name, parent);
				else
					foreach (XmlSchemaObject item in _context.Items)
						if (item is XmlSchemaSimpleType &&
							((XmlSchemaSimpleType)item).Name == attribute.SchemaTypeName.Name)
						{
							XmlSchemaSimpleType st = item as XmlSchemaSimpleType;
							if (_config.Iteration == IterationType.ComplexType)
								return new VisitableAttributeSimpleType(attribute, st, attribute.Name, 
									st.Name, parent);
							else
								return new VisitableAttributeSimpleType(attribute, st, 
									attribute.Name, attribute.Name + _config.TypeNaming, parent);
						}
			}
			else return new VisitableAttributeSimpleType(attribute, 
                     attribute.SchemaType as XmlSchemaSimpleType, attribute.Name, 
					 attribute.Name + _config.TypeNaming, parent);

			return null;
		}

		/// <summary>
		/// Builds an IVisitableComponent with the element passed.
		/// </summary>
		/// <param name="element">The element to use to build the visitable component.</param>
		/// <param name="parent">The parent element of the visitable component to build.</param>
		/// <returns>The visitable component.</returns>
		/// <remarks>If element type is a named complex type, the component typeName will depend on the IterationType
		/// selected in the configuration. So it IterationType.ComplexElements is selected, the typeName will 
		/// be the element name plus the TypeNaming convention specified. Else, the ComplexType name is used.
		/// If it is a simple type, the simple type name is used. If the type is unnamed, the element name
		/// plus the TypeNaming is used.
		/// </remarks>
		private IVisitableComponent Build(XmlSchemaElement element, BaseSchemaTypedElement parent)
		{
			if (!element.SchemaTypeName.IsEmpty)
			{
				if (element.SchemaTypeName.Namespace == XmlSchema.Namespace)
					return new VisitableElementIntrinsicType(element, element.Name, parent);
				else
					foreach (XmlSchemaObject item in _context.Items)
						if (item is XmlSchemaType &&
							((XmlSchemaType)item).Name == element.SchemaTypeName.Name)
						{
							if (item is XmlSchemaSimpleType)
							{
								XmlSchemaSimpleType st = item as XmlSchemaSimpleType;
								return new VisitableElementSimpleType(element, st, element.Name, st.Name, parent);
							}
							else
							{
								XmlSchemaComplexType ct = item as XmlSchemaComplexType;
								if (_config.Iteration == IterationType.ComplexType)
									return RecurseElement(element, ct, element.Name, ct.Name, parent);
								else
									return RecurseElement(element, ct, element.Name, element.Name + 
										_config.TypeNaming, parent);
							}
						}
			}
			else if (element.SchemaType is XmlSchemaSimpleType)
				return new VisitableElementSimpleType(element, 
					element.SchemaType as XmlSchemaSimpleType, element.Name, element.Name + 
					_config.TypeNaming, parent);
			else if (element.SchemaType is XmlSchemaComplexType)
			{
				XmlSchemaComplexType ct = element.SchemaType as XmlSchemaComplexType;
					return RecurseElement(element, ct, element.Name, 
						element.Name + _config.TypeNaming, parent);
			}

			return null;
		}

		/// <summary>
		/// Traverses a complex element.
		/// </summary>
		/// <param name="element">The current element.</param>
		/// <param name="type">The current element type.</param>
		/// <param name="name">The element name.</param>
		/// <param name="typeName">The element type name, that is, name + typeNaming.</param>
		/// <param name="parent">The parent of the element received.</param>
		/// <returns>The IVisitableComponent to add to the composite parent.</returns>
		private IVisitableComponent RecurseElement(XmlSchemaElement element, XmlSchemaComplexType type,
			string name, string typeName, BaseSchemaTypedElement parent)
		{
			VisitableElementComplexType result = 
				new VisitableElementComplexType(element, type, name, typeName, parent);

			// Traverse all the attributes of the complex type.
			foreach (XmlSchemaObject obj in type.Attributes)
				if (obj is XmlSchemaAttribute) result.Add(Build(obj as XmlSchemaAttribute, result));

			// We only take into account the Choice and Sequence subgroups.
			if (type.Particle is XmlSchemaGroupBase && !(type.Particle is XmlSchemaAll))
			{
				XmlSchemaGroupBase group = type.Particle as XmlSchemaGroupBase;
				foreach (XmlSchemaObject item in group.Items)
					if (item is XmlSchemaElement) result.Add(Build(item as XmlSchemaElement, result));
			}
			return result;
		}
	}
}
