//===============================================================================
// NMatrix XGoF Plug-in - XDL
// http://sourceforge.net/projects/dotnetopensrc/
//
// BaseVisitor.cs
// Contains the base class used by XDL visitor implementations.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.XSD;
using NMatrix.XGoF.Visitors;

namespace NMatrix.XGoF.PlugIn.XDL
{
	/// <summary>
	/// Base class for XDL visitors.
	/// </summary>
	public abstract class BaseVisitor : BaseCodeVisitor
	{
		/// <summary>
		/// Flag indicating that the current element has the IsDataSet="true" attribute.
		/// </summary>
		protected bool IsDataSet = false;
		/// <summary>
		/// Save a reference to the dataset type declaration inside the current schema.
		/// </summary>
		protected CodeTypeDeclaration CurrentDataSetType = null;
		/// <summary>
		/// Represents the namespaces in the schema.
		/// </summary>
		protected XmlNamespaceManager Namespaces;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public BaseVisitor()
		{
		}

		/// <summary>
		/// Called for complex elements. Sets the <c>IsDataSet</c> flag.
		/// </summary>
		/// <param name="element"></param>
		public override void Visit(VisitableElementComplexType element)
		{
			base.Visit(element);
			IsDataSet = false;

			// Search for the IsDataSet attribute.
			XmlAttribute[] attributes = element.SchemaObject.UnhandledAttributes;
			if ( attributes != null )
				foreach (XmlAttribute attr in attributes)
					if (attr.LocalName == "IsDataSet" && attr.Value == "true")
					{
						IsDataSet = true;
						break;
					}
		}

		/// <summary>
		/// Initializes <c>CurrentDataSetType</c> and <c>Namespaces</c> protected members.
		/// </summary>
		/// <param name="schema"></param>
		public override void Visit(VisitableSchemaRoot schema)
		{
			base.Visit(schema);

			// Find the dataset type declaration.
			foreach (CodeTypeMember member in CurrentNamespace.Types)
				if (member.UserData.Contains("IsDataSet") && 
					((XmlAttribute)member.UserData["IsDataSet"]).Value == "true")
				{
					CurrentDataSetType = member as CodeTypeDeclaration;
					break;
				}

			if (CurrentDataSetType == null) ThrowDataSetNotFound();

			// Remove collection type naming convention from the dataset name.
			CurrentDataSetType.Name = CurrentDataSetType.Name.Replace(Configuration.CollectionNaming, "");

			// Initialize the NamespaceManger to use for resolving XPath expressions in descendents.
			Namespaces = new XmlNamespaceManager(new NameTable());
			XmlQualifiedName[] nss = CurrentSchema.Namespaces.ToArray();
			foreach (XmlQualifiedName ns in nss)
				if (ns.Name != String.Empty)
				{
					Namespaces.NameTable.Add(ns.Name);
					Namespaces.NameTable.Add(ns.Namespace);
					Namespaces.AddNamespace(ns.Name, ns.Namespace);
				}
		}

		/// <summary>
		/// Returns the method specified. If the method doesn't exist, it adds a new private 
		/// final (sealed) method without return type.
		/// </summary>
		/// <param name="type">The type to return the method from.</param>
		/// <param name="method">The name of the method to return.</param>
		/// <returns>The method type member.</returns>
		protected CodeMemberMethod RetrieveMethod(CodeTypeDeclaration type, string method)
		{
			CodeMemberMethod result = null;
			foreach (CodeTypeMember member in type.Members)
				if (member.Name == method && member is CodeMemberMethod)
				{
					result = member as CodeMemberMethod;
					break;
				}

			if (result == null)
			{
				result = new CodeMemberMethod();
				result.Name = method;
				result.Attributes = MemberAttributes.Private | MemberAttributes.Final;
				type.Members.Add(result);
			}

			return result;
		}

		/// <summary>
		/// Returns the prefix associated to the target namespace of the source file.
		/// </summary>
		protected string RetrievePrefix()
		{
			// This should work, but it doesn't.
			// return Namespaces.LookupPrefix(CurrentSchema.TargetNamespace);
			return Namespaces.LookupPrefix(Namespaces.NameTable.Get(CurrentSchema.TargetNamespace)) + ":";
			
			/* 
			 * It should work now with the new LookupPrefix call.
			if (CurrentSchema.ElementFormDefault == XmlSchemaForm.Qualified)
			{
				XmlQualifiedName[] nss = CurrentSchema.Namespaces.ToArray();
				foreach (XmlQualifiedName ns in nss)
					if (ns.Name != String.Empty && ns.Namespace == CurrentSchema.TargetNamespace)
						return ns.Name + ":";
			}
			return String.Empty;
			*/
		}

		/// <summary>
		/// Throws an exception signaling that the DataSet type declaration was't found.
		/// </summary>
		protected void ThrowDataSetNotFound()
		{
			throw new InvalidOperationException("The DataSet type declaration wasn't found in the generated code."
				+ Environment.NewLine + "Check the configuration files.");
		}
	}
}
