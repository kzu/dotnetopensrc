//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// CollectionBuilder.cs
// The visitor that builds the Collection object when applies, and adds the 
// corresponding property to the containing type.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.Customization;
using NMatrix.XGoF.XSD;
using System.Collections;

namespace NMatrix.XGoF.Visitors
{
	/// <summary>
	/// Build the collection type for a complex element.
	/// </summary>
	internal class CollectionBuilder : BaseCodeVisitor
	{
		public CollectionBuilder()
		{
		}

		public override void Visit(VisitableElementComplexType element)
		{
			ArrayList nodes = Retriever.RetrieveCustomization(element, NodeType.Collection);
			string name = element.Name + Configuration.CollectionNaming;

			if (nodes.Count != 0)
			{
				CodeTypeDeclaration type = null;

				foreach (CodeTypeDeclaration obj in CurrentNamespace.Types)
					if (obj.Name == name)
					{
						type = obj;
						break;
					}

				if (type == null)
				{
					type = new CodeTypeDeclaration(name);
					CurrentNamespace.Types.Add(type);
					// Mark the type as being a collection.
					type.UserData.Add("IsCollection", true);

					// Append unhandled attributes to the type UserData property, for use by custom visitors.
					XmlAttribute[] attributes = element.SchemaObject.UnhandledAttributes;
					if ( attributes != null )
						foreach (XmlAttribute attr in attributes)
							type.UserData.Add(attr.LocalName, attr);
				}

				type.CustomAttributes.AddRange(CodeDomHelper.BuildCustomAttributes(nodes));
				type.BaseTypes.AddRange(CodeDomHelper.BuildBaseTypes(nodes));
			}

			// If the element is contained in another element, and generateContainerProperty configuration
			// is "true", add the corresponding property if it isn't already present.
			if (Configuration.InnerData.GenerateContainerProperty && element.Parent != null 
				&& element.Parent is VisitableElementComplexType)
			{
				CodeTypeDeclaration enclosing = null;
				foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
					if (type.Name == element.Parent.TypeName)
					{
						enclosing = type;
						break;
					}

				// If we find the parent type and the property hasn't been defined already
				if (enclosing != null)
				{
					bool existing = false;
					foreach (CodeTypeMember prop in enclosing.Members)
						if (prop.Name == element.Name)
						{
							existing = true;
							break;
						}

					if (!existing)
					{
						CodeMemberProperty prop = new CodeMemberProperty();
						prop.Name = element.Name;
						prop.Attributes = MemberAttributes.Public;
						// TODO: Review if it is necessary to add an attribute to specify getter and setter for the property.
						prop.HasGet = true;
						prop.HasSet = true;

						// Do we have a custom Collection?
						if (nodes.Count != 0)
							prop.Type = new CodeTypeReference(element.Name + Configuration.CollectionNaming);
						else
							prop.Type = new CodeTypeReference(element.TypeName, 1);

						enclosing.Members.Add(prop);
					}
				}		
			}			
		}		

		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Building Collections ... ");
			base.Visit(schema);
		}
	}
}
