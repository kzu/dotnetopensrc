//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ConstructorBuilder.cs
// The visitor that adds the constructors and member sections for complex types.
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
	/// <summary>
	/// Adds constructors to the generated types.
	/// </summary>
	internal class ConstructorBuilder : BaseCodeVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ConstructorBuilder()
		{
		}

		/// <summary>
		/// Visitor implementation. Processes the passed element 
		/// adding the corresponding constructors.
		/// </summary>
		/// <param name="element"></param>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (CurrentType != null)
				AddConstructor(element, Retriever.RetrieveCustomization(
					element, NodeType.Type));
			
			ArrayList nodes = Retriever.RetrieveCustomization(
				element, NodeType.Collection);
			string name = element.Name + Configuration.CollectionNaming;
			if (nodes.Count != 0)
			{
				CurrentType = null;
				foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
					if (type.Name == name)
					{
						CurrentType = type;
						break;
					}
				if (CurrentType != null)
					AddConstructor(element, nodes);
				// TODO: Constructor isn't added!!!
			}
		}

		/// <summary>
		/// Adds constructors to the CurrentType based on the configuration nodes passed.
		/// </summary>
		/// <param name="element">Current element.</param>
		/// <param name="nodes">Configuration nodes to use.</param>
		private void AddConstructor(VisitableElementComplexType element, ArrayList nodes)
		{
			CodeTypeMemberCollection constructors = new CodeTypeMemberCollection();
			foreach (XmlNode node in nodes)
			{
				foreach (XmlNode inner in node.ChildNodes)
				{
					if (inner.LocalName == "Constructor")
					{
						CodeConstructor ct = new CodeConstructor();
						// TODO: Review if it is necessary to add an attribute to specify visibility.
						ct.Attributes = MemberAttributes.Public;
						foreach (XmlNode child in inner.ChildNodes)
							if (child.LocalName == "Parameter")
								ct.Parameters.Add(new CodeParameterDeclarationExpression(
									child.Attributes["Type"].InnerText, child.Attributes["Name"].InnerText));
							else if (child.LocalName == "SourceCode")
								ct.Statements.Add(new CodeSnippetStatement(
									CodeDomHelper.ParseCodeContainer(element.Name,
									Configuration.TypeNaming,
									Configuration.CollectionNaming,
									child, CurrentNamespace)));
						constructors.Add(ct);
					}
				}
			}

			// If we don't find any constructors in the configuration files, we 
			// add a public constructor with no parameters.
			if (constructors.Count != 0)
			{
				CurrentType.Members.AddRange(constructors);
			}
			else
			{
				CodeConstructor ct = new CodeConstructor();
				ct.Attributes = MemberAttributes.Public;
				CurrentType.Members.Add(ct);
			}
		}

		/// <summary>
		/// Visitor implementation. Notifies progress.
		/// </summary>
		/// <param name="schema"></param>
		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Building Constructors ...");
			base.Visit(schema);
		}
	}
}
