//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ClassBuilder.cs
// The visitor that builds the Namespace and Type for the complex elements.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.Customization;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.Visitors
{
	internal class ClassBuilder : BaseCodeVisitor
	{
		public ClassBuilder()
		{
		}

		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (CurrentType == null)
			{
				ArrayList nodes = Retriever.RetrieveCustomization(element, NodeType.Type);

				if (nodes.Count != 0 || Retriever.Files.Count == 0)
				{
					CodeTypeDeclaration type = new CodeTypeDeclaration(element.TypeName);
					type.CustomAttributes.AddRange(CodeDomHelper.BuildCustomAttributes(nodes));
					type.BaseTypes.AddRange(CodeDomHelper.BuildBaseTypes(nodes));
					// Append unhandled attributes to the type UserData property, for use by custom visitors.
					XmlAttribute[] attributes = element.SchemaObject.UnhandledAttributes;
					if ( attributes != null )
						foreach (XmlAttribute attr in attributes)
							type.UserData.Add(attr.LocalName, attr);

					CurrentNamespace.Types.Add(type);
				}
			}
		}

		public override void Visit(VisitableSchemaRoot element)
		{
			OnProgress("Building classes ...");
			base.Visit(element);

			if (CurrentNamespace == null)
			{
				CurrentNamespace = new CodeNamespace(element.SchemaObject.TargetNamespace);
				CurrentNamespace.Imports.AddRange(CodeDomHelper.BuildNamespaceImports(Configuration.NamespaceImports));
				// Append unhandled attributes to the type UserData property, for use by custom visitors.
				XmlAttribute[] attributes = element.SchemaObject.UnhandledAttributes;
				if ( attributes != null )
					foreach (XmlAttribute attr in attributes)
						CurrentNamespace.UserData.Add(attr.LocalName, attr);

				Unit.Namespaces.Add(CurrentNamespace);
			}
		}
	}
}
