//===============================================================================
// NMatrix XGoF Generator XDL PlugIn.
// http://www.deverest.com.ar/XGoF
//
// DelegatesBuilder.cs
// Adds delegate declarations and events to the DataTable elements.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Data;
using System.CodeDom;
using NMatrix.XGoF.PlugIn.XDL;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.PlugIn.XDL.DataSets
{
	
	/// <summary>
	/// Adds delegate declarations and events to the DataTable elements.
	/// </summary>
	public class DelegatesBuilder : BaseVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DelegatesBuilder()
		{
		}

		/// <summary>
		/// Visitor implementation. 
		/// </summary>
		/// <param name="element">The element from which to build the code.</param>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (!IsDataSet)
			{
				#region Delegate declaration
                CodeTypeDelegate del = new CodeTypeDelegate();
				del.Name = element.TypeName + "ChangeEventHandler";
				//param object sender
				del.Parameters.Add(
					new CodeParameterDeclarationExpression(
						typeof(object), "sender"));
				//param publishersRowChangeEvent e
				del.Parameters.Add(
					new CodeParameterDeclarationExpression(
					element.TypeName + "ChangeEvent", "e"));
				//public delegate void publishersRowChangeEventHandler(object sender, publishersRowChangeEvent e);
				CurrentDataSetType.Members.Add(del);
				#endregion

				#region Event argument class
				CodeTypeDeclaration ev = new CodeTypeDeclaration(element.TypeName + "ChangeEvent");
				//[System.Diagnostics.DebuggerStepThrough()]
                ev.CustomAttributes.Add(
					new CodeAttributeDeclaration("System.Diagnostics.DebuggerStepThrough"));
				//public class publishersRowChangeEvent : EventArgs {
				ev.BaseTypes.Add(typeof(EventArgs));
				//    private publishersRow eventRow;
				ev.Members.Add(new CodeMemberField(element.TypeName, "eventRow"));
				//    private DataRowAction eventAction;
				ev.Members.Add(new CodeMemberField(typeof(DataRowAction), "eventAction"));
					#region Constructor
					//    public publishersRowChangeEvent(publishersRow row, DataRowAction action)
					CodeConstructor ctor = new CodeConstructor();
					ctor.Parameters.Add(
						new CodeParameterDeclarationExpression(element.TypeName, "row"));
					ctor.Parameters.Add(
						new CodeParameterDeclarationExpression(typeof(DataRowAction), "action"));
					ctor.Attributes = MemberAttributes.Public;
						//        this.eventRow = row;
						ctor.Statements.Add(
							new CodeAssignStatement(
								new CodePropertyReferenceExpression(
									new CodeThisReferenceExpression(), "eventRow"),
								new CodeVariableReferenceExpression("row")));
						//        this.eventAction = action;
						ctor.Statements.Add(
							new CodeAssignStatement(
							new CodePropertyReferenceExpression(
							new CodeThisReferenceExpression(), "eventAction"),
							new CodeVariableReferenceExpression("action")));
					ev.Members.Add(ctor);
					#endregion
					#region Properties
					//    public publishersRow Row 
					
					//        get {
					//            return this.eventRow;
					//        }
					//    }
					CodeMemberProperty prop = new CodeMemberProperty();
					prop.Name = "Row";
					prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;
					prop.Type = new CodeTypeReference(element.TypeName);
					prop.HasGet = true;
					prop.GetStatements.Add(
						new CodeMethodReturnStatement(
							new CodePropertyReferenceExpression(
								new CodeThisReferenceExpression(), "eventRow")));
					ev.Members.Add(prop);
					//    public DataRowAction Action {
					//        get {
					//            return this.eventAction;
					//        }
					prop = new CodeMemberProperty();
					prop.Name = "Action";
					prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;
					prop.Type = new CodeTypeReference(typeof(DataRowAction));
					prop.HasGet = true;
					prop.GetStatements.Add(
						new CodeMethodReturnStatement(
							new CodePropertyReferenceExpression(
								new CodeThisReferenceExpression(), "eventAction")));
					ev.Members.Add(prop);
					#endregion
				CurrentDataSetType.Members.Add(ev);
				#endregion
			}
		}
	}
}
