//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// GeneratorSection.cs
// Maintains configuration settings.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.Collections;
using System.CodeDom.Compiler;

namespace NMatrix.XGoF.Configuration
{
	/// <summary>
	/// Defines different types of enumeration.
	/// </summary>
	public enum IterationType 
	{ 
		/// <summary>
		/// Creates a class for each element whose type is not a 
		/// simple type or an XSD intrinsic data type.
		/// </summary>
		ComplexElement, 
		/// <summary>
		/// Default. Generates a class for each complex type found.
		/// </summary>
		ComplexType 
	}

	internal struct OptionsData
	{
		public bool GenerateContainerProperty;
		public IterationType Iteration;
		public CodeDomProvider Provider;
		public string CollectionNaming;
		public string TypeNaming;
		public string TargetAssembly;
		public string TargetFolder;
		public string TargetNamespace;
		public bool OutputSource;
		public bool OutputCompiled;
		public ArrayList NamespaceImports;
		public ArrayList AssemblyReferences;
		public bool CreateNamespaceFolders;
		public bool ValidateCustomizations;
	}

	/// <summary>
	/// Maintains configuration settings.
	/// </summary>
	public class GeneratorSection : ICloneable
	{
		internal OptionsData InnerData;

		/// <summary>
		/// Initializes and clones the ArrayList members;
		/// </summary>
		/// <param name="data">The object to use as source of the cloning.</param>
		internal GeneratorSection(OptionsData data)
		{
			InnerData = data;
			InnerData.AssemblyReferences = new ArrayList(data.AssemblyReferences);
			InnerData.NamespaceImports = new ArrayList(data.NamespaceImports);
		}

		/// <summary>
		/// Initializes the default values.
		/// </summary>
		internal GeneratorSection()
		{
			InnerData.AssemblyReferences = new ArrayList();
			InnerData.CollectionNaming = "s";
			InnerData.CreateNamespaceFolders = true;			
			InnerData.GenerateContainerProperty = false;
			InnerData.Iteration = IterationType.ComplexType;
			InnerData.NamespaceImports = new ArrayList();
			InnerData.OutputCompiled = true;
			InnerData.OutputSource = false;
			InnerData.Provider = new Microsoft.CSharp.CSharpCodeProvider();
			InnerData.TargetAssembly = "NMatrix.Generated.dll";
			InnerData.TargetFolder = String.Empty;
			InnerData.TargetNamespace = String.Empty;
			InnerData.TypeNaming = String.Empty;
			InnerData.ValidateCustomizations = true;
		}

		/// <summary>
		/// Clones the current instance.
		/// </summary>
		/// <returns>The cloned instance.</returns>
		public object Clone()
		{
			return new GeneratorSection(InnerData);
		}

		/// <summary>
		/// The type of iteration for traversing the schemas.
		/// </summary>
		public IterationType Iteration
		{
			get { return InnerData.Iteration; }
		}

		/// <summary>
		/// Provider for code generation.
		/// </summary>
		/// <remarks>Default provider is C#.</remarks>
		public CodeDomProvider Provider
		{
			get { return InnerData.Provider; }
		}

		/// <summary>
		/// The suffix to append to collections.
		/// </summary>
		/// <remarks>By default, appends an "s" to the type.</remarks>
		public string CollectionNaming
		{
			get { return InnerData.CollectionNaming; }
		}

		/// <summary>
		/// Specifies any string to append to class names.
		/// </summary>
		public string TypeNaming
		{
			get { return InnerData.TypeNaming; }
		}

		/// <summary>
		/// The assembly to generate if OutputCompiled is selected.
		/// </summary>
		public string TargetAssembly
		{
			get { return InnerData.TargetAssembly; }
		}

		/// <summary>
		/// The folder to put assembly and/or source in.
		/// </summary>
		public string TargetFolder
		{
			get { return InnerData.TargetFolder; }
		}

		/// <summary>
		/// A namespace to put classes in. 
		/// </summary>
		/// <remarks>If this value is not set and the source files are 
		/// XmlSchema files, the targetnamespace of the schema is used.</remarks>
		public string TargetNamespace
		{
			get { return InnerData.TargetNamespace; }
		}

		/// <summary>
		/// Generate output source code?
		/// </summary>
		public bool OutputSource
		{
			get { return InnerData.OutputSource; }
		}

		/// <summary>
		/// Generate compiled output?
		/// </summary>
		public bool OutputCompiled
		{
			get { return InnerData.OutputCompiled; }
		}
            
		/// <summary>
		/// Imports at the schema/namespace level.
		/// </summary>
		public ArrayList NamespaceImports
		{
			get { return InnerData.NamespaceImports; }
		}

		/// <summary>
		/// References for compiled output generation.
		/// </summary>
		public ArrayList AssemblyReferences
		{
			get { return InnerData.AssemblyReferences; }
		}

		/// <summary>
		/// Defines if a subfolder is created for each Namespace.
		/// </summary>
		public bool CreateNamespaceFolders
		{
			get { return InnerData.CreateNamespaceFolders; }
		}

		/// <summary>
		/// Defines if the default customization retriever should validate XML 
		/// customization files against the default customization schema.
		/// </summary>
		public bool ValidateCustomizations
		{
			get { return InnerData.ValidateCustomizations; }
		}
	}
}
