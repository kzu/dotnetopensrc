//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ExtenderConfigurationSectionHandler.cs
// Loads configuration for the Extenders section.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Collections;
using System.Configuration;
using NMatrix.XGoF.XSD;
using NMatrix.XGoF.Customization;
using NMatrix.XGoF.Visitors;
using NMatrix.Core.Utility;
using NMatrix.Core.Patterns;
using NMatrix.Core.Collections;
using NMatrix.Core.Configuration;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Loads configuration for the Extenders section.
	/// </summary>
	public class ExtenderSectionHandler : IMergeConfigurationSectionHandler
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExtenderSectionHandler()
		{
		}

		/// <summary>
		/// Creates and initializes the configuration options.
		/// </summary>
		/// <param name="parent">Parent configuration node, the root configuration.</param>
		/// <param name="configContext">Not used.</param>
		/// <param name="configNode">The configuration node.</param>
		/// <returns>An <c>ExtenderSection</c> object.</returns>
		public object Create(object parent, object configContext, XmlNode configNode)
		{
			if (configNode == null) return null;
			ExtenderSection extender = new ExtenderSection();
			AddConfigurations(configNode, extender);
            return extender;
		}

		/// <summary>
		/// Merges an existing ExtenderSection object with the new data.
		/// </summary>
		public object Merge(object currentConfig, object parent, object configContext, XmlNode configNode)
		{
			ExtenderSection extender;
			if (currentConfig != null)
				extender = ((ExtenderSection)currentConfig).Clone() as ExtenderSection;
			else
				extender = new ExtenderSection();

			AddConfigurations(configNode, extender);
            return extender;
		}

		/// <summary>
		/// Processes the configuration node and adds the objects to the ExtenderSection.
		/// </summary>
		/// <param name="configNode">The node with the configuration data.</param>
		/// <param name="extender">The extender object to configure.</param>
		private void AddConfigurations(XmlNode configNode, ExtenderSection extender)
		{
			if (configNode == null) return;
			XmlNodeList nodes;

			try
			{
				#region Traversers
				nodes = configNode.SelectNodes("//traverser[@enabled=\"true\"]");
				if (nodes.Count > 1)
					throw new ConfigurationException("Only one Traverser can be enabled at a time.");

				if (nodes.Count != 0)
				{
					object traverser = Reflection.GetObject(nodes[0].Attributes["type"].Value);
					if (traverser == null) 
					{
						throw new ConfigurationException("Couldn't load Traverser: " + 
							nodes[0].Attributes["type"].Value + ".");
					}
					// Traversers must implement ITraverser.
					Reflection.EnsureInterface(traverser.GetType(), typeof(ITraverser));
					extender.Traverser = traverser as ITraverser;
				}
				#endregion

				#region Visitors
				// Retrieve all to remove disabled visitors.
				nodes = configNode.SelectNodes("//visitor");
				foreach (XmlNode node in nodes)
				{
					if (node.Attributes["enabled"] == null || node.Attributes["enabled"].Value == "true")
					{
						object visitor = Reflection.GetObject(node.Attributes["type"].Value);
						if (visitor == null) throw new ConfigurationException("Specified visitor type couldn't be loaded: " + node.Attributes["type"].Value);
						// Visitors must implement IVisitor.
						Type vtype = visitor.GetType();
						Reflection.EnsureInterface(vtype, typeof(IVisitor));
						bool add = true;
						foreach (DictionaryEntry v in extender.Visitors)
						{
							if (v.Value.GetType() == vtype)
							{
								add = false;
								break;
							}
						}

						if (add)
						{
							XmlAttribute attr = node.Attributes["runOrder"];
							int order = -1;
							if (attr != null)
								order = int.Parse(attr.Value);
							if (order == -1) order = int.MaxValue;
							extender.Visitors.Add(new SortedDuplicateKey(order), visitor);
						}
					}
					else
					{
						object[] visitors = new object[extender.Visitors.Count];
						extender.Visitors.CopyTo(visitors, 0);
						Type visitor = Reflection.LoadType(node.Attributes["type"].Value);
						// Start from the last element to safely remove at an specific ordinal position.
						for (int i = visitors.Length - 1; i >= 0; i--)
						{
							if (((DictionaryEntry)visitors[i]).Value.GetType() == visitor)
								extender.Visitors.RemoveAt(i);
						}
					}
				}
				#endregion

				#region Retrievers
				nodes = configNode.SelectNodes("//retriever");
				foreach (XmlNode node in nodes)
				{
					if (node.Attributes["enabled"].Value == "false")
					{
                        object[] retrievers = new object[extender.Retrievers.Count];
                        extender.Retrievers.CopyTo(retrievers, 0);
						// Start from the last element to safely remove at an specific ordinal position.
						for (int i = retrievers.Length; i > 0; i--)
							// We use StartsWith because the AssemblyQualifiedName contains version, token and language info. 
							if (((DictionaryEntry)retrievers[i]).Value.GetType().AssemblyQualifiedName.StartsWith(node.Attributes["type"].Value))
								extender.Retrievers.RemoveAt(i);
					}
					else
					{
						object retriever = Reflection.GetObject(node.Attributes["type"].Value);
						if (retriever == null) throw new ConfigurationException("Invalid Retriever type.");
						// Retrievers must implement ICustomizationRetriever.
						Reflection.EnsureInterface(retriever.GetType(), typeof(ICustomizationRetriever));
						extender.Retrievers.Add(retriever);
					}
				}
				#endregion
			}
			catch (Exception ex)
			{
				throw new ConfigurationException("Invalid values found in Extenders configuration.", ex);
			}
		}
	}	
}
