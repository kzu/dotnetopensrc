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

namespace NMatrix.XGoF.SampleProject
{
	/// <summary>
	/// Adds diagnostics method calls to generated output.
	/// </summary>
	public class DiagnosticsBuilder : BaseCodeVisitor
	{
		public DiagnosticsBuilder()
		{
		}

		private void DebugProperty(BaseLeafSchemaElement element)
		{
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

			// Add the debugging statements

			prop.GetStatements.Insert(0, 
				new CodeExpressionStatement(
					new CodeMethodInvokeExpression(
						new CodeTypeReferenceExpression(
							typeof(System.Diagnostics.Debug)), 
					"WriteLine", new CodeExpression[] { new CodePrimitiveExpression(
														"Getting property " + element.Name) 
													  })));
			prop.SetStatements.Insert(0, 
				new CodeExpressionStatement(
					new CodeMethodInvokeExpression(
						new CodeTypeReferenceExpression(
							typeof(System.Diagnostics.Debug)), 
							"WriteLine", new CodeExpression[] { new CodePrimitiveExpression(
													  "Setting property " + element.Name) 
												  })));				
		}

		#region Simple types are converted to properties
		public void Visit(VisitableAttributeIntrinsicType element)
		{
			DebugProperty(element);
		}

		public void Visit(VisitableAttributeSimpleType element)
		{			
			DebugProperty(element);
		}

		public void Visit(VisitableElementIntrinsicType element)
		{
			DebugProperty(element);
		}

		public void Visit(VisitableElementSimpleType element)
		{
			DebugProperty(element);
		}
		#endregion

		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			CodeConstructor ctor = null;
			// Locate existing constructor.
			foreach (CodeTypeMember member in CurrentType.Members)
			{
				if (member is CodeConstructor)
				{
					ctor = (CodeConstructor) member;
					break;
				}
			}
			if (ctor == null) return;

			ctor.Statements.Insert(0, 
				new CodeExpressionStatement(
					new CodeMethodInvokeExpression(
						new CodeTypeReferenceExpression(
							typeof(System.Diagnostics.Debug)), 
							"WriteLine", new CodeExpression[] { new CodePrimitiveExpression(
													  "Constructing object of type " + element.TypeName) 
												  })));
		}

		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Adding debugging statements ... ");
			base.Visit(schema);
		}
	}
}
