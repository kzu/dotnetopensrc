//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// GeneratorConfigurationSectionHandler.cs
// The class responsible for creating and initializing Generator configurations.
// Implements both IConfigurationSectionHandler and 
// IMergableConfigurationSectionHandler interfaces, to provide full configuration
// support.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Collections;
using System.Configuration;
using NMatrix.Core.Configuration;
using NMatrix.Core.Utility;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Initializes generator configuration options.
	/// </summary>
	public class GeneratorSectionHandler : IMergeConfigurationSectionHandler
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public GeneratorSectionHandler()
		{
		}

		/// <summary>
		/// Creates and initializes the configuration options.
		/// </summary>
		/// <param name="parent">Parent configuration node, the root configuration.</param>
		/// <param name="configContext">Not used.</param>
		/// <param name="configNode">The configuration node.</param>
		/// <returns>An <c>GeneratorOptions</c> object.</returns>
		/// <remarks>Only uses the first node found, which can have any suitable name, such as "Defaults".
		/// Node attributes are used for initialization.</remarks>
		public object Create(object parent, object configContext, XmlNode configNode)
		{
			if (configNode == null) return null;
			GeneratorSection options = new GeneratorSection();

			try
			{
				foreach (XmlNode node in configNode.ChildNodes)
				{
					if (node.LocalName == "default")
					{
						XmlAttribute attr;

						attr = node.Attributes["createNamespaceFolders"];
						if (attr != null) options.InnerData.CreateNamespaceFolders = bool.Parse(attr.Value);

						attr = node.Attributes["collectionNaming"];
						if (attr != null) options.InnerData.CollectionNaming = attr.Value;

						attr = node.Attributes["generateContainerProperty"];
						if (attr != null) options.InnerData.GenerateContainerProperty = bool.Parse(attr.Value);

						attr = node.Attributes["iterationType"];
						if (attr != null) options.InnerData.Iteration = (IterationType)Enum.Parse(typeof(IterationType), attr.Value);

						attr = node.Attributes["outputCompiled"];
						if (attr != null) options.InnerData.OutputCompiled = bool.Parse(attr.Value);

						attr = node.Attributes["outputSource"];
						if (attr != null) options.InnerData.OutputSource = bool.Parse(attr.Value);
					
						attr = node.Attributes["provider"];
						if (attr != null) 
						{
							object provider = Reflection.GetObject(attr.Value);
							if (provider == null) 
								throw new ConfigurationException("Invalid provider setting.");
							options.InnerData.Provider = provider as System.CodeDom.Compiler.CodeDomProvider;
						}

						attr = node.Attributes["targetAssembly"];
						if (attr != null) options.InnerData.TargetAssembly = attr.Value;

						attr = node.Attributes["targetFolder"];
						if (attr != null) options.InnerData.TargetFolder = attr.Value;

						attr = node.Attributes["targetNamespace"];
						if (attr != null) options.InnerData.TargetNamespace = attr.Value;

						attr = node.Attributes["typeNaming"];
						if (attr != null) options.InnerData.TypeNaming = attr.Value;

						attr = node.Attributes["validateCustomizations"];
						if (attr != null) options.InnerData.ValidateCustomizations = bool.Parse(attr.Value);
					}
					else if (node.LocalName == "namespaceImport")
						options.InnerData.NamespaceImports.Add(node.InnerText);
					else if (node.LocalName == "assemblyReference")
						options.InnerData.AssemblyReferences.Add(node.InnerText);
				}
			}
			catch (Exception ex)
			{
				throw new ConfigurationException("Invalid values found in Generator configuration.", ex);
			}
            return options;
		}

		/// <summary>
		/// <c>IMergableConfigurationSectionHandler</c> implementation.
		/// </summary>
		public object Merge(object currentConfig, object parent, object configContext, XmlNode configNode)
		{
			GeneratorSection options = new GeneratorSection();
			GeneratorSection current = currentConfig as GeneratorSection;
			if (current == null) current = new GeneratorSection();

			if (current == null && configNode == null) return null;
			if (configNode == null) return current;

			options.InnerData.NamespaceImports = 
				current.InnerData.NamespaceImports.Clone() as ArrayList;
			options.InnerData.AssemblyReferences = 
				current.InnerData.AssemblyReferences.Clone() as ArrayList;

			try
			{
				foreach (XmlNode node in configNode.ChildNodes)
				{
					if (node.LocalName == "default")
					{
						XmlAttribute attr;

						attr = node.Attributes["collectionNaming"];
						if (attr != null) 
							options.InnerData.CollectionNaming = attr.Value;
						else
							options.InnerData.CollectionNaming = current.InnerData.CollectionNaming;

						attr = node.Attributes["createNamespaceFolders"];
						if (attr != null) 
							options.InnerData.CreateNamespaceFolders = bool.Parse(attr.Value);
						else
							options.InnerData.CreateNamespaceFolders = current.InnerData.CreateNamespaceFolders;

						attr = node.Attributes["generateContainerProperty"];
						if (attr != null) 
							options.InnerData.GenerateContainerProperty = bool.Parse(attr.Value);
						else
							options.InnerData.GenerateContainerProperty = current.InnerData.GenerateContainerProperty;
						
						attr = node.Attributes["iterationType"];
						if (attr != null) 
							options.InnerData.Iteration = (IterationType)Enum.Parse(typeof(IterationType), attr.Value);
						else
							options.InnerData.Iteration = current.InnerData.Iteration;

						attr = node.Attributes["outputCompiled"];
						if (attr != null) 
							options.InnerData.OutputCompiled = bool.Parse(attr.Value);
						else
							options.InnerData.OutputCompiled = current.InnerData.OutputCompiled;

						attr = node.Attributes["outputSource"];
						if (attr != null) 
							options.InnerData.OutputSource = bool.Parse(attr.Value);
						else
							options.InnerData.OutputSource = current.InnerData.OutputSource;					

						attr = node.Attributes["provider"];
						if (attr != null) 
						{
							object provider = Reflection.GetObject(attr.Value);
							if (provider == null) 
								throw new ConfigurationException("Invalid CodeDomProvider setting.");
							options.InnerData.Provider = provider as System.CodeDom.Compiler.CodeDomProvider;
						}
						else
							options.InnerData.Provider = current.InnerData.Provider;

						attr = node.Attributes["targetAssembly"];
						if (attr != null) 
							options.InnerData.TargetAssembly = attr.Value;
						else
							options.InnerData.TargetAssembly = current.InnerData.TargetAssembly;

						attr = node.Attributes["targetFolder"];
						if (attr != null) 
							options.InnerData.TargetFolder = attr.Value;
						else
							options.InnerData.TargetFolder = current.InnerData.TargetFolder;

						attr = node.Attributes["targetNamespace"];
						if (attr != null) 
							options.InnerData.TargetNamespace = attr.Value;
						else
							options.InnerData.TargetNamespace = current.InnerData.TargetNamespace;

						attr = node.Attributes["typeNaming"];
						if (attr != null) 
							options.InnerData.TypeNaming = attr.Value;
						else
							options.InnerData.TypeNaming = current.InnerData.TypeNaming;

						attr = node.Attributes["validateCustomizations"];
						if (attr != null) options.InnerData.ValidateCustomizations = bool.Parse(attr.Value);
					}
					else if (node.LocalName == "namespaceImport")
					{
						if (!options.InnerData.NamespaceImports.Contains(node.InnerText))
							options.InnerData.NamespaceImports.Add(node.InnerText);
					}
					else if (node.LocalName == "assemblyReference")
						if (!options.InnerData.AssemblyReferences.Contains(node.InnerText))
							options.InnerData.AssemblyReferences.Add(node.InnerText);
				}
			}
			catch
			{
				throw new ConfigurationException("Invalid values found in Generator configuration.");
			}
            return options;
		}
	}
}
