//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// CodeDomHelper.cs
// Helper class that creates CodeDom objects for the xxxBuilder classes.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.CodeDom;
using System.Collections;

namespace NMatrix.XGoF
{
	/// <summary>
	/// Helper class for CodeDom generation.
	/// </summary>
	public class CodeDomHelper
	{
		static CodeComment _xmlcomment;

		static CodeDomHelper()
		{
			_xmlcomment = new CodeComment();
			_xmlcomment.DocComment = true;
			_xmlcomment.Text = "<summary>XGoF generated member.</summary>";
		}

		private CodeDomHelper()
		{
		}

		internal static CodeComment EmptyComment
		{
			get { return _xmlcomment; }
		}

		/// <summary>
		/// Creates an array of namespaces to append to the CompileUnit.
		/// </summary>
		/// <param name="values">The ArrayList with the namespaces.</param>
		/// <returns>An array of CodeNamespaceImport objects.</returns>
		public static CodeNamespaceImport[] BuildNamespaceImports(
			ArrayList values)
		{
			CodeNamespaceImport[] imports = new CodeNamespaceImport[values.Count];

			for (int i = 0; i < values.Count; i ++)
			{
				imports[i] = new CodeNamespaceImport(values[i].ToString());
			}

			return imports;
		}

		/// <summary>
		/// Builds the attribute declaration objects to add to a class or property.
		/// </summary>
		/// <param name="nodes">The XmlNodes containing the attributes.</param>
		/// <returns>The CodeAttributeDeclarationCollection to add.</returns>
		public static CodeAttributeDeclarationCollection BuildCustomAttributes(
			ArrayList nodes)
		{
			CodeAttributeDeclarationCollection result = new CodeAttributeDeclarationCollection();

			foreach (XmlNode node in nodes)
			{
				foreach (XmlNode child in node.ChildNodes)
				{
					if (child.LocalName == "CustomAttribute")
					{
						CodeAttributeDeclaration attr = 
							new CodeAttributeDeclaration();
						string[] namevalue = child.InnerText.Split('(');
						attr.Name = namevalue[0];
						attr.Arguments.Add(new CodeAttributeArgument(
							new CodeSnippetExpression(namevalue[1].Substring(0, 
							namevalue[1].Length - 1))));
						result.Add(attr);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Builds the collection of CodeTypeReference for base types of a class.
		/// </summary>
		/// <param name="nodes">The nodes containing the configuration.</param>
		/// <returns>The CodeTypeReferenceCollection to add.</returns>
		public static CodeTypeReferenceCollection BuildBaseTypes(
			ArrayList nodes)
		{
			CodeTypeReferenceCollection result = new CodeTypeReferenceCollection();

			foreach (XmlNode node in nodes)
			{
				foreach (XmlNode child in node.ChildNodes)
				{
					if (child.LocalName == "BaseType")
						result.Add(child.InnerText);
				}
			}

			return result;
		}

		/// <summary>
		/// Parses the element's content, replacing Current and TypesIteration blocks.
		/// </summary>
		/// <param name="element">The string element name, to be replaced in Current instances, without the TypeNaming appended.</param>
		/// <param name="typeNaming">The string to append to the type name for class names.</param>
		/// <param name="collectionNaming">The string to append to the type name for collections.</param>
		/// <param name="source">The XmlNode with the source code to parse.</param>
		/// <param name="ns">The current namespace being used, for TypesIteration processing.</param>
		/// <returns>The parsed string, used to build CodeSnipped objects.</returns>
		public static string ParseCodeContainer(string element, string typeNaming, string collectionNaming, 
			XmlNode source, CodeNamespace ns)
		{
			XmlNode parsed = source.Clone();

			foreach (XmlNode child in parsed.ChildNodes)
			{
				if (child.LocalName == "TypesIteration")
					child.InnerXml = ParseTypes(element, typeNaming, collectionNaming, child, ns);
				else if (child.LocalName == "PropertiesIteration")
					child.InnerXml = ParseProperties(element, typeNaming, collectionNaming, child, ns);
				else if (child.LocalName == "Current")
					child.InnerText = element;
				else if (child.LocalName == "CurrentType")
					child.InnerText = element + typeNaming;
				else if (child.LocalName == "CurrentCollection")
					child.InnerText = element + collectionNaming;
			}
			
			return parsed.InnerText;
		}

		/// <summary>
		/// Parses PropertyIteration blocks.
		/// </summary>
		private static string ParseProperties(string element, string typeNaming, string collectionNaming, 
			XmlNode source, CodeNamespace ns)
		{
			StringWriter w = new StringWriter();
			CodeTypeDeclaration proptype = null;

			foreach (CodeTypeDeclaration type in ns.Types)
				if (type.Name == element + typeNaming)
				{
					proptype = type;
					break;
				}

			if (proptype == null) throw new InvalidOperationException("Expected property wasn't found in Type declaration.");

			foreach (CodeTypeMember prop in proptype.Members)
			{
				if (prop is CodeMemberProperty)
				{
					XmlNode cloned = source.Clone();
					foreach (XmlNode current in cloned.ChildNodes)
						if (current.LocalName == "TypesIteration")
							current.InnerXml = ParseTypes(element, typeNaming, collectionNaming, current, ns);
						else if (current.LocalName == "PropertiesIteration")
							current.InnerXml = ParseProperties(element, typeNaming, collectionNaming, current, ns);
						else if (current.LocalName == "Current")
							current.InnerText = element;
						else if (current.LocalName == "CurrentType")
							current.InnerText = element + typeNaming;
						else if (current.LocalName == "CurrentCollection")
							current.InnerText = element + collectionNaming;
						else if (current.LocalName == "CurrentProperty")
							current.InnerText = prop.Name;
						else if (current.LocalName == "PropertyType")
							current.InnerText = ((CodeMemberProperty)prop).Type.BaseType;
					w.Write(cloned.InnerText);
				}
			}
			return w.ToString();
		}

		/// <summary>
		/// Parses TypesIteration blocks.
		/// </summary>
		private static string ParseTypes(string element, string typeNaming, string collectionNaming, 
			XmlNode source, CodeNamespace ns)
		{
			StringWriter w = new StringWriter();
			foreach (CodeTypeDeclaration type in ns.Types)
			{
				if (!type.UserData.Contains("IsCollection"))
				{
					XmlNode cloned = source.Clone();
					foreach (XmlNode current in cloned.ChildNodes)
						if (current.LocalName == "TypesIteration")
							current.InnerXml = ParseTypes(type.Name.Replace(typeNaming, ""), typeNaming, collectionNaming, current, ns);
						else if (current.LocalName == "PropertiesIteration")
							current.InnerXml = ParseProperties(type.Name.Replace(typeNaming, ""), typeNaming, collectionNaming, current, ns);
						else if (current.LocalName == "Current")
							current.InnerText = type.Name.Replace(typeNaming, "");
						else if (current.LocalName == "CurrentType")
							current.InnerText = type.Name;
						else if (current.LocalName == "CurrentCollection")
							current.InnerText = type.Name.Replace(typeNaming, "") + collectionNaming;

					w.Write(cloned.InnerText);
				}
			}
			return w.ToString();
		}

		/// <summary>
		/// Creates a property object to add to the type.
		/// </summary>
		/// <param name="elementName">The element name, that is, the class containing the property.</param>
		/// <param name="propertyName">The property name.</param>
		/// <param name="typeNaming">The naming convention in effect for class naming (the postfix).</param>
		/// <param name="collectionNaming">The naming convention in effect for collection naming (the postfix).</param>
		/// <param name="propertyType">The Type of the property.</param>
		/// <param name="nodes">The configuration nodes that apply to this property.</param>
		/// <param name="currentNamespace">The current namespace in use, for source code parsing purposes.</param>
		/// <returns></returns>
		public static CodeMemberProperty BuildProperty(string elementName, string propertyName, string typeNaming,
			string collectionNaming, Type propertyType, ArrayList nodes, CodeNamespace currentNamespace)
		{
			CodeMemberProperty prop = new CodeMemberProperty();

			prop.Type = new CodeTypeReference(propertyType);
			prop.Name = propertyName;
			// TODO: Is it necessary to add attributes to control visibility?
			prop.Attributes = MemberAttributes.Public;
			prop.CustomAttributes.AddRange(BuildCustomAttributes(nodes));

			foreach (XmlNode child in nodes)
				foreach (XmlNode node in child.ChildNodes)
					if (node.LocalName == "Get")
					{
						prop.HasGet = true;
						prop.GetStatements.Add(new CodeSnippetStatement(
							ParseProperty(elementName, propertyName, typeNaming, collectionNaming, 
							prop.Type.BaseType, node, currentNamespace)));
					}
					else if (node.LocalName == "Set")
					{
						prop.HasSet = true;
						prop.SetStatements.Add(new CodeSnippetStatement(
							ParseProperty(elementName, propertyName, typeNaming, collectionNaming, 
							prop.Type.BaseType, node, currentNamespace)));
					}

			return prop;
		}

		/// <summary>
		/// Parses a single Property block (Get or Set)
		/// </summary>
		private static string ParseProperty(string parent, string property, string typeNaming, 
			string collectionNaming, string propertyType, XmlNode source, CodeNamespace ns)
		{
			XmlNode cloned = source.Clone();
			foreach (XmlNode current in cloned.ChildNodes)
				if (current.LocalName == "TypesIteration")
					current.InnerXml = ParseTypes(parent, typeNaming, collectionNaming, current, ns);
				else if (current.LocalName == "PropertiesIteration")
					current.InnerXml = ParseProperties(parent, typeNaming, collectionNaming, current, ns);
				else if (current.LocalName == "Current")
					current.InnerText = parent;
				else if (current.LocalName == "CurrentType")
					current.InnerText = parent + typeNaming;
				else if (current.LocalName == "CurrentCollection")
					current.InnerText = parent + collectionNaming;
				else if (current.LocalName == "CurrentProperty")
					current.InnerText = property;
				else if (current.LocalName == "PropertyType")
					current.InnerText = propertyType;

			return cloned.InnerText;
		}
	}
}
