//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ConfigurationLoader.cs
// Provides support for ConfigurationSettings and ConfigurationRetriever classes.
// Manages validation of configuration files (".config") and checks and retrieves
// configuration handlers and data nodes.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Reflection;
using System.Collections;
using System.Configuration;
using NMatrix.Core.Utility;
using NMatrix.Core.Xml;

namespace NMatrix.Core.Configuration
{
	/// <summary>
	/// The loader of configuration files.
	/// </summary>
	public class ConfigurationLoader
	{
		private ConfigurationLoader()
		{
		}

		/// <summary>
		/// Retrieve the specified section from the specified configuration file.
		/// </summary>
		/// <param name="sectionName">The section to retrieve.</param>
		/// <param name="document">The document to use to retrieve the section.</param>
		/// <returns>The section handler associated with the section.</returns>
		public static IConfigurationSectionHandler GetSectionHandler(string sectionName, XmlDocument document)
		{
			string xpath = "//section[@name=\"" + sectionName + "\"]";
			XmlNode section = document.SelectSingleNode(xpath);

            if (section == null) return null;
			return Reflection.GetObject(section.Attributes["type"].Value) as IConfigurationSectionHandler;
		}

		/// <summary>
		/// Returns the node matching the section name, from the root node ("//").
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		/// <param name="document">The document to use to retrieve the section.</param>
		/// <returns>The node matching the section.</returns>
		public static XmlNode GetNode(string sectionName, XmlDocument document)
		{
			return document.SelectSingleNode("//" + sectionName);
		}

		/// <summary>
		/// Return the nodes from the root section in the file, that is, the "configuration" section.
		/// </summary>
		/// <param name="document">The document to use to retrieve the section.</param>
		public static XmlNode GetRootNode(XmlDocument document)
		{
			return GetNode("configuration", document);
		}

		/// <summary>
		/// Validates that every section has its corresponding section handler.
		/// </summary>
		/// <param name="document">The configuration file to check.</param>
		public static void ValidateSections(XmlDocument document)
		{
			XmlNodeList nodes = document.SelectNodes("//section");
			Hashtable sections = new Hashtable(nodes.Count);
			foreach (XmlNode section in nodes)
				sections.Add(section.Attributes["name"].Value, section.Attributes["type"].Value);

			nodes = document.SelectNodes("//configuration/*");
			ValidateNodes(nodes, sections);
		}

		/// <summary>
		/// Validates that every section has its corresponding section handler,
		/// accumulating sections defined in a second document.
		/// </summary>
		/// <param name="document">The configuration file to check.</param>
		/// <param name="appSettings">A second configuration document to use for accumulating sections.</param>
		public static void ValidateSections(XmlDocument document, XmlDocument appSettings)
		{
			XmlNodeList nodes = document.SelectNodes("//section");
			Hashtable sections = new Hashtable(nodes.Count);
			foreach (XmlNode section in nodes)
				sections.Add(section.Attributes["name"].Value, section.Attributes["type"].Value);

			nodes = appSettings.SelectNodes("//section");
			foreach (XmlNode section in nodes)
				if (!sections.ContainsKey(section.Attributes["name"].Value))
                    sections.Add(section.Attributes["name"].Value, section.Attributes["type"].Value);

			nodes = document.SelectNodes("//configuration/*");
			ValidateNodes(nodes, sections);
		}

		/// <summary>
		/// Checks that the section has been defined and the type can be loaded.
		/// </summary>
		/// <param name="nodes">The nodes to check.</param>
		/// <param name="sections">The list of sections defined.</param>
		private static void ValidateNodes(XmlNodeList nodes, Hashtable sections)
		{
			foreach (XmlNode node in nodes)
			{
				if (!(node.LocalName == "sectionGroup" || node.LocalName == "configSections"))
				{
					if (!sections.Contains(node.LocalName))
						throw new ConfigurationException("Unrecognized configuration section '" 
							+ node.LocalName + "'");
					else
					{
						IConfigurationSectionHandler handler;
						try
						{
							handler = Reflection.GetObject(sections[node.LocalName].ToString()) as IConfigurationSectionHandler;
						}
						catch
						{
							throw new ConfigurationException("Couldn't load section handler for '" + node.LocalName + "'");
						}
						if (handler == null) 
							throw new ConfigurationException("Couldn't load section handler for '" + node.LocalName + "'");
					}
				}
			}
		}

		/// <summary>
		/// Validates a document against a schema, and throws an exception if validation fails.
		/// </summary>
		/// <param name="document">The document to validate.</param>
		/// <param name="schema">The schema for the document.</param>
		public static void ValidateDocument(XmlDocument document, XmlSchema schema)
		{
			Validator valid = new Validator();
			valid.Validate(document, schema);
		}
	}
}
