//===============================================================================
// NMatrix XGoF Generator TypedDataSet PlugIn.
// http://www.sourceforge.net/projects/dotnetopensrc/XGoF
//
// DataRows.cs
// The visitor that adds the private variables for each DataColumn in the DataTable,
// and initializes them in InitVars and InitClass methods.
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
	/// Initializes DataColumn properties in the DataTable object.
	/// </summary>
	public class DataRows : BaseTypedDataSetVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataRows()
		{
		}

		/// <summary>
		/// Visitor implementation. Adds a property to the class declaration.
		/// </summary>
		/// <param name="element"></param>
		public void Visit(VisitableAttributeIntrinsicType element)
		{
			base.Visit(element);
			if (CurrentType == null) return;
			CodeDomHelper.AddProperty(CurrentType, element, 
				((XmlSchemaDatatype)element.SchemaObject.AttributeType).ValueType, 
				CurrentNamespace, RetrieveMethod(CurrentType, "InitClass"),
				RetrieveMethod(CurrentType, "InitVars"));
		}

		/// <summary>
		/// Visitor implementation. Adds a property to the class declaration.
		/// </summary>
		/// <param name="element"></param>
		public void Visit(VisitableAttributeSimpleType element)
		{			
			base.Visit(element);
			if (CurrentType == null) return;
			CodeDomHelper.AddProperty(CurrentType, element,
				((XmlSchemaSimpleType)element.SchemaObject.AttributeType).Datatype.ValueType, 
				CurrentNamespace, RetrieveMethod(CurrentType, "InitClass"),
				RetrieveMethod(CurrentType, "InitVars"));
		}


		/// <summary>
		/// Visitor implementation. Adds a property to the class declaration.
		/// </summary>
		/// <param name="element"></param>
		public void Visit(VisitableElementIntrinsicType element)
		{
			base.Visit(element);
			if (CurrentType == null) return;
			CodeDomHelper.AddProperty(CurrentType, element,
				((XmlSchemaDatatype)element.SchemaObject.ElementType).ValueType, 
				CurrentNamespace, RetrieveMethod(CurrentType, "InitClass"),
				RetrieveMethod(CurrentType, "InitVars"));
		}

		/// <summary>
		/// Visitor implementation. Adds a property to the class declaration.
		/// </summary>
		/// <param name="element"></param>
		public void Visit(VisitableElementSimpleType element)
		{
			base.Visit(element);
			if (CurrentType == null) return;
			CodeDomHelper.AddProperty(CurrentType, element,
				((XmlSchemaSimpleType)element.SchemaObject.ElementType).Datatype.ValueType, 
				CurrentNamespace, RetrieveMethod(CurrentType, "InitClass"),
				RetrieveMethod(CurrentType, "InitVars"));
		}

		/// <summary>
		/// Reposition the CurrentType variable to point to the DataTable object.
		/// </summary>
		/// <param name="element"></param>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);
			CurrentType = null;
            foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
				if (type.Name == element.Name + Configuration.CollectionNaming)
				{
                    CurrentType = type;
					break;
				}
		}

		/// <summary>
		/// Visitor implementation. Notifies progress.
		/// </summary>
		/// <param name="schema"></param>
		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Adding DataRow variables ...");
			base.Visit(schema);
		}
	}
}
