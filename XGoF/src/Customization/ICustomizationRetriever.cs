//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ICustomizationRetriever.cs
// Interface to implement by extenders which want to handle customizations retrieval.
// Implemented by the CustomizationManager for configuration retrieval.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;
using NMatrix.XGoF.XSD;
using NMatrix.Core.ComponentModel;

namespace NMatrix.XGoF.Customization
{
	/// <summary>
	/// Retrieves customization for typed schema elements.
	/// </summary>
	public interface ICustomizationRetriever : IHostedComponent
	{
		/// <summary>
		/// Retrieves the customization nodes that apply to the specified element.
		/// </summary>
		/// <param name="element">The element to use as a filter.</param>
		/// <param name="type">The type of node to look for (Type or Collection)</param>
		/// <param name="files">The list of XmlDocuments to use for retrieval.</param>
		/// <returns>An <c>ArrayList</c> of matching XmlNodes.</returns>
		ArrayList RetrieveCustomization(BaseSchemaTypedElement element, NodeType type, ArrayList files);
		/// <summary>
		/// Retrieves the customization nodes that apply to the specified element.
		/// </summary>
		/// <param name="element">The element to use as a filter.</param>
		/// <param name="files">The list of XmlDocuments to use for retrieval.</param>
		/// <returns>An <c>ArrayList</c> of matching XmlNodes.</returns>
		ArrayList RetrieveCustomization(BaseSchemaTypedElement element, ArrayList files);
	}
}
