//===============================================================================
// NMatrix XGoF Generator TypedDataSet PlugIn.
// http://www.sourceforge.net/projects/dotnetopensrc/XGoF
//
// Constraints.cs
// The visitor that adds primary keys and unique constraints to the datatables.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.XSD;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace NMatrix.XGoF.PlugIn.TypedDataSet
{
	/// <summary>
	/// Adds constraints to the DataTable objects defined.
	/// </summary>
	public class Constraints : BaseTypedDataSetVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Constraints()
		{
		}

		/// <summary>
		/// Visitor implementation. Creates the constraints.
		/// </summary>
		/// <param name="element"></param>
		public override void Visit(VisitableElementComplexType element)
		{
			// The base visitor will initialize the variables.
			base.Visit(element);

			if (!IsDataSet)
			{
				// Reset the variables we will use.
				CurrentType = null;
				string name = element.Name + Configuration.CollectionNaming;

				// Find the DataTable type.
				foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
					if (type.Name == name)
					{
						CurrentType = type;
						break;
					}
				
				if (CurrentType != null)
				{
					string xpath;
					bool haspk = false;
					bool ispk = false;

					CodeMemberMethod initclass = RetrieveMethod(CurrentType, "InitClass");
					CodeMemberMethod initvars = RetrieveMethod(CurrentType, "InitVars");

					// Make the InitVars method internal
					initvars.Attributes = MemberAttributes.Final | MemberAttributes.Assembly;

					#region Process uniques.
					xpath = ".//";
					xpath += RetrievePrefix();
					xpath += element.Name;
					xpath = "//xsd:unique[xsd:selector/@xpath=\"" + xpath + "\"]";

					XmlNodeList uniques = CurrentSchemaDom.SelectNodes(xpath, Namespaces);
					foreach (XmlNode unique in uniques)
						ProcessUniqueKey(unique, initclass, ref haspk, ref ispk);
					#endregion

					#region Process keys.
					xpath = ".//";
					xpath += RetrievePrefix();
					xpath += element.Name;
					xpath = "//xsd:key[xsd:selector/@xpath=\"" + xpath + "\"]";

					XmlNodeList keys = CurrentSchemaDom.SelectNodes(xpath, Namespaces);
					foreach (XmlNode key in keys)
						ProcessUniqueKey(key, initclass, ref haspk, ref ispk);
					#endregion

					// Add an internal PK if this element has a nested DataTable and
					// this table doesn't have a primary key already defined.
					// TODO: unimplemented.
					//		if (!haspk)
					//			CodeDomHelper.BuildInternalPrimaryKey(CurrentType, element, initclass, initvars);
				}
			}
		}

		/// <summary>
		/// Processes a unique key and adds the corresponding code to the initClass parameter.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="initClass"></param>
		/// <param name="hasPK"></param>
		/// <param name="isPK"></param>
		private void ProcessUniqueKey(XmlNode node, CodeMemberMethod initClass, 
			ref bool hasPK, ref bool isPK)
		{
			XmlAttribute attr;
			ArrayList fields = new ArrayList();
			
			attr = node.Attributes["PrimaryKey", "urn:schemas-microsoft-com:xml-msdata"];
			if (attr != null)
				if (attr.Value == "true") 
				{
					hasPK = true;
					isPK = true;
				}

			foreach (XmlNode child in node.ChildNodes)
				if (child.LocalName == "field")
					fields.Add(Regex.Replace(child.Attributes["xpath"].Value, "([A-z|0-9]+):", ""));

			// Add unique constraints.
			initClass.Statements.AddRange(CodeDomHelper.BuildUniqueConstraint(
				node.Attributes["name"].Value, isPK, fields));
		}

		/// <summary>
		/// Notifies progress.
		/// </summary>
		/// <param name="schema"></param>
		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Building constraints ...");
			base.Visit(schema);
		}
	}
}
