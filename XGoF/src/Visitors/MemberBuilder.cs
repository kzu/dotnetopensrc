//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// MemberBuilder.cs
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
	/// Adds raw code members to the generated types.
	/// </summary>
	internal class MemberBuilder : BaseCodeVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public MemberBuilder()
		{
		}

		/// <summary>
		/// Adds members from the CodeSnippetTypeMember parsed by the CodeDomHelper helper class.
		/// </summary>
		/// <param name="nodes">The configuration nodes.</param>
		/// <param name="elementName">The name of the element.</param>
		private void ParseNodes(ArrayList nodes, string elementName)
		{
			foreach (XmlNode node in nodes)
			{
				foreach (XmlNode inner in node.ChildNodes)
				{
					if (inner.LocalName == "Member")
						CurrentType.Members.Add(new CodeSnippetTypeMember(
							CodeDomHelper.ParseCodeContainer(elementName, 
							Configuration.TypeNaming,
							Configuration.CollectionNaming, 
							inner, CurrentNamespace)));
				}
			}
		}

		/// <summary>
		/// Visitor implementation. Processes the passed element 
		/// adding the member elements defined in the customization.
		/// </summary>
		/// <param name="element"></param>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			#region Add Members to Type declarations
			if (CurrentType != null)
			{
				ArrayList nodes = Retriever.RetrieveCustomization(
					element, NodeType.Type);
				ParseNodes(nodes, element.Name);
			}
			#endregion

			#region Add Members to Collection declarations
			string name = element.Name + Configuration.CollectionNaming;
			foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
				if (type.Name == name)
				{
					CurrentType = type;
					break;
				}

			if (CurrentType != null)
			{
				ArrayList nodes = Retriever.RetrieveCustomization(
					element, NodeType.Collection);
                ParseNodes(nodes, element.Name);
			}	
			#endregion
		}

		/// <summary>
		/// Notifies progress.
		/// </summary>
		/// <param name="schema"></param>
		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Building custom Member code blocks ... ");
			base.Visit(schema);
		}
	}
}
