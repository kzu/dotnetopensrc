//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// CustomizationRetriever.cs
// Administers retrieval of customizations applicable to a specific element. 
// This is the default customizations retriever.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using NMatrix.XGoF.Host;
using NMatrix.XGoF.XSD;
using NMatrix.Core;
using NMatrix.Core.Xml;
using NMatrix.Core.ComponentModel;
using NMatrix.Core.Utility;

namespace NMatrix.XGoF.Customization
{
	/// <summary>
	/// Administers retrieval of customizations applicable to a specific element.
	/// </summary>
	internal class CustomizationRetriever : HostedComponent, ICustomizationRetriever
	{
		private Hashtable _managers = new Hashtable();

		/// <summary>
		/// Constructor initializes the namespace manager used by XPath queries.
		/// It loads the configuration schema from the same location as the assembly.
		/// </summary>
		public CustomizationRetriever()
		{
		}

		/// <summary>
		/// Retrieves the customization nodes that apply to the specified element.
		/// </summary>
		/// <param name="element">The element to use as a filter.</param>
		/// <param name="type">The type of node to look for (Type or Collection)</param>
		/// <param name="files">The list of XmlDocuments to use for retrieval.</param>
		/// <returns>An <c>ArrayList</c> of matching XmlNodes.</returns>
		public ArrayList RetrieveCustomization(BaseSchemaTypedElement element, NodeType type, ArrayList files)
		{
			if (element is VisitableElementComplexType)
				return GetTypeConfiguration(element as VisitableElementComplexType, type, files);
			else if (element is BaseLeafSchemaElement)
				return GetPropertyConfiguration(element as BaseLeafSchemaElement, files);
			return new ArrayList();
		}

		/// <summary>
		/// Retrieves the customization nodes that apply to the specified element.
		/// </summary>
		/// <param name="element">The element to use as a filter.</param>
		/// <param name="files">The list of XmlDocuments to use for retrieval.</param>
		/// <returns>An <c>ArrayList</c> of matching XmlNodes.</returns>
		public ArrayList RetrieveCustomization(BaseSchemaTypedElement element, ArrayList files)
		{
			if (element is VisitableElementComplexType)
				return GetTypeConfiguration(element as VisitableElementComplexType, NodeType.Type, files);
			else if (element is BaseLeafSchemaElement)
				return GetPropertyConfiguration(element as BaseLeafSchemaElement, files);
			return null;
		}

		/// <summary>
		/// Retrieves a list containing XmlNode objects with the configurations that apply to
		/// the element and nodeType (Type or Collection).
		/// </summary>
		/// <param name="element">The XmlSchemaElement to filter by.</param>
		/// <param name="nodeType">Can be eiter "Type" or "Collection", which are the possible
		/// configuration nodes</param>
		/// <param name="files">The list of XmlDocuments to use for retrieval.</param>
		/// <returns>ArrayList with the XmlNode objects matching the criteria.</returns>
		private ArrayList GetTypeConfiguration(VisitableElementComplexType element, NodeType nodeType, ArrayList files)
		{
			ArrayList results = new ArrayList();
			StringWriter w = new StringWriter();

			XmlAttribute[] attributes = element.SchemaObject.UnhandledAttributes;

			// TODO: Add the posibility to specify wildcards in ApplyTo/ExceptOf filters.

			//Open the query
			w.Write("//xgf:" + nodeType.ToString() + "[");
		
			//---- ApplyTo section ----//
			//Check for /Name
			w.Write("(not(boolean(xgf:ApplyTo/xgf:Name)) or xgf:ApplyTo/xgf:Name=\"{0}\")", element.Name);
	            
			//Check for /Attribute 
			if (attributes != null)
				foreach (XmlAttribute attr in attributes)
				{
					w.Write(" and (not(boolean(xgf:ApplyTo/xgf:Attribute/@Name=\"{0}\"))", attr.Name);
					w.Write(" or (xgf:ApplyTo/xgf:Attribute/@Name=\"{0}\" and xgf:ApplyTo/xgf:Attribute/@Value=\"{1}\"))", attr.Name, attr.Value);
				}
			//------------------------//

			//---- ExceptOf section ----//
			//Check for /Name
			w.Write(" and not(xgf:ExceptOf/xgf:Name=\"{0}\")", element.Name);
	            
			//Check for /Attribute
			if (attributes != null)
				foreach (XmlAttribute attr in attributes)
				w.Write(" and (not(xgf:ExceptOf/xgf:Attribute/@Name=\"{0}\" and xgf:ExceptOf/xgf:Attribute/@Value=\"{1}\"))", attr.Name, attr.Value);
			//------------------------//

			//Close the query
			w.Write("]");

			string xpath = w.ToString();

			// Runs the xpath query against every file in the list.
			foreach (XmlDocument doc in files)
			{
				XmlNamespaceManager mgr = GetManager(doc);
				XmlNodeList nodes = doc.SelectNodes(xpath, mgr);
				bool passed;
				foreach (XmlNode node in nodes)
				{
					// Check that attributes exist if they are present in ApplyTo section
					XmlNodeList attrnodes = node.SelectNodes("//xgf:" + nodeType.ToString() + "/xgf:ApplyTo/xgf:Attribute", mgr);
					passed = false;
					foreach (XmlNode attrnode in attrnodes)
						if (attributes != null)
							foreach (XmlAttribute attr in attributes)
							{
								if (attr.Name == attrnode.Attributes["Name"].Value)
									if (attr.Value == attrnode.Attributes["Value"].Value)
										passed = true;
									else
										passed = false;
							}
					// If the attributes have passed or there are no attributes in the ApplyTo section.
					if (passed || attrnodes.Count == 0) results.Add(node);
				}
			}
			return results;
		}
	
		/// <summary>
		/// Retrieves a list containing XmlNode objects with the configurations that apply to
		/// a property (Attribute or Element with a SimpleType or intrinsic XSD type.
		/// </summary>
		/// <param name="element">The XmlSchemaElement to filter by.</param>
		/// <param name="files">The list of XmlDocuments to use for retrieval.</param>
		/// <returns>ArrayList with the XmlNode objects matching the criteria.</returns>
		private ArrayList GetPropertyConfiguration(BaseLeafSchemaElement element, ArrayList files)
		{
			// Retrieve the parent configuration, to load results from them.
			ArrayList parentconfig = GetTypeConfiguration(element.Parent as VisitableElementComplexType, NodeType.Type, files);
			ArrayList results = new ArrayList();

			// If no configuration applies to the parent element, exit the method.
			if (parentconfig.Count == 0) return results;

			XmlAttribute[] attributes = ((XmlSchemaAnnotated)element.SchemaObject).UnhandledAttributes;

			StringWriter w = new StringWriter();

			//Open the query
			w.Write("//xgf:Property[");
		
			//---- ApplyTo section ----//
			//Check for /Name
			w.Write("(not(boolean(xgf:ApplyTo/xgf:Name)) or xgf:ApplyTo/xgf:Name=\"{0}\")", element.Name);
	            
			//Check for /Attribute 
			if (attributes != null)
				foreach (XmlAttribute attr in attributes)
				{
					w.Write(" and (not(boolean(xgf:ApplyTo/xgf:Attribute/@Name=\"{0}\"))", attr.Name);
					w.Write(" or (xgf:ApplyTo/xgf:Attribute/@Name=\"{0}\" and xgf:ApplyTo/xgf:Attribute/@Value=\"{1}\"))", attr.Name, attr.Value);
				}
			//------------------------//

			//---- ExceptOf section ----//
			//Check for /Name
			w.Write(" and not(xgf:ExceptOf/xgf:Name=\"{0}\")", element.Name);
	            
			//Check for /Attribute
			if (attributes != null)
				foreach (XmlAttribute attr in attributes)
					w.Write(" and (not(xgf:ExceptOf/xgf:Attribute/@Name=\"{0}\" and xgf:ExceptOf/xgf:Attribute/@Value=\"{1}\"))", attr.Name, attr.Value);
			//------------------------//

			//Close the query
			w.Write("]");
			string xpath = w.ToString();

			// Traverse the parent nodes and select the nodes matching the query.
			foreach (XmlNode conf in parentconfig)
			{
				XmlNodeList nodes = conf.SelectNodes(xpath, GetManager(conf.OwnerDocument));
				foreach (XmlNode node in nodes)
					results.Add(node);
			}
			return results;
		}

		/// <summary>
		/// Retrieves the namespace manager applicable to the current <c>XmlDocument</c>.
		/// </summary>
		/// <param name="currentDocument">The document which NameTable is going to be used.</param>
		/// <remarks>The managers are cached for improved performance, and are indexed by the
		/// XmlDocument.</remarks>
		private XmlNamespaceManager GetManager(XmlDocument currentDocument)
		{
			if (_managers.ContainsKey(currentDocument))
				return _managers[currentDocument] as XmlNamespaceManager;
			XmlNamespaceManager mgr = new XmlNamespaceManager(currentDocument.NameTable);
			
			XmlQualifiedName[] names = GeneratorHost.Instance.CustomizationSchema.Namespaces.ToArray();
			foreach (XmlQualifiedName ns in names)
				mgr.AddNamespace(ns.Name, ns.Namespace);
			
			_managers.Add(currentDocument, mgr);
			return mgr;
		}
	}
}
