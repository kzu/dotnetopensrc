//===============================================================================
// NMatrix XGoF Generator XDL PlugIn.
// http://www.deverest.com.ar/XGoF
//
// DataRowBuilder.cs
// Adds a DataRow class for each element in the DataTable.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Data;
using System.CodeDom;
using System.Xml.Schema;
using NMatrix.XGoF.PlugIn.XDL;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.PlugIn.XDL.DataRows
{
	/// <summary>
	/// Builds the <c>DataRow</c> classes.
	/// </summary>
	public class DataRowBuilder : BaseVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataRowBuilder()
		{
		}

		/// <summary>
		/// Adds code for the property.
		/// </summary>
		/// <param name="element">The element from which to build the property.</param>
		/// <param name="type">The type of the property to generate.</param>
		private void AddPropertyCode(BaseLeafSchemaElement element, Type type)
		{
			CodeMemberProperty prop = null;
			foreach (CodeTypeMember member in CurrentType.Members)
				if (member is CodeMemberProperty && member.Name == element.Name)
				{
                    prop = member as CodeMemberProperty;
					break;	
				}

			if (prop == null) throw new InvalidOperationException("Property " + element.Name + " wasn't found in the generated code.");

			//try 
			//{			
			//	return (string) GetValue("country"); 
			//}
			//catch (InvalidCastException e) 
			//{
			//	throw new StrongTypingException("Cannot get value because it is DBNull.", e);
			//}
			prop.GetStatements.Add(
				new CodeTryCatchFinallyStatement(
					new CodeStatement[] {
						new CodeMethodReturnStatement(
							new CodeCastExpression(type, 
								new CodeMethodInvokeExpression(null, "GetValue",
									new CodeExpression[] { new CodePrimitiveExpression(element.Name) })))
										},
					new CodeCatchClause[] {
						new CodeCatchClause("e", new CodeTypeReference(typeof(InvalidCastException)), 
							new CodeStatement[] { 
								new CodeThrowExceptionStatement(
									new CodeObjectCreateExpression(
										typeof(StrongTypingException), 
										new CodeExpression[] { 
											new CodePrimitiveExpression("Cannot get value because it is DBNull."),
											new CodeVariableReferenceExpression("e")
															}))
												})
										  }, 
					new CodeStatement[0] ));

			// SetValue("country", value);
			prop.SetStatements.Add(
				new CodeMethodInvokeExpression(null, "SetValue",
					new CodeExpression[] { 
						new CodePrimitiveExpression(element.Name), 
						new CodeVariableReferenceExpression("value") 
										 } ));
		}

		/// <summary>
		/// Visitor implementation. Builds a property for the element.
		/// </summary>
		/// <param name="element">The element from which to build the property.</param>
		public void Visit(VisitableAttributeIntrinsicType element)
		{
			AddPropertyCode(element, ((XmlSchemaDatatype)element.SchemaObject.AttributeType).ValueType);
		}

		/// <summary>
		/// Visitor implementation. Builds a property for the element.
		/// </summary>
		/// <param name="element">The element from which to build the property.</param>
		public void Visit(VisitableAttributeSimpleType element)
		{			
			AddPropertyCode(element, ((XmlSchemaSimpleType)element.SchemaObject.AttributeType).Datatype.ValueType);
		}

		/// <summary>
		/// Visitor implementation. Builds a property for the element.
		/// </summary>
		/// <param name="element">The element from which to build the property.</param>
		public void Visit(VisitableElementIntrinsicType element)
		{
			AddPropertyCode(element, ((XmlSchemaDatatype)element.SchemaObject.ElementType).ValueType);
		}

		/// <summary>
		/// Visitor implementation. Builds a property for the element.
		/// </summary>
		/// <param name="element">The element from which to build the property.</param>
		public void Visit(VisitableElementSimpleType element)
		{
			AddPropertyCode(element, ((XmlSchemaSimpleType)element.SchemaObject.ElementType).Datatype.ValueType);
		}

		/// <summary>
		/// Adds the inheritance, constructors and appropiate overrides for the generated class.
		/// </summary>
		/// <param name="element">The element from which to build the class.</param>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (!IsDataSet)
			{
				CurrentType.BaseTypes.Add(typeof(NMatrix.XDL.BaseRow));

				#region Constructors
				// Remove default constructor.
				CodeTypeMember[] members = new CodeTypeMember[CurrentType.Members.Count];
				CurrentType.Members.CopyTo(members, 0);
				foreach (CodeTypeMember member in members)
					if (member is CodeConstructor)
						CurrentType.Members.Remove(member);

				//internal publishersRow(DataRow row) : base(row) 
				CodeConstructor ctor = new CodeConstructor();
				ctor.Attributes = MemberAttributes.Assembly;
				ctor.Parameters.Add(
					new CodeParameterDeclarationExpression(typeof(DataRow), "row"));
				ctor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("row"));
				CurrentType.Members.Add(ctor);

				//internal publishersRow() 
				ctor = new CodeConstructor();
				ctor.Attributes = MemberAttributes.Assembly;
				CurrentType.Members.Add(ctor);
				#endregion

				//protected override Type GetRowType()
				//{
				//	return typeof(publishersRow);
				//}
				CodeMemberMethod method = new CodeMemberMethod();
				method.Name = "GetRowType";
				method.Attributes = MemberAttributes.Family | MemberAttributes.Override;
				method.ReturnType = new CodeTypeReference(typeof(Type));
				method.Statements.Add(
					new CodeMethodReturnStatement(
						new CodeTypeOfExpression(element.TypeName)));
				CurrentType.Members.Add(method);

				//protected override Type GetTableType()
				//{
				//	return typeof(publishersDataTable);
				//}
				method = new CodeMemberMethod();
				method.Name = "GetTableType";
				method.Attributes = MemberAttributes.Family | MemberAttributes.Override;
				method.ReturnType = new CodeTypeReference(typeof(Type));
				method.Statements.Add(
					new CodeMethodReturnStatement(
						new CodeTypeOfExpression(element.Name + Configuration.CollectionNaming)));
				CurrentType.Members.Add(method);
			}
		}
	}
}
