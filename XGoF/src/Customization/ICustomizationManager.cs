//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ICustomizationManager.cs
// Interface used by extenders to ask for the customization manager instance.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;
using NMatrix.XGoF.XSD;
using NMatrix.XGoF.Customization;

namespace NMatrix.XGoF.Customization
{
	/// <summary>
	/// Interface used by extenders to ask for the customization manager instance.
	/// </summary>
	public interface ICustomizationManager
	{
		/// <summary>
		/// The list of configured customizations to run.
		/// </summary>
		ArrayList Files
		{
			get;
		}
		/// <summary>
		/// Retrieve customizations that apply to the passed element.
		/// </summary>
		/// <param name="element">Current element to use as filter.</param>
		/// <param name="type">Type of configuration nodes to retrieve (Colletion|Type).</param>
		/// <returns>A list of <c>XmlNode</c> with the matching nodes.</returns>
		ArrayList RetrieveCustomization(BaseSchemaTypedElement element, NodeType type);
		/// <summary>
		/// Retrieve customizations that apply to the passed element.
		/// </summary>
		/// <param name="element">Current element to use as filter.</param>
		/// <returns>A list of <c>XmlNode</c> with the matching nodes.</returns>
		ArrayList RetrieveCustomization(BaseSchemaTypedElement element);
	}
}
