//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// GeneratorHost.cs
// The hosting environment of all components in the XGoF application
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.Customization;
using NMatrix.Core.Xml;
using NMatrix.Core.Utility;
using NMatrix.Core.Configuration;

namespace NMatrix.XGoF.Host
{
	/// <summary>
	/// The hosting environment for the application.
	/// </summary>
	public sealed class GeneratorHost : IServiceProvider
	{
        internal ConfigurationRetriever Retriever = null;
		internal CustomizationManager Manager = new CustomizationManager();
		internal CurrentState State = new CurrentState();
		internal ExtenderSection Extender = null;
		internal GeneratorSection GeneratorOptions = null;
		internal XmlNamespaceManager DefaultNamespaceManager = null;
		internal XmlSchema CustomizationSchema = null;
		internal XmlSchema ConfigurationSchema = null;

		/// <summary>
		/// The XGoF namespace for schemas and customization files.
		/// </summary>
		public const string Namespace = "http://sourceforge.net/projects/dotnetopensrc/xgof";

		/// <summary>
		/// Singleton implementation to access an instance of this object.
		/// </summary>
		public static readonly GeneratorHost Instance = new GeneratorHost();

		/*
		/// <summary>
		/// Traditional singleton implementation private variable.
		/// </summary>
		private static GeneratorHost _instance;

		/// <summary>
		/// Traditional Singleton implementation.
		/// </summary>
		public static GeneratorHost Instance
		{
			get
			{
				if (_instance == null)
					_instance = new GeneratorHost();
				return _instance;
			}
		}
		*/

		internal void InitSchema()
		{
			DefaultNamespaceManager = new XmlNamespaceManager(new NameTable());
			DefaultNamespaceManager.AddNamespace("xgf", Namespace);

			//Validate customization if appropriate.
			if (GeneratorOptions.ValidateCustomizations == true)
			{
				Validator valid = new Validator();
				using (FileStream fs = new FileStream(IO.AppendLocalPath("CustomizationSchema.xsd"), FileMode.Open))
					CustomizationSchema = XmlSchema.Read(fs, new ValidationEventHandler(valid.OnValidation));

				if (valid.HasErrors) 
					throw new InvalidOperationException("Customization schema file isn't valid." + 
						Environment.NewLine + valid.Errors);

				DefaultNamespaceManager = new XmlNamespaceManager(new NameTable());
				XmlQualifiedName[] names = CustomizationSchema.Namespaces.ToArray();
				foreach (XmlQualifiedName ns in names)
				{
					if (ns.Name != "xgf") 
						DefaultNamespaceManager.AddNamespace(ns.Name, ns.Namespace);
					else if (ns.Namespace != GeneratorHost.Namespace)
						throw new InvalidOperationException("xgf namespace prefix is reserved for XGoF namespace.");
				}
			}
		}

		private GeneratorHost()
		{
		}

		/// <summary>
		/// Throws an exception with the received message.
		/// </summary>
		/// <param name="message"></param>
		public static void ThrowInvalidHostResponse(string message)
		{
			throw new ApplicationException("Generator Host didn't return the expected answer." +
				Environment.NewLine + message);
		}

		/// <summary>
		/// Throws an exception with the received type.
		/// Typically used when the host doesn't return an instance of the expected 
		/// service type.
		/// </summary>
		/// <param name="requestedType"></param>
		public static void ThrowInvalidHostResponse(Type requestedType)
		{
			ThrowInvalidHostResponse("Returned " + requestedType.Name + " is null.");
		}

		/// <summary>
		/// Returns an instance of the requested service type. May return null.
		/// </summary>
		/// <param name="serviceType"></param>
		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(ICustomizationManager))
				return Manager;
			else if (serviceType == typeof(IConfigurationRetriever))
				return Retriever;
			else if (serviceType == typeof(ICurrentState))
				return State;
			
			return null;
		}
	}
}
