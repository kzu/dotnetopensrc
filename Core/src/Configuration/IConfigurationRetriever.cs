//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// IConfigurationRetriever.cs
// This interface is used by hosted components to query for an instance of 
// the ConfigurationRetriever, which allows extenders to retrieve configurations
// in the .config file.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;

namespace NMatrix.Core.Configuration
{
	/// <summary>
	/// Retrieves configurations in the .config file.
	/// </summary>
	public interface IConfigurationRetriever
	{
		/// <summary>
		/// Retrieve the configuration associated with a section name.
		/// </summary>
		/// <param name="sectionName">The section to retrieve.</param>
		/// <returns>The object returned by the section handler.</returns>
		object GetConfig(string sectionName);
	}
}
