//===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// BaseCodeVisitor.cs
// Contains the base used by visitor implementations.
// Provides basic handling and recovering of the current processing 
// Namespace and Type, for use by inheritors.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.CodeDom;
using System.Xml;
using System.Xml.Schema;
using NMatrix.XGoF.Host;
using NMatrix.XGoF.Configuration;
using NMatrix.XGoF.Customization;
using NMatrix.XGoF.XSD;
using NMatrix.Core;
using NMatrix.Core.Configuration;
using NMatrix.Core.ComponentModel;
using NMatrix.Core.Patterns;

namespace NMatrix.XGoF.Visitors
{
	/// <summary>
	/// The base class of all the visitors.
	/// </summary>
	public abstract class BaseCodeVisitor : HostedVisitor, INotifier
	{
		/// <summary>
		/// The current unit of code being generated.
		/// </summary>
		protected CodeCompileUnit Unit;
		/// <summary>
		/// The namespace in use, according to the source file.
		/// </summary>
		protected CodeNamespace CurrentNamespace;
		/// <summary>
		/// The class type declaration of the current element.
		/// </summary>
		protected CodeTypeDeclaration CurrentType;
		/// <summary>
		/// The global generator options.
		/// </summary>
		protected GeneratorSection Configuration;
		/// <summary>
		/// The active retriever to perform queries against customization schemas.
		/// </summary>
		protected ICustomizationManager Retriever;

		/// <summary>
		/// Notify listeners about progress in lengthy processes.
		/// </summary>
		public event ProgressEventHandler Progress;

		/// <summary>
		/// Keeps a reference to the current schema in XmlDocument format.
		/// </summary>
		protected XmlDocument CurrentSchemaDom;

		/// <summary>
		/// Keeps a reference to the current schema in XmlDocument format.
		/// </summary>
		protected XmlSchema CurrentSchema;

	 	/// <summary>
		/// Initializes the component passing the environment host variable.
		/// </summary>
		/// <param name="environment">The host reference</param>
		public override void Initialize(IServiceProvider environment)
		{
			base.Initialize(environment);
            ICurrentState state;
			state = Host.GetService(typeof(ICurrentState)) as ICurrentState;
			if (state == null) GeneratorHost.ThrowInvalidHostResponse(typeof(CurrentState));

			Unit = state.Unit;

			// Retrieve the current generator configuration.
			object service = Host.GetService(typeof(IConfigurationRetriever));
			if (service == null) GeneratorHost.ThrowInvalidHostResponse(typeof(IConfigurationRetriever));
			Configuration = ((IConfigurationRetriever)service).GetConfig("generator") as GeneratorSection;

			// Retrieve the current ICustomizationManager instance.
			service = Host.GetService(typeof(ICustomizationManager));
			if (service == null) GeneratorHost.ThrowInvalidHostResponse(typeof(ICustomizationManager));
			Retriever = (ICustomizationManager)service;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public BaseCodeVisitor()
		{
		}

		/// <summary>
		/// Visitor implementation. Processes the passed element 
		/// repositioning the CurrentType member variable.
		/// </summary>
		/// <param name="element"></param>
		public virtual void Visit(VisitableElementComplexType element)
		{
			CurrentType = null;
			foreach (CodeTypeDeclaration type in CurrentNamespace.Types)
				if (type.Name == element.TypeName)
				{
					CurrentType = type;
					break;
				}
		}

		/// <summary>
		/// Visitor implementation. Processes the passed element 
		/// repositioning the CurrentNamespace and CurrentSchema member variables.
		/// </summary>
		/// <param name="element"></param>
		public virtual void Visit(VisitableSchemaRoot element)
		{
			if (element.SchemaObject.TargetNamespace == null ||
				element.SchemaObject.TargetNamespace.Length == 0 ||
				element.SchemaObject.TargetNamespace == String.Empty)
			{
				throw new InvalidOperationException("Schema " + element.SchemaObject.SourceUri + " must have a TargetNamespace attribute.");
			}
			CurrentNamespace = null;
			CurrentSchema = element.SchemaObject;
			CurrentSchemaDom = element.SchemaDocument;

			foreach (CodeNamespace ns in Unit.Namespaces)
			{
				if (ns.Name == element.SchemaObject.TargetNamespace)
				{
					CurrentNamespace = ns;
					break;
				}
			}
		}

		/// <summary>
		/// The method called for notifications.
		/// </summary>
		/// <param name="message">The message to use.</param>
		protected void OnProgress(string message)
		{
			if (Progress != null)
				Progress(this, new ProgressEventArgs(message));
		}
	}
}
