//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// PropertyBackerBuilder.cs
// The visitor that adds the private field for each empty property getter/setter found.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.XSD;
using NMatrix.XGoF.Visitors;
using System.Collections;

namespace NMatrix.XGoF.Visitors
{
	/// <summary>
	/// Adds properties to the generated output.
	/// </summary>
	internal class PropertyBackerBuilder : BaseCodeVisitor
	{
		public PropertyBackerBuilder()
		{
		}

		/// <summary>
		/// Adds private fields and getter/setter statements pointing to it to empty properties.
		/// </summary>
		/// <remarks>
		/// Naming convention is "_" + lowercase property name.
		/// </remarks>
		private void CheckProperty(BaseLeafSchemaElement element, Type type)
		{
			if (CurrentType == null) return;

			CodeMemberProperty prop = null;
			// Locate the property declaration
			foreach (CodeTypeMember member in CurrentType.Members)
			{
				if (member is CodeMemberProperty && member.Name == element.Name)
				{
					prop = (CodeMemberProperty) member;
					break;
				}
			}

			if (prop == null) return;

			// If there are no get/set statements, add field declarations and 
			// fill getter/setters
			if (prop.GetStatements.Count == 0 && prop.SetStatements.Count == 0)
			{
				CodeMemberField fld = new CodeMemberField(type, "_" + element.Name.ToLower());
				CurrentType.Members.Add(fld);

				prop.GetStatements.Add(new CodeMethodReturnStatement(
					new CodeFieldReferenceExpression(
					new CodeThisReferenceExpression(), fld.Name)));
				prop.SetStatements.Add(new CodeAssignStatement(
					new CodeFieldReferenceExpression(
					new CodeThisReferenceExpression(), fld.Name), 
					new CodePropertySetValueReferenceExpression()));
			}
		}

		public void Visit(VisitableAttributeIntrinsicType element)
		{
			CheckProperty(element, ((XmlSchemaDatatype)element.SchemaObject.AttributeType).ValueType);
		}

		public void Visit(VisitableAttributeSimpleType element)
		{			
			CheckProperty(element, ((XmlSchemaSimpleType)element.SchemaObject.AttributeType).Datatype.ValueType);
		}

		public void Visit(VisitableElementIntrinsicType element)
		{
			CheckProperty(element, ((XmlSchemaDatatype)element.SchemaObject.ElementType).ValueType);
		}

		public void Visit(VisitableElementSimpleType element)
		{
			CheckProperty(element, ((XmlSchemaSimpleType)element.SchemaObject.ElementType).Datatype.ValueType);
		}

		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Building backing private field for properties ... ");
			base.Visit(schema);
		}
	}
}
