using System;
using System.Data;
using System.CodeDom;
using NMatrix.XGoF.PlugIn.XDL;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.PlugIn.XDL.DataSets
{
	/// <summary>
	/// Builds the <c>DataSet</c> class.
	/// </summary>
	public class DataSetBuilder : BaseVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataSetBuilder()
		{
		}

		/// <summary>
		/// Visitor implementation.
		/// </summary>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (!IsDataSet)
			{
				#region Public DataTable accessors
				//[System.ComponentModel.Browsable(false)]
				//[System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
				//public publishersDataTable publishers
				//{
				//	get { return (publishersDataTable)RetrieveDataTable(typeof(publishersDataTable), "publishers"); }
				//}
				CodeMemberProperty prop = new CodeMemberProperty();
				prop.Name = element.Name;
				prop.Type = new CodeTypeReference(element.Name + Configuration.CollectionNaming);
				prop.HasGet = true;
				prop.GetStatements.Add(
					new CodeMethodReturnStatement(
						new CodeCastExpression(
							element.Name + Configuration.CollectionNaming,
							new CodeMethodInvokeExpression(
								new CodeThisReferenceExpression(), 
								"RetrieveDataTable", 
								new CodeExpression[] {
									new CodeTypeOfExpression(element.Name + Configuration.CollectionNaming),
									new CodePrimitiveExpression(element.Name) } ))));
				prop.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						"System.ComponentModel.Browsable", 
						new CodeAttributeArgument[] { 
							new CodeAttributeArgument(new CodePrimitiveExpression(false)) } ));
				prop.CustomAttributes.Add(
					new CodeAttributeDeclaration(
						"System.ComponentModel.DesignerSerializationVisibilityAttribute", 
						new CodeAttributeArgument[] { 
							new CodeAttributeArgument(
								new CodeFieldReferenceExpression(
									new CodeTypeReferenceExpression(
										typeof(System.ComponentModel.DesignerSerializationVisibility)), 
								"Content")) } ));
				CurrentDataSetType.Members.Add(prop);
				#endregion
			}
		}

		/// <summary>
		/// Visitor implementation.
		/// </summary>
		public override void Visit(VisitableSchemaRoot element)
		{
			base.Visit(element);

			#region DataSet attributes
			//[Serializable()]
			CurrentDataSetType.CustomAttributes.Add(
                new CodeAttributeDeclaration("Serializable"));
			//[System.ComponentModel.DesignerCategoryAttribute("code")]
			CurrentDataSetType.CustomAttributes.Add(
                new CodeAttributeDeclaration("System.ComponentModel.DesignerCategoryAttribute",
					new CodeAttributeArgument[] { new CodeAttributeArgument(
						new CodePrimitiveExpression("code")) } ));
			//[System.Diagnostics.DebuggerStepThrough()]
			CurrentDataSetType.CustomAttributes.Add(
                new CodeAttributeDeclaration("System.Diagnostics.DebuggerStepThrough"));
			//[System.ComponentModel.ToolboxItem(true)]
			CurrentDataSetType.CustomAttributes.Add(
                new CodeAttributeDeclaration("System.ComponentModel.ToolboxItem",
					new CodeAttributeArgument[] { new CodeAttributeArgument(
						new CodePrimitiveExpression(true)) } ));
			#endregion

			//Add Base type
			CurrentDataSetType.BaseTypes.Add(
				typeof(NMatrix.XDL.BaseDataSet));

			//Remove the naming convention
			CurrentDataSetType.Name = CurrentDataSetType.Name.Replace(
				Configuration.TypeNaming, "").Replace(
				Configuration.CollectionNaming, "");

			#region Constructors
			//Remove default constructor
			CodeTypeMember[] members = new CodeTypeMember[CurrentDataSetType.Members.Count];
			CurrentDataSetType.Members.CopyTo(members, 0);
			foreach (CodeTypeMember member in members)
				if (member is CodeConstructor)
					CurrentDataSetType.Members.Remove(member);
			////public dsPublishers(string schemaFile) : base(schemaFile)
            CodeConstructor ctor = new CodeConstructor();
			ctor.Attributes = MemberAttributes.Public;
			ctor.Parameters.Add(
				new CodeParameterDeclarationExpression(
				typeof(string), "schemaFile"));
			ctor.BaseConstructorArgs.Add(
				new CodeVariableReferenceExpression("schemaFile"));
            CurrentDataSetType.Members.Add(ctor);
			//public dsPublishers(string schemaFile, InternalDataSet state) : base(schemaFile, state) { }
            ctor = new CodeConstructor();
			ctor.Attributes = MemberAttributes.Public;
			ctor.Parameters.Add(
				new CodeParameterDeclarationExpression(
				typeof(string), "schemaFile"));
			ctor.Parameters.Add(
				new CodeParameterDeclarationExpression(
				typeof(NMatrix.XDL.Wrappers.InternalDataSet), "state"));
			ctor.BaseConstructorArgs.Add(
				new CodeVariableReferenceExpression("schemaFile"));
			ctor.BaseConstructorArgs.Add(
				new CodeVariableReferenceExpression("state"));
            CurrentDataSetType.Members.Add(ctor);
			#endregion
		}
	}
}
