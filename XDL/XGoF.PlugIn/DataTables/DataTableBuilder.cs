//===============================================================================
// NMatrix XGoF Generator XDL PlugIn.
// http://www.deverest.com.ar/XGoF
//
// DataTableBuilder.cs
// Creates the DataTable elements.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Data;
using System.CodeDom;
using NMatrix.XGoF.PlugIn.XDL;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.PlugIn.XDL.DataTables
{
	/// <summary>
	/// Creates the DataTable elements.
	/// </summary>
	public class DataTableBuilder : BaseVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataTableBuilder()
		{
		}

		/// <summary>
		/// Visitor implementation. 
		/// </summary>
		/// <param name="element">The element from which to build the code.</param>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			// Reposition CurrentType in the DataTable type declaration.
			CurrentType = null;
			foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
				if (type.Name == element.Name + Configuration.CollectionNaming)
				{
					CurrentType = type;
					break;
				}

			if (!IsDataSet)
			{
				CodeMemberProperty prop = null;
				CodeMemberMethod method = null;

				#region Build DataTable type if it doesn't exist already
				if (CurrentType == null)
				{
					CurrentType = new CodeTypeDeclaration(element.Name + Configuration.CollectionNaming);
					// Mark the type as being a collection.
					CurrentType.UserData.Add("IsCollection", true);

					// Append unhandled attributes to the type UserData property, for use by custom visitors.
					XmlAttribute[] attributes = element.SchemaObject.UnhandledAttributes;
					if ( attributes != null )
						foreach (XmlAttribute attr in attributes)
							CurrentType.UserData.Add(attr.LocalName, attr);
					CurrentNamespace.Types.Add(CurrentType);
				}
				#endregion

				CurrentType.BaseTypes.Add(
					typeof(NMatrix.XDL.BaseDataTable));

				#region Constructors
				//Remove default constructor
				CodeTypeMember[] members = new CodeTypeMember[CurrentDataSetType.Members.Count];
				CurrentType.Members.CopyTo(members, 0);
				foreach (CodeTypeMember member in members)
					if (member is CodeConstructor)
						CurrentType.Members.Remove(member);
				//public publishersDataTable(DataTable table) : base(table)
				CodeConstructor ctor = new CodeConstructor();
				ctor.Attributes = MemberAttributes.Public;
				ctor.Parameters.Add(
					new CodeParameterDeclarationExpression(
					typeof(DataTable), "table"));
				ctor.BaseConstructorArgs.Add(
					new CodeVariableReferenceExpression("table"));
				CurrentType.Members.Add(ctor);
				#endregion

				#region Indexer
				//public publishersRow this[int index] 
				//{
				//	get { return ((publishersRow)(this.Rows[index])); }
				//}
				prop = new CodeMemberProperty();
				prop.Name = "Item";
				prop.Type = new CodeTypeReference(element.TypeName);
                prop.Parameters.Add(
					new CodeParameterDeclarationExpression(
						typeof(int), "index"));
				
                prop.HasGet = true;
				prop.GetStatements.Add(
					new CodeMethodReturnStatement(
						new CodeCastExpression(
							element.TypeName, 
							new CodeIndexerExpression(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(), "Rows"),
							new CodeExpression[] { 
								new CodeVariableReferenceExpression("index") } ))));
				CurrentType.Members.Add(prop);
				#endregion

				#region Events
					#region Declarations
					//public event publishersRowChangeEventHandler publishersRowChanged;
					CodeMemberEvent ev = new CodeMemberEvent();
					ev.Attributes = MemberAttributes.Public;
					ev.Name = element.TypeName + "Changed";
					ev.Type = new CodeTypeReference(element.TypeName + "ChangeEventHandler");
					CurrentType.Members.Add(ev);
					//public event publishersRowChangeEventHandler publishersRowChanging;
					ev = new CodeMemberEvent();
					ev.Attributes = MemberAttributes.Public;
					ev.Name = element.TypeName + "Changing";
					ev.Type = new CodeTypeReference(element.TypeName + "ChangeEventHandler");
					CurrentType.Members.Add(ev);
					//public event publishersRowChangeEventHandler publishersRowDeleted;
					ev = new CodeMemberEvent();
					ev.Attributes = MemberAttributes.Public;
					ev.Name = element.TypeName + "Deleted";
					ev.Type = new CodeTypeReference(element.TypeName + "ChangeEventHandler");
					CurrentType.Members.Add(ev);
					//public event publishersRowChangeEventHandler publishersRowDeleting;
					ev = new CodeMemberEvent();
					ev.Attributes = MemberAttributes.Public;
					ev.Name = element.TypeName + "Deleting";
					ev.Type = new CodeTypeReference(element.TypeName + "ChangeEventHandler");
					CurrentType.Members.Add(ev);
					#endregion
					#region Raising methods
					CurrentType.Members.Add(OnEvent(element.TypeName, "Changed"));
					CurrentType.Members.Add(OnEvent(element.TypeName, "Changing"));
					CurrentType.Members.Add(OnEvent(element.TypeName, "Deleted"));
					CurrentType.Members.Add(OnEvent(element.TypeName, "Deleting"));
					#endregion
				#endregion

				#region Row-related methods
				//public publishersRow Add(string pub_id, string pub_name, string city, string state, string country)
				method = new CodeMemberMethod();
				method.Name = "Add";
				method.ReturnType = new CodeTypeReference(element.TypeName);
				CodeTypeDeclaration rowtype = null;
				foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
					if (type.Name == element.TypeName)
					{
						rowtype = type;
						break;
					}
				if (rowtype == null) throw new InvalidOperationException("Type " + element.TypeName + " not found in generated code");
				foreach (CodeTypeMember member in rowtype.Members)
					if (member is CodeMemberProperty)
						method.Parameters.Add(
							new CodeParameterDeclarationExpression(
								((CodeMemberProperty)member).Type, member.Name));
				//	publishersRow row = this.NewRow();
				method.Statements.Add(
					new CodeVariableDeclarationStatement(
						element.TypeName, "row", 
						new CodeMethodInvokeExpression(
							new CodeThisReferenceExpression(), "NewRow", new CodeExpression[0])));
				//	row.ItemArray = new object[] 
				//		{ pub_id, pub_name, city, state, country };
				CodeArrayCreateExpression create = new CodeArrayCreateExpression();
				create.CreateType = new CodeTypeReference(typeof(object));
				foreach (CodeTypeMember member in rowtype.Members)
					if (member is CodeMemberProperty)
						create.Initializers.Add(new CodeVariableReferenceExpression(member.Name));
				method.Statements.Add(
					new CodeAssignStatement(
						new CodePropertyReferenceExpression(
							new CodeVariableReferenceExpression(
								"row"), "ItemArray"), create));
				//	this.Rows.Add(row);
				method.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "Rows"), "Add", 
							new CodeExpression[] { new CodeVariableReferenceExpression("row") } ));
				//	return row;
				method.Statements.Add(
					new CodeMethodReturnStatement(new CodeVariableReferenceExpression("row")));
				method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
				CurrentType.Members.Add(method);

				//public new publishersRow NewRow() 
				//{
				//	return ((publishersRow)(base.NewRow()));
				//}
				method = new CodeMemberMethod();
				method.Name = "NewRow";
				method.ReturnType = new CodeTypeReference(element.TypeName);
				method.Statements.Add(
					new CodeMethodReturnStatement(
						new CodeCastExpression(
							element.TypeName, new CodeMethodInvokeExpression(
								new CodeThisReferenceExpression(), "NewRow", new CodeExpression[0]))));
				method.Attributes = MemberAttributes.Public | MemberAttributes.New;
				CurrentType.Members.Add(method);

				//protected override Type GetRowType() 
				//{
				//	return typeof(publishersRow);
				//}
				method = new CodeMemberMethod();
				method.Name = "GetRowType";
				method.ReturnType = new CodeTypeReference(typeof(Type));
				method.Statements.Add(
					new CodeMethodReturnStatement(
						new CodeTypeOfExpression(element.TypeName)));
				method.Attributes = MemberAttributes.Family | MemberAttributes.Override;
				CurrentType.Members.Add(method);

				//public void Remove(publishersRow row) 
				//{
				//	this.Rows.Remove(row);
				//}
                method = new CodeMemberMethod();
				method.Name = "Remove";
				method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                method.Parameters.Add(
					new CodeParameterDeclarationExpression(element.TypeName, "row"));
				method.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "Rows"), 
						"Remove", new CodeExpression[] { new CodeVariableReferenceExpression("row") } ));
				CurrentType.Members.Add(method);

				//public void Remove(DataRow row) 
				//{
				//	this.Rows.Remove(row);
				//}
                method = new CodeMemberMethod();
				method.Name = "Remove";
				method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                method.Parameters.Add(
					new CodeParameterDeclarationExpression(typeof(DataRow), "row"));
				method.Statements.Add(
					new CodeMethodInvokeExpression(
						new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "Rows"), 
						"Remove", new CodeExpression[] { new CodeVariableReferenceExpression("row") } ));
				CurrentType.Members.Add(method);
				#endregion
			}
		}

		/// <summary>
		/// Creates the handler which raises the event, i.e. OnRowChanged.
		/// </summary>
		/// <param name="elementTypeName">The class corresponding to the element. i.e. AuthorDataRow</param>
		/// <param name="eventName">The name of the event to be raised, i.e. Changing.</param>
		private CodeMemberMethod OnEvent(string elementTypeName, string eventName)
		{
			//protected override void OnRow[eventName](DataRowChangeEventArgs e) 
			//{
			//	base.OnRow[eventName](e);
			//	if ((this.[elementTypeName][eventName] != null)) 
			//	{
			//		this.[elementTypeName][eventName](this, new [elementTypeName]ChangeEvent(new [elementTypeName](e.Row), e.Action));
			//	}
			//}
			CodeMemberMethod method = new CodeMemberMethod();
			method.Name = "OnRow" + eventName;
			method.Attributes = MemberAttributes.Family | MemberAttributes.Override;
			method.Parameters.Add(
				new CodeParameterDeclarationExpression(typeof(DataRowChangeEventArgs), "e"));
			//base.OnRowChanged(e);
			method.Statements.Add(
				new CodeMethodInvokeExpression(
					new CodeBaseReferenceExpression(), "OnRow" + eventName, 
					new CodeExpression[] { new CodeVariableReferenceExpression("e") } ));
			method.Statements.Add(new CodeConditionStatement(
				//if (
				new CodeBinaryOperatorExpression(
					//(this.publishersRowChanged
					new CodePropertyReferenceExpression(
						new CodeThisReferenceExpression(), elementTypeName + eventName),
						//!= null)
						CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)),
					new CodeStatement[] {
						//this.publishersRowChanged(this, new publishersRowChangeEvent(new publishersRow(e.Row), e.Action));
						new CodeExpressionStatement(
							new CodeMethodInvokeExpression(
								//this.publishersRowChanged(
								new CodeThisReferenceExpression(), elementTypeName + eventName,
								new CodeExpression[] {
									//this, 
									new CodeThisReferenceExpression(),
									//new publishersRowChangeEvent(
									new CodeObjectCreateExpression(
										elementTypeName + "ChangeEvent",
										new CodeExpression[] {
											//new publishersRow(
											new CodeObjectCreateExpression(
												elementTypeName, 
													new CodeExpression[] { 
														//e.Row)
														new CodePropertyReferenceExpression(
															new CodeVariableReferenceExpression("e"), "Row")
																			}),
											//e.Action)
											new CodePropertyReferenceExpression(
												new CodeVariableReferenceExpression("e"), "Action")
																})
														}))
										}));
			return method;
		}
	}
}
