//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// CustomizationManager.cs
// Retrieves customization information from all the files, accumulating results
// obtained by each ICustomizationRetriever defined in the configuration file.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Xml;
using System.Collections;
using NMatrix.XGoF.Host;
using NMatrix.XGoF.XSD;
using NMatrix.XGoF.Customization;
using NMatrix.XGoF.Configuration;
using NMatrix.Core.Xml;

namespace NMatrix.XGoF.Customization
{
	/// <summary>
	/// Retrieves customization information from all the files set in the Initialize method.
	/// </summary>
	/// <remarks>This class iterates though all the configured <c>ICustomizationRetriever</c>
	/// objects and accumulates their results.</remarks>
	internal class CustomizationManager : ICustomizationManager
	{
		private ArrayList _files;
		private ArrayList _retrievers;

		/// <summary>
		/// Default empty constructor.
		/// </summary>
		public CustomizationManager()
		{
		}

		/// <summary>
		/// Provides access to the list of files currently used to retrieve customizations.
		/// </summary>
		public ArrayList Files
		{
			get { return _files; }
		}

		/// <summary>
		/// Initializes the class with the list of files to use for customization retrieval.
		/// </summary>
		/// <param name="files">List of <c>XmlDocument</c> objects to use.</param>
		/// <remarks>Validates each customization file against the corresponding customization schema.</remarks>
		public void Initialize(ArrayList files)
		{
			Validator valid = new Validator();
			if (GeneratorHost.Instance.GeneratorOptions.ValidateCustomizations)
				foreach (XmlDocument file in files)
					valid.Validate(file, GeneratorHost.Instance.CustomizationSchema);

			_files = files;	
		}

		/// <summary>
		/// List of customization retrievers, loaded from the configuration file.
		/// </summary>
		private ArrayList Retrievers
		{
			get 
			{
				if (_retrievers == null)
				{
					ExtenderSection section = (ExtenderSection)
						GeneratorHost.Instance.Retriever.GetConfig("extender");
					if (section == null) GeneratorHost.ThrowInvalidHostResponse(typeof(ExtenderSection));
                    _retrievers = section.Retrievers;
				}
				return _retrievers;
			}
		}

		/// <summary>
		/// Retrieve customizations that apply to the passed element.
		/// </summary>
		/// <param name="element">Current element to use as filter.</param>
		/// <param name="type">Type of configuration nodes to retrieve (Colletion|Type).</param>
		/// <returns>A list of <c>XmlNode</c> with the matching nodes.</returns>
		public ArrayList RetrieveCustomization(BaseSchemaTypedElement element, NodeType type)
		{
			ArrayList result = new ArrayList();			
			foreach (ICustomizationRetriever retriever in Retrievers)
			{
				ArrayList nodes = retriever.RetrieveCustomization(element, type, _files);
				foreach (object node in nodes)
					result.Add(node);
			}

			return result;
		}

		/// <summary>
		/// Retrieve customizations that apply to the passed element.
		/// </summary>
		/// <param name="element">Current element to use as filter.</param>
		/// <returns>A list of <c>XmlNode</c> with the matching nodes.</returns>
		public ArrayList RetrieveCustomization(BaseSchemaTypedElement element)
		{
			ArrayList result = new ArrayList();			
			foreach (ICustomizationRetriever retriever in Retrievers)
			{
				ArrayList nodes = retriever.RetrieveCustomization(element, _files);
				foreach (object node in nodes)
					result.Add(node);
			}

			return result;
		}
	}
}
