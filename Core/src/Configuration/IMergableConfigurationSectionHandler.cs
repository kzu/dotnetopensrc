//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// IMergableConfigurationSectionHandler.cs
// A configuration section handler which can merge the configuration at the 
// library level with a custom configuration file.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Configuration;

namespace NMatrix.Core.Configuration
{
	/// <summary>
	/// Implement in configuration handler if merging is desired.
	/// </summary>
	public interface IMergableConfigurationSectionHandler : IConfigurationSectionHandler
	{
		/// <summary>
		/// Merges the passed configuration with the new parameters.
		/// </summary>
		/// <param name="currentConfig">The current configuration to use for merging.</param>
		/// <param name="parent">The parent node.</param>
		/// <param name="configContext">The current context for configuration. Usually <c>null</c>, 
		/// except in ASP.NET scenarios.</param>
		/// <param name="section">The node with the configuration data.</param>
		object Merge(object currentConfig, object parent, object configContext, XmlNode section);
	}
}
