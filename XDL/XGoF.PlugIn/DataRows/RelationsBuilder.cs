using System;
using System.Data;
using System.CodeDom;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using NMatrix.XGoF.PlugIn.XDL;
using NMatrix.XGoF.XSD;

namespace NMatrix.XGoF.PlugIn.XDL.DataRows
{
	/// <summary>
	/// Analyzes and adds properties and variables corresponding to ForeignKey,
	/// UniqueKey and PrimaryKeys associated with the element. Adds child-related methods
	/// to the class.
	/// </summary>
	public class RelationsBuilder : BaseVisitor
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public RelationsBuilder()
		{
		}

		/// <summary>
		/// Looks for keys which use the element, and adds parent or child references.
		/// </summary>
		/// <param name="element">The element being inspected.</param>
		/// <remarks>In an xsd schema, the following represents a foreign-key:
		///		&gt;xsd:keyref name="publisherstitles" refer="publisherKey"&lt;
		///			&gt;xsd:selector xpath=".//mstns:titles" /&lt;
		///			&gt;xsd:field xpath="mstns:pub_id" /&lt;
		///		&gt;/xsd:keyref&lt;
		/// the "refer" attribute designates the parent key.
		///	The xsd:selector represents the child element and xsd:field the 
		///	children field which points to the parent key. There may be multiple fields.
		/// </remarks>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);

			if (!IsDataSet)
			{
				string xpath;
				#region Look for parent relations
				// We need to look for every keyref where the current element appears in the xsd:selector element
				xpath = ".//";
				xpath += RetrievePrefix();
				xpath += element.Name;
				xpath = ".//xsd:keyref[xsd:selector/@xpath=\"" + xpath + "\"]";
				XmlNodeList keyrefs = CurrentSchemaDom.SelectNodes(xpath, Namespaces);
				
				foreach (XmlNode keyref in keyrefs)
				{
					string parent;
					xpath = ".//xsd:key[@name=\"" + keyref.Attributes["refer"].Value + "\"] | ";
					xpath += ".//xsd:unique[@name=\"" + keyref.Attributes["refer"].Value + "\"]";
                    XmlNode key = CurrentSchemaDom.SelectSingleNode(xpath, Namespaces);
					XmlNode parentnode = CurrentSchemaDom.SelectSingleNode(
						"//xsd:element[@name=\"" + 
						Regex.Replace(key.SelectSingleNode("xsd:selector/@xpath", Namespaces).Value, 
							"([A-z|0-9|.|/]+):", "") + "\"]", Namespaces);
					parent = parentnode.Attributes["name"].Value;
					
					//public publishersRow publishers
					CodeMemberProperty prop = new CodeMemberProperty();
					prop.Name = parent;
					prop.Attributes = MemberAttributes.Public;
					prop.HasGet = true;
					prop.HasSet = true;
					prop.Type = new CodeTypeReference(parent + Configuration.TypeNaming);

					#region Property Get
					//	get { 
					//		return ((publishersRow)GetParentRow("publisherstitles", typeof(publishersRow))); 
					//		}
					prop.GetStatements.Add(
						//return
						new CodeMethodReturnStatement(
							//(publishersRow)
							new CodeCastExpression(
								parent + Configuration.TypeNaming, 
								//(GetParentRow
								new CodeMethodInvokeExpression(null, "GetParentRow",
									//"publishertitltes"
									new CodeExpression[] {
										new CodePrimitiveExpression(keyref.Attributes["name"].Value), 
										//, typeof(publishersRow)
										new CodeTypeOfExpression(parent + Configuration.TypeNaming) 
														 } ))));
					#endregion

					#region Property Set
					//	set { SetParentRow(value, this.Table.ParentRelations["publisherstitles"]); }
					prop.SetStatements.Add(
						//SetParentRow
						new CodeMethodInvokeExpression(null, "SetParentRow",
							new CodeExpression[] {
								//(value
								new CodeVariableReferenceExpression("value"),
								//, this.Table.ParentRelations
								new CodeIndexerExpression(
									new CodePropertyReferenceExpression(
										new CodePropertyReferenceExpression(
											new CodeThisReferenceExpression(), "Table"),
											"ParentRelations"),
									//["publisherstitles"]
									new CodePrimitiveExpression(keyref.Attributes["name"].Value)) 
													}));
					#endregion

					CurrentType.Members.Add(prop);
				}
				#endregion

				#region Look for child relations
				// We need to look for every keyref where the current element appears in the "refer" key
				xpath = ".//";
				xpath += RetrievePrefix();
				xpath += element.Name;
				xpath = ".//xsd:key[xsd:selector/@xpath=\"" + xpath + "\"] | " + 
						".//xsd:unique[xsd:selector/@xpath=\"" + xpath + "\"]";
				XmlNodeList keys = CurrentSchemaDom.SelectNodes(xpath, Namespaces);
				
				// For each key, try to find a keyref pointing to it.
				foreach (XmlNode key in keys)
				{
					xpath = ".//xsd:keyref[@refer=\"" + key.Attributes["name"].Value + "\"]";
                    XmlNode keyref = CurrentSchemaDom.SelectSingleNode(xpath, Namespaces);

					if (keyref != null)
					{
						// Convert ".//mstns:titles" to "titles"
						string child = Regex.Replace(
							keyref.SelectSingleNode("xsd:selector/@xpath", Namespaces).Value, 
							"([A-z|0-9|.|/]+):", "");

						//public titlesRow[] titles() {
						//    return ((titlesRow[])(GetChildRows("publisherstitles", typeof(titlesRow)));
						//}
						CodeMemberProperty prop = new CodeMemberProperty();
						//titlesRow[]
						CodeTypeReference type = new CodeTypeReference(child + Configuration.TypeNaming, 1);
                        prop.Name = child;
						prop.Type = type;
						prop.Attributes = MemberAttributes.Public | MemberAttributes.Final;
						prop.HasGet = true;
						prop.GetStatements.Add(
							//return
							new CodeMethodReturnStatement(
								//(titlesRow[])
								new CodeCastExpression(type, 
									new CodeMethodInvokeExpression(null, "GetChildRows",
										new CodeExpression[] { 
											//"publisherstitles"
											new CodePrimitiveExpression(keyref.Attributes["name"].Value), 
											new CodeTypeOfExpression(child + Configuration.TypeNaming) 
															 } ))));
						CurrentType.Members.Add(prop);
					}
				}
				#endregion
			}
		}
	}
}
