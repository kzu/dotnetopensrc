//===============================================================================
// NMatrix XGoF Generator TypedDataSet PlugIn.
// http://www.sourceforge.net/projects/dotnetopensrc/XGoF
//
// Relations.cs
// The visitor that adds and initializes datarelations in the dataset.
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
	/// Adds DataRelation objects to the DataSet.
	/// </summary>
	public class Initializations : BaseTypedDataSetVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Initializations()
		{
		}

		/// <summary>
		/// Visitor implementation. Adds code to InitClass and InitVars methods of the DataTable classes.
		/// </summary>
		/// <param name="element"></param>
		public override void Visit(VisitableElementComplexType element)
		{
			// The base visitor will initialize the variables.
			base.Visit(element);

			if (!IsDataSet)
			{
				string xpath;
				ArrayList fieldsref = new ArrayList();
				ArrayList fieldskey = new ArrayList();

				CodeMemberMethod initclass = RetrieveMethod(CurrentDataSetType, "InitClass");
				CodeMemberMethod initvars = RetrieveMethod(CurrentDataSetType, "InitVars");

				#region Initialize InitVars method.
				//this.tablepublishers = ((publishersDataTable)(this.Tables["publishers"]));
				initvars.Statements.Add(
					new CodeAssignStatement(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "table" + element.Name),
						new CodeCastExpression(element.Name + Configuration.CollectionNaming, 
							new CodeIndexerExpression(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(), "Tables"),
								new CodeExpression[] { new CodePrimitiveExpression(element.Name) }))));

				//if ((this.tablepublishers != null)) this.tablepublishers.InitVars();
				initvars.Statements.Add(
					new CodeConditionStatement(
						new CodeBinaryOperatorExpression(
							new CodePropertyReferenceExpression(
								new CodeThisReferenceExpression(), "table" + element.Name), 
							CodeBinaryOperatorType.IdentityInequality,
							new CodePrimitiveExpression(null)), 
						new CodeStatement[] { 
							new CodeExpressionStatement(
								new CodeMethodInvokeExpression(
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(), "table" + element.Name),
									"InitVars", 
									new CodeExpression[0])) } ));
				#endregion

				#region Initialize InitClass method.
				//this.tablepublishers = new publishersDataTable();
				initclass.Statements.Add(
					new CodeAssignStatement(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "table" + element.Name), 
						new CodeObjectCreateExpression(
							element.Name + Configuration.CollectionNaming, new CodeExpression[0])));
				//this.Tables.Add(this.tablepublishers);
				initclass.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "Tables"),
						"Add", new CodeExpression[] {
							new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "table" + element.Name) }));
				#endregion
				
				#region Retrieve keyref nodes.
				xpath = ".//";
				xpath += RetrievePrefix();
				xpath += element.Name;
				xpath = "//xsd:keyref[xsd:selector/@xpath=\"" + xpath + "\"]";

				XmlNodeList keyrefs = CurrentSchemaDom.SelectNodes(xpath, Namespaces);
				foreach (XmlNode keyref in keyrefs)
				{
					string parentname = String.Empty;
					string fkname = keyref.Attributes.GetNamedItem("name").Value;
					if (keyref.Attributes.GetNamedItem("ConstraintName") != null) 
						fkname = keyref.Attributes.GetNamedItem("ConstraintName").Value;

					// Retrieve fields which form the keyref
					foreach (XmlNode node in keyref.ChildNodes)
						if (node.LocalName == "field")
							fieldsref.Add(Regex.Replace(node.Attributes.GetNamedItem("xpath").Value, "([A-z|0-9]+):", ""));

					//Retrieve the key being referenced and its fields
					xpath = ".//xsd:key[@name=\"" + keyref.Attributes.GetNamedItem("refer").Value + "\"] | ";
					xpath += ".//xsd:unique[@name=\"" + keyref.Attributes.GetNamedItem("refer").Value + "\"]";
					XmlNode key = CurrentSchemaDom.SelectSingleNode(xpath, Namespaces);

					if (key == null) throw new ArgumentException("A referenced key couldn't be found.");

					foreach (XmlNode node in key.ChildNodes)
						if (node.LocalName == "field")
							fieldskey.Add(
								Regex.Replace(node.Attributes.GetNamedItem("xpath").Value, "([A-z|0-9]+):", "").
								Replace(".", "").Replace("/", ""));
						else if (node.LocalName == "selector")
                            parentname = Regex.Replace(node.Attributes.GetNamedItem("xpath").Value, "([A-z|0-9]+):", "").
								Replace(".", "").Replace("/", "");

					#region Create Foreign Key
					CodeExpression[] fields = new CodeExpression[fieldskey.Count];
					for ( int i = 0; i < fieldskey.Count; i ++ )
						fields[i] = new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodeThisReferenceExpression(), "table" + parentname),
							fieldskey[i].ToString() + "Column");
					CodeArrayCreateExpression parentfldcreate = 
						new CodeArrayCreateExpression(typeof(DataColumn), fields);

					fields = new CodeExpression[fieldsref.Count];
					for ( int i = 0; i < fieldsref.Count; i ++ )
						fields[i] = new CodePropertyReferenceExpression(
							new CodePropertyReferenceExpression(
								new CodeThisReferenceExpression(), "table" + element.Name),
							fieldsref[i].ToString() + "Column");
					CodeArrayCreateExpression childfldcreate = 
						new CodeArrayCreateExpression(typeof(DataColumn), fields);
					
					//fkc = new ForeignKeyConstraint("publisherstitles", new DataColumn[] {
					//			this.tablepublishers.pub_idColumn,
					//			this.tablepublishers.pub_nameColumn}, new DataColumn[] {
					//			this.tabletitles.titlepub_idColumn,
					//			this.tabletitles.titleColumn});
					initclass.Statements.Add(
						new CodeAssignStatement(
							new CodeVariableReferenceExpression("fkc"),
						new CodeObjectCreateExpression(typeof(ForeignKeyConstraint),
							new CodeExpression[] { 
								new CodePrimitiveExpression(fkname), parentfldcreate, childfldcreate } )));
					#endregion

					#region Add and initialize the Foreign Key
					//this.tabletitles.Constraints.Add(fkc);
					initclass.Statements.Add(
						new CodeMethodInvokeExpression(
							new CodePropertyReferenceExpression(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(), "table" + element.Name),
								"Constraints"), 
							"Add", new CodeExpression[] { new CodeVariableReferenceExpression("fkc") } ));

					//fkc.AcceptRejectRule = AcceptRejectRule.None;
					CodeExpression arrule = new CodeFieldReferenceExpression(
						new CodeTypeReferenceExpression(typeof(AcceptRejectRule)), "None");
					if (keyref.Attributes.GetNamedItem("AcceptRejectRule") != null) 
						arrule = new CodeFieldReferenceExpression(
							new CodeTypeReferenceExpression(typeof(AcceptRejectRule)), 
								keyref.Attributes.GetNamedItem("AcceptRejectRule").Value);
					initclass.Statements.Add(
						new CodeAssignStatement(
							new CodePropertyReferenceExpression(
								new CodeVariableReferenceExpression("fkc"), "AcceptRejectRule"), arrule));

					//fkc.DeleteRule = Rule.Cascade;
					CodeExpression rule = new CodeFieldReferenceExpression(
						new CodeTypeReferenceExpression(typeof(Rule)), "Cascade");
					if (keyref.Attributes.GetNamedItem("DeleteRule") != null) 
						arrule = new CodeFieldReferenceExpression(
							new CodeTypeReferenceExpression(typeof(Rule)), 
								keyref.Attributes.GetNamedItem("DeleteRule").Value);
					initclass.Statements.Add(
						new CodeAssignStatement(
							new CodePropertyReferenceExpression(
								new CodeVariableReferenceExpression("fkc"), "DeleteRule"), rule));

					//fkc.UpdateRule = Rule.Cascade;
					rule = new CodeFieldReferenceExpression(
						new CodeTypeReferenceExpression(typeof(Rule)), "Cascade");
					if (keyref.Attributes.GetNamedItem("UpdateRule") != null) 
						arrule = new CodeFieldReferenceExpression(
							new CodeTypeReferenceExpression(typeof(Rule)), 
								keyref.Attributes.GetNamedItem("UpdateRule").Value);
					initclass.Statements.Add(
						new CodeAssignStatement(
							new CodePropertyReferenceExpression(
								new CodeVariableReferenceExpression("fkc"), "UpdateRule"), rule));
					#endregion

					#region Add and initialize the DataRelation
					if (keyref.Attributes.GetNamedItem("ConstraintOnly") == null ||
						keyref.Attributes.GetNamedItem("ConstraintOnly").Value != "false")
					{
						string relation = keyref.Attributes.GetNamedItem("name").Value;
						//private DataRelation relationpublisherstitles; (at dataset-level)
						CodeMemberField fld = new CodeMemberField(
							typeof(DataRelation), "relation" + keyref.Attributes.GetNamedItem("name").Value);
						fld.Attributes = MemberAttributes.Private;
						CurrentDataSetType.Members.Add(fld);

						//In InitVars:
						//this.relationpublisherstitles = this.Relations["publisherstitles"];
						initvars.Statements.Add(
							new CodeAssignStatement(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(), "relation" + relation),
								new CodeIndexerExpression(
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(), "Relations"),
									new CodeExpression[] { new CodePrimitiveExpression(relation) } )));

						//this.relationpublisherstitles = new DataRelation("publisherstitles", new DataColumn[] {
						//            this.tablepublishers.pub_idColumn,
						//            this.tablepublishers.pub_nameColumn}, new DataColumn[] {
						//            this.tabletitles.titlepub_idColumn,
						//            this.tabletitles.titleColumn}, false);
						initclass.Statements.Add(
							new CodeAssignStatement(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(),  "relation" + relation),
								new CodeObjectCreateExpression(
									typeof(DataRelation), 
									new CodeExpression[] { 
										new CodePrimitiveExpression(relation), parentfldcreate, childfldcreate} )));

						//this.relationpublishertitles.Nested = true;
						if (element.Parent != null && element.Parent is VisitableElementComplexType)
							initclass.Statements.Add(
								new CodeAssignStatement(
									new CodePropertyReferenceExpression(
										new CodePropertyReferenceExpression(
											new CodeThisReferenceExpression(), "relation" + relation), 
										"Nested"),
									new CodePrimitiveExpression(true)));

						//this.Relations.Add(this.relationpublisherstitles);
						initclass.Statements.Add(
							new CodeMethodInvokeExpression(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(), "Relations"),
								"Add", new CodeExpression[] { 
									new CodePropertyReferenceExpression(
										new CodeThisReferenceExpression(), "relation" + relation) }));

						// Add the property to retrieve child rows to the parent type.
						parentname += Configuration.TypeNaming;
						string childname = element.Name + Configuration.TypeNaming;
						foreach (CodeTypeMember type in CurrentNamespace.Types)
							if (type.Name == parentname)
							{
								//public titlesRow[] GettitlesRows() {
								//    return ((titlesRow[])
								//		(this.GetChildRows(this.Table.ChildRelations["publisherstitles"]))); }
								CodeMemberMethod getrows = new CodeMemberMethod();
								getrows.Attributes = MemberAttributes.Public | MemberAttributes.Final;
								getrows.ReturnType = new CodeTypeReference(childname, 0);
								getrows.Name = "Get" + childname;
                                getrows.Statements.Add(
									new CodeMethodReturnStatement(
										new CodeCastExpression(new CodeTypeReference(childname, 0), 
											new CodeMethodInvokeExpression(
												new CodeThisReferenceExpression(), "GetChildRows",
												new CodeExpression[] {
													new CodeIndexerExpression(
														new CodePropertyReferenceExpression(
															new CodePropertyReferenceExpression(
																new CodeThisReferenceExpression(), "Table"),
															"ChildRelations"), 
														new CodeExpression[] { 
															new CodePrimitiveExpression(relation) }) }))));                                
								break;
							}
					}
					#endregion
				}
				#endregion

				/*
						internal void InitVars() {
							this.relationtitlestitleauthors = this.Relations["titlestitleauthors"];
						}
				*/

				#region Add the Get* method to the parent elements in the relations
				/*
						public titlesRow[] GettitlesRows() 
						{
							return ((titlesRow[])(this.GetChildRows(this.Table.ChildRelations["publisherstitles"])));
						}
				*/
				#endregion
			}
		}

		/// <summary>
		/// Initializes the InitClass and InitVars methods for the DataSet. We process
		/// in this method as it is called only once per schema.
		/// </summary>
		/// <param name="schema"></param>
		public override void Visit(VisitableSchemaRoot schema)
		{
			OnProgress("Adding initialization and DataRelation objects ...");
			base.Visit(schema);
			CodeMemberMethod initclass = RetrieveMethod(CurrentDataSetType, "InitClass");
			CodeMemberMethod initvars = RetrieveMethod(CurrentDataSetType, "InitVars");

			// Make the InitVars method internal
			initvars.Attributes = MemberAttributes.Final | MemberAttributes.Assembly;

			#region Initialize InitClass method.
			//this.DataSetName = "dsPublishers";
            initclass.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "DataSetName"), 
					new CodePrimitiveExpression(CurrentDataSetType.Name)));
			//this.Prefix = "";
			initclass.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "Prefix"),
					new CodePrimitiveExpression("")));
            //this.Namespace = "deverest-com-ar";
			initclass.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "Namespace"),
					new CodePrimitiveExpression(CurrentSchema.TargetNamespace)));
			//this.Locale = new System.Globalization.CultureInfo("en-US");
			string locale = "en-US";
			if (CurrentDataSetType.UserData.Contains("Locale"))
				locale = ((XmlAttribute)CurrentDataSetType.UserData["Locale"]).Value;
			initclass.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "Locale"), 
					new CodeObjectCreateExpression(
						typeof(System.Globalization.CultureInfo), 
						new CodeExpression[] { new CodePrimitiveExpression(locale) })));
			//this.CaseSensitive = false;
            bool sensitive = false;
			if (CurrentDataSetType.UserData.Contains("CaseSensitive"))
				sensitive = bool.Parse(((XmlAttribute)CurrentDataSetType.UserData["CaseSensitive"]).Value);
			initclass.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "CaseSensitive"), 
					new CodePrimitiveExpression(sensitive)));
            //this.EnforceConstraints = true;
			initclass.Statements.Add(
				new CodeAssignStatement(
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), "EnforceConstraints"), 
					new CodePrimitiveExpression(true)));

			// Add the variable to use for foreign keys.
			initclass.Statements.Add(
				new CodeVariableDeclarationStatement(typeof(ForeignKeyConstraint), "fkc"));
			#endregion
		}
	}
}
