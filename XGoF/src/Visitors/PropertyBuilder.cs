//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// PropertyBuilder.cs
// The visitor that adds the properties for complex types. Each attribute or
// simple element represents a property.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.XSD;
using System.Collections;

namespace NMatrix.XGoF.Visitors
{
	/// <summary>
	/// Adds properties to the generated output.
	/// </summary>
	internal class PropertyBuilder : BaseCodeVisitor
	{
		public PropertyBuilder()
		{
		}

		/// <summary>
		/// If no customization files are defined, add an empty read/write property.
		/// </summary>
		/// <remarks>
		/// Beware that with this configuration code may not compile because of the 
		/// warnings about the properties not returning any value.
		/// </remarks>
		private void AddProperty(BaseLeafSchemaElement element, Type type)
		{
			ArrayList nodes = Retriever.RetrieveCustomization(element);			
			
			if (nodes.Count != 0)
			{
				// Original type name is the current type name without the 
				// type naming convention appended during tree parsing.
				string name = (Configuration.TypeNaming != String.Empty) ?
					CurrentType.Name.Replace(Configuration.TypeNaming, "") :
					CurrentType.Name;

				CodeMemberProperty prop = CodeDomHelper.BuildProperty(
					name, element.Name, Configuration.TypeNaming, 
					Configuration.CollectionNaming,
					type, nodes, CurrentNamespace);

				XmlAttribute[] attributes = ((XmlSchemaAnnotated)element.SchemaObject).UnhandledAttributes;
				if ( attributes != null )
					foreach (XmlAttribute attr in attributes)
						prop.UserData.Add(attr.LocalName, attr);

				CurrentType.Members.Add(prop);
			}
			else if (Retriever.Files.Count == 0)
			{
				CodeMemberProperty prop = new CodeMemberProperty();
				prop.Attributes = MemberAttributes.Public;
				prop.Name = element.Name;
				prop.Type = new CodeTypeReference(type);
				prop.HasGet = true;
				prop.HasSet = true;
				CurrentType.Members.Add(prop);
			}

		}

		public void Visit(VisitableAttributeIntrinsicType element)
		{
			AddProperty(element, ((XmlSchemaDatatype)element.SchemaObject.AttributeType).ValueType);
		}

		public void Visit(VisitableAttributeSimpleType element)
		{			
			AddProperty(element, ((XmlSchemaSimpleType)element.SchemaObject.AttributeType).Datatype.ValueType);
		}

		public void Visit(VisitableElementIntrinsicType element)
		{
			AddProperty(element, ((XmlSchemaDatatype)element.SchemaObject.ElementType).ValueType);
		}

		public void Visit(VisitableElementSimpleType element)
		{
			AddProperty(element, ((XmlSchemaSimpleType)element.SchemaObject.ElementType).Datatype.ValueType);
		}

		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Building properties ... ");
			base.Visit(schema);
		}
	}
}
