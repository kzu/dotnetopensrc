//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// RunnerConfigurationSectionHandler.cs
// The class responsible for initializing the <Runner> section in the configuration
// file.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Collections;
using System.Configuration;
using NMatrix.Core.Collections;
using NMatrix.Core.Configuration;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Administers Runner sections in the configuration file.
	/// </summary>
	public class RunnerSectionHandler : IMergeConfigurationSectionHandler
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public RunnerSectionHandler()
		{
		}

		/// <summary>
		/// Creates and initializes the configuration options.
		/// </summary>
		/// <param name="parent">Parent configuration node, the root configuration.</param>
		/// <param name="configContext">Not used.</param>
		/// <param name="configNode">The configuration node.</param>
		/// <returns>A <c>RunnerSection</c> object.</returns>
		public object Create(object parent, object configContext, XmlNode configNode)
		{
			if (configNode == null) return null;
			RunnerSection runner = new RunnerSection();
			XmlNodeList nodes = configNode.SelectNodes("customizations/customization[not(boolean(@enabled)) or @enabled=\"true\"]");

			try
			{
				ProcessCustomizations(nodes, runner.Customizations);

				nodes = configNode.SelectNodes("sources");				
				RunnerSource sch;
				foreach (XmlNode node in nodes)
				{
					sch = new RunnerSource(node.Attributes["file"].Value);
                    ProcessCustomizations(node.SelectNodes("customizations/customization[not(boolean(@enabled)) or @enabled=\"true\"]"), sch.Customizations);
                    runner.Sources.Add(sch.FileName, sch);
				}
			}
			catch (Exception ex)
			{
				throw new ConfigurationException("Invalid values found in Runner configuration.", ex);
			}
            return runner;
		}

		/// <summary>
		/// Merges an existing RunnerSection object with the new data.
		/// </summary>
		public object Merge(object currentConfig, object parent, object configContext, XmlNode configNode)
		{
			RunnerSection runner = currentConfig as RunnerSection;
			RunnerSection merger;
			if (runner != null)
				merger = runner.Clone() as RunnerSection;
			else
				merger = new RunnerSection();

			if (runner == null && configNode == null) return null;
			if (configNode == null) return merger;

			XmlNodeList nodes = configNode.SelectNodes("customizations/customization[not(boolean(@enabled)) or @enabled=\"true\"]");

			try
			{
				ProcessCustomizations(nodes, merger.Customizations);

				nodes = configNode.SelectNodes("sources/source");
				RunnerSource sch;
				foreach (XmlNode node in nodes)
				{
					sch = new RunnerSource(node.Attributes["file"].Value);
                    ProcessCustomizations(node.SelectNodes("customizations/customization[not(boolean(@enabled)) or @enabled=\"true\"]"), sch.Customizations);
                    merger.Sources.Add(sch.FileName, sch);
				}
			}
			catch (Exception ex)
			{
				throw new ConfigurationException("Invalid values found in Runner configuration.", ex);
			}
            return merger;
		}

		/// <summary>
		/// Loads customization elements to the passed <c>SortedList</c>
		/// </summary>
		/// <param name="nodes">The customization nodes.</param>
		/// <param name="customizations">The collection to add elements to.</param>
		private void ProcessCustomizations(XmlNodeList nodes, SortedList customizations)
		{
			RunnerCustomization row;
			XmlAttribute attr;
			foreach (XmlNode node in nodes)
			{
				attr = node.Attributes["runOrder"];
				int order = 0;
				if (attr != null)
					order = int.Parse(attr.Value);
				if (order == -1) order = int.MaxValue;
                row = new RunnerCustomization(node.Attributes["file"].Value, order);
				customizations.Add(new SortedDuplicateKey(order), row);
			}
		}
	}
}
