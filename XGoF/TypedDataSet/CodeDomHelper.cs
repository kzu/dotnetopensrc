using System;
using System.CodeDom;
using System.Collections;
using System.Data;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.PlugIn.TypedDataSet
{
	/// <summary>
	/// Helper class for CodeDOM generation
	/// </summary>
	internal class CodeDomHelper
	{
		private CodeDomHelper()
		{
		}

		/// <summary>
		/// Adds code statements to InitClass and InitVars methods.
		/// </summary>
		/// <param name="currentType">The type whose property we will add.</param>
		/// <param name="element">The current property element.</param>
		/// <param name="propertyType">The property type.</param>
		/// <param name="currentNamespace">Current namespace type declaration.</param>
		/// <param name="initClass">InitClass method declaration.</param>
		/// <param name="initVars">InitVars method declaration.</param>
		public static void AddProperty(CodeTypeDeclaration currentType, 
			BaseLeafSchemaElement element, Type propertyType, CodeNamespace currentNamespace,
			CodeMemberMethod initClass, CodeMemberMethod initVars)
		{
			if (currentType == null) return;
			//private DataColumn columntitle
            currentType.Members.Add(
					new CodeMemberField(typeof(DataColumn), "column" + element.Name));

			//internal DataColumn titleColumn 
			//{
			//	get { return this.columntitle; }
			//}
			CodeMemberProperty prop = new CodeMemberProperty();
			prop.Name = element.Name + "Column";
			prop.Type = new CodeTypeReference(typeof(DataColumn));
			prop.Attributes = MemberAttributes.Final | MemberAttributes.Assembly;
			prop.HasGet = true;
			prop.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "column" + element.Name)));
			currentType.Members.Add(prop);

			#region Null-related methods at the DataRow level
			CodeTypeDeclaration row = null;
			foreach (CodeTypeDeclaration type in currentNamespace.Types)
				if (type.Name == element.Parent.TypeName)
				{
					row = type;
					break;
				}

			if (row != null)
			{
				//public bool IstitleNull() {
				//	return this.IsNull(this.tabletitles.titleColumn);
				//}
				CodeMemberMethod nullmethod = new CodeMemberMethod();
				nullmethod.Name = "Is" + element.Name + "Null";
				nullmethod.ReturnType = new CodeTypeReference(typeof(bool));
				nullmethod.Statements.Add(
					new CodeMethodReturnStatement(
						new CodeMethodInvokeExpression(
							new CodeThisReferenceExpression(), "IsNull",
							new CodeExpression[] {
								new CodePropertyReferenceExpression(
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(),
											"table" + element.Parent.Name),
										element.Name + "Column") } )));
				row.Members.Add(nullmethod);

				//public void SettitleNull() {
				//	this[this.tabletitles.titleColumn] = System.Convert.DBNull;
				//}
				nullmethod = new CodeMemberMethod();
				nullmethod.Name = "Set" + element.Name + "Null";
				nullmethod.Statements.Add(
					new CodeAssignStatement(
						new CodeIndexerExpression(
							new CodeThisReferenceExpression(), 
							new CodeExpression[] {
								new CodePropertyReferenceExpression(
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(),
											"table" + element.Parent.Name),
										element.Name + "Column") }),
						new CodeFieldReferenceExpression(
							new CodeTypeReferenceExpression(typeof(Convert)), "DBNull")));
				row.Members.Add(nullmethod);
			}
			#endregion

			//	this.columntitle = new DataColumn("title", 
			//		typeof(string), null, System.Data.MappingType.Element);
			CodeExpression[] prm = new CodeExpression[4];
			prm[0] = new CodePrimitiveExpression(element.Name);
			prm[1] = new CodeTypeOfExpression(propertyType);
			prm[2] = new CodePrimitiveExpression(null);
			prm[3] = new CodeFieldReferenceExpression(
				new CodeTypeReferenceExpression(typeof(System.Data.MappingType)), "Element");

			initClass.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "column" + element.Name),
					new CodeObjectCreateExpression(typeof(DataColumn), prm)));

			//	this.Columns.Add(this.columntitle);
			prm = new CodeExpression[1];
			prm[0] = new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "column" + element.Name);

			initClass.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "Columns"), 
					"Add", prm));

			//	this.columntitle_id = this.Columns["title_id"];
			initVars.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "column" + element.Name),
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "Columns"), 
						new CodeExpression[] { new CodePrimitiveExpression(element.Name) })));
		}

		/// <summary>
		/// Builds the statements to create and initialize a unique constraint.
		/// </summary>
		/// <param name="name">The element name.</param>
		/// <param name="isPrimaryKey">Is the current constraint the DataTable primerty key?</param>
		/// <param name="fields">List of fields composing the constraint.</param>
		/// <returns>A <c>CodeStatementCollection</c> object with the statements.</returns>
		public static CodeStatementCollection BuildUniqueConstraint(string name, bool isPrimaryKey, ArrayList fields)
		{
			CodeStatementCollection statements = new CodeStatementCollection();
			CodeExpression[] cols = new CodeExpression[fields.Count];

			for (int i = 0; i < fields.Count; i++)
				cols[i] = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "column" + fields[i]);

			CodeArrayCreateExpression dc;
			//if (isPrimaryKey) cols[cols.Length - 1] = new CodePrimitiveExpression(true);

			dc = new CodeArrayCreateExpression(typeof(DataColumn), cols);
			CodeExpression[] uc;

			if (isPrimaryKey)
			{
				uc = new CodeExpression[3];
				uc[2] = new CodePrimitiveExpression(true);
			}
			else
				uc = new CodeExpression[2];

			uc[0] = new CodePrimitiveExpression(name);
			uc[1] = dc;

			statements.Add(new CodeMethodInvokeExpression(
				new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Constraints"), 
				"Add", new CodeObjectCreateExpression(typeof(UniqueConstraint), uc)));

			CodePropertySetValueReferenceExpression prop = new CodePropertySetValueReferenceExpression();
			foreach (string field in fields)
				statements.Add(
					new CodeAssignStatement(
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodeThisReferenceExpression(), "column" + field),
							"AllowDBNull"),
						new CodePrimitiveExpression(false)));

			if (fields.Count == 1)
				statements.Add(
					new CodeAssignStatement(
						new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodeThisReferenceExpression(), "column" + fields[0]),
							"Unique"), 
						new CodePrimitiveExpression(true)));

			return statements;
		}

        /// <summary>
        /// Add the statements required to define a hidden primary key to a DataTable. This
        /// is needed when no primary key is defined for it, and is used for internal purposes by the 
        /// DataTable object. Adds the variable declaration, internal property accessor and
        /// statements in InitClass and InitVars methods.
        /// </summary>
        /// <param name="currentType">The current DataTable corresponding to the element being visited.</param>
        /// <param name="element">The element being visited.</param>
        /// <param name="initClassMethod">The InitClass method.</param>
        /// <param name="initVarsMethod">The InitVars method.</param>
		public static void BuildInternalPrimaryKey(CodeTypeDeclaration currentType, 
			VisitableElementComplexType element, CodeMemberMethod initClassMethod, 
			CodeMemberMethod initVarsMethod)
		{
			// Add this statement: private DataColumn columntitles_Id;
			CodeMemberField var = 
				new CodeMemberField(typeof(DataColumn),
				"column" + element.Name + "_ID");
			currentType.Members.Add(var);

			// Build this statement:
			//		internal DataColumn titles_IDColumn { 
			//			get { return this.columntitles_ID; } 
			//		}
			CodeMemberProperty prop = new CodeMemberProperty();
			prop.Name = element.Name + "_IDColumn";
			prop.Type = new CodeTypeReference(typeof(DataColumn));
			prop.Attributes = MemberAttributes.Assembly | MemberAttributes.Final;
			prop.HasSet = false;
			prop.HasGet = true;
			prop.GetStatements.Add(
				new CodeMethodReturnStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "column" + element.Name + "_ID")));
			currentType.Members.Add(prop);

			// Add the following statement to InitVars:
			//		this.columntitles_ID = this.Columns["titles_ID"];
			CodeIndexerExpression column = new CodeIndexerExpression();
			column.TargetObject = new CodeThisReferenceExpression();
			column.Indices.Add(new CodePrimitiveExpression(element.Name + "_ID"));

			initVarsMethod.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "column" + element.Name + "_ID"),
					new CodeIndexerExpression(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "Columns"), 
						new CodeExpression[] { new CodePrimitiveExpression(element.Name + "_ID") })));


			// Add the following statements to InitClass:
			//		this.columntitles_Id = new DataColumn("titles_ID", 
			//			typeof(int), null, System.Data.MappingType.Hidden);
			//		this.Columns.Add(this.columntitles_ID);
			//		this.columntitles_ID.AutoIncrement = true;
			//		this.columntitles_ID.AllowDBNull = false;
			//		this.columntitles_ID.Unique = true;
			CodeExpression[] prm = new CodeExpression[4];
			prm[0] = new CodePrimitiveExpression(element.Name + "_ID");
			prm[1] = new CodeTypeOfExpression(typeof(int));
			prm[2] = new CodePrimitiveExpression(null);
			//prm[3] = new CodePrimitiveExpression(System.Data.MappingType.Hidden);
			prm[3] = new CodeFieldReferenceExpression(
				new CodeTypeReferenceExpression(typeof(System.Data.MappingType)), "Element");
	
			// Save a reference to: this.columntitles_ID
			CodePropertyReferenceExpression idcol = 
				new CodePropertyReferenceExpression(
				new CodeThisReferenceExpression(), "column" + element.Name + "_ID");

			initClassMethod.Statements.Add(
				new CodeAssignStatement(idcol,
					new CodeObjectCreateExpression(typeof(DataColumn), prm)));
			initClassMethod.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "Columns"), 
					"Add", new CodeExpression[] { idcol }));
			initClassMethod.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(idcol, "AutoIncrement"),
					new CodePrimitiveExpression(true)));
			initClassMethod.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(idcol, "AllowDBNull"),
					new CodePrimitiveExpression(false)));
			initClassMethod.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(idcol, "Unique"),
					new CodePrimitiveExpression(true)));
		}
	}
}
