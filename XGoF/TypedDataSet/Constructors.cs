//===============================================================================
// NMatrix XGoF Generator TypedDataSet PlugIn.
// http://www.sourceforge.net/projects/dotnetopensrc/XGoF
//
// Constructors.cs
// The visitor that removes the default public constructors added by the base generator.
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
	/// Removes default constructors from the generated classes.
	/// </summary>
	public class Constructors : BaseTypedDataSetVisitor
	{
		/// <summary>
		/// Empty default constructor.
		/// </summary>
		public Constructors()
		{
		}

		/// <summary>
		/// Remove the public constructor added by the base generator to both DataTable and DataRow.
		/// </summary>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (CurrentType != null)
				RemoveConstructors();
	
			CurrentType = null;
			foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
				if (type.Name == element.Name + Configuration.CollectionNaming)
				{
					CurrentType = type;
					break;
				}

			if (CurrentType != null)
				RemoveConstructors();
		}

		private void RemoveConstructors()
		{
			CodeTypeMember[] members = new CodeTypeMember[CurrentType.Members.Count];
			CurrentType.Members.CopyTo(members, 0);
			foreach (CodeTypeMember member in members)
				if (member is CodeConstructor)
					CurrentType.Members.Remove(member);
		}

		/// <summary>
		/// Perform the visit operation on the schema.
		/// </summary>
		/// <param name="schema">The schema being visited.</param>
		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Adding Constructors ...");
			base.Visit(schema);
		}
	}
}