 //===============================================================================
// NMatrix XGoF Generator.
// http://sourceforge.net/projects/dotnetopensrc/
//
// Runner.cs
// Entry point for the generator. 
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using NMatrix.XGoF.Host;
using NMatrix.XGoF.Visitors;
using NMatrix.XGoF.Configuration;
using NMatrix.Core;
using NMatrix.Core.Patterns;
using NMatrix.Core.Configuration;
using NMatrix.Core.ComponentModel;

namespace NMatrix.XGoF
{
	/// <summary>
	/// Class responsible for running generations.
	/// </summary>
	public class Runner : INotifier
	{
		/// <summary>
		/// Notify listeners of a progress in the process.
		/// </summary>
		public event ProgressEventHandler Progress;
		/// <summary>
		/// Notify listeners that a input source file has been processed.
		/// </summary>
		public event ProgressEventHandler FileFinished;

		private string _config;

		/// <summary>
		/// Initialize the process with the specified file.
		/// </summary>
		public Runner(string fileName)
		{
			_config = fileName;
		}

		/// <summary>
		/// Starts processing the file passed in the constructor.
		/// </summary>
		public void Start()
		{
			CodeCompileUnit unit;

			#region Initialize GeneratorHost variables
			GeneratorHost.Instance.Retriever = new ConfigurationRetriever(
				_config, ConfigurationSettings.AppPathConfig);

			ExtenderSection extender = (ExtenderSection)
				GeneratorHost.Instance.Retriever.GetConfig("extender");
			if (extender == null) GeneratorHost.ThrowInvalidHostResponse("Invalid <extender> section.");
			GeneratorHost.Instance.Extender = extender;

			GeneratorSection options = (GeneratorSection)
				GeneratorHost.Instance.Retriever.GetConfig("generator");
			if (options == null) GeneratorHost.ThrowInvalidHostResponse("Invalid <generator> section.");
			GeneratorHost.Instance.GeneratorOptions = options;
			// If the configured traverser implements INotifier, append the progress event handler.
			if (GeneratorHost.Instance.Extender.Traverser is INotifier)
				((INotifier)GeneratorHost.Instance.Extender.Traverser).Progress += 
					new ProgressEventHandler(OnBubbleProgress);

			GeneratorHost.Instance.InitSchema();
			#endregion

			RunnerSection section = (RunnerSection)
				GeneratorHost.Instance.Retriever.GetConfig("runner");

			// Load the ordered list of files and run the process.
			foreach (DictionaryEntry sch in section.Sources)
			{
				RunnerSource source = sch.Value as RunnerSource;
				// Reinitialize the current unit.

				#region Order the customizations in a sorted list.
				SortedList indexes = new SortedList();
				ArrayList files = new ArrayList();
                foreach (DictionaryEntry entry in section.Customizations)
				{
					RunnerCustomization cust = entry.Value as RunnerCustomization;
					files = indexes[cust.RunOrder] as ArrayList;
					if (files == null)
					{
						files = new ArrayList();
						indexes.Add(cust.RunOrder, files);
					}
					files.Add(cust.File);
				}

				foreach (DictionaryEntry entry in source.Customizations)
				{
					RunnerCustomization cust = entry.Value as RunnerCustomization;
					files = indexes[cust.RunOrder] as ArrayList;
					if (files == null)
					{
						files = new ArrayList();
						indexes.Add(cust.RunOrder, files);
					}
					files.Add(cust.File);
				}
				#endregion

				#region Execute the ordered entries
				foreach (DictionaryEntry entry in indexes)
				{
					unit = new CodeCompileUnit();
					GeneratorHost.Instance.Manager.Initialize(entry.Value as ArrayList);
					GeneratorHost.Instance.State.Init(source, unit);
					RunSource(source);
					GenerateOutput(unit, source);
				}
				#endregion

				// If no customizations are defined, run the process anyway (empty structure).
				if (indexes.Count == 0)
				{
					unit = new CodeCompileUnit();
					GeneratorHost.Instance.Manager.Initialize(new ArrayList());
					GeneratorHost.Instance.State.Init(source, unit);
					RunSource(source);
					GenerateOutput(unit, source);
				}

				OnProgress("Finished!");
			}			
		}

		/// <summary>
		/// Runs all the subscribed visitors against the visitable tree 
		/// parsed by the ITraverser implementation.
		/// </summary>
		/// <param name="source">The <c>RunnerSource</c> object to process.</param>
		private void RunSource(RunnerSource source)
		{			
			GeneratorHost.Instance.Extender.Traverser.Initialize(GeneratorHost.Instance);				

			// Build the result tree to use.
			IVisitable result = 
				GeneratorHost.Instance.Extender.Traverser.Traverse(source.FileName);

			foreach (DictionaryEntry entry in GeneratorHost.Instance.Extender.Visitors)
			{
				IVisitor visitor = entry.Value as IVisitor;

				// If the element implements IHostedComponent, initialize it 
				// with the GeneratorHost instance.
				if (visitor is IHostedComponent)
					((IHostedComponent)visitor).Initialize(GeneratorHost.Instance);
				// If the element implements INotifier, append the progress event handler.
				if (visitor is INotifier)
				{
					// Remove it just in case it was already subscribed.
					((INotifier)visitor).Progress -= new ProgressEventHandler(OnBubbleProgress);
					((INotifier)visitor).Progress += new ProgressEventHandler(OnBubbleProgress);
				}

				result.Accept(visitor);
			}            
		}

		/// <summary>
		/// Compiles the generated code in the output assembly.
		/// </summary>
		/// <param name="unit">The code graph to compile.</param>
		/// <param name="source">The current source in use.</param>
		private void GenerateOutput(CodeCompileUnit unit, RunnerSource source)
		{
			if (GeneratorHost.Instance.GeneratorOptions.OutputSource)
			{
				OnProgress("Generating source code output ... ");
				ICodeGenerator gen = 
					GeneratorHost.Instance.GeneratorOptions.Provider.CreateGenerator();
				CodeGeneratorOptions opt = new CodeGeneratorOptions();
				opt.BracingStyle = "C";
				
				string ns = GeneratorHost.Instance.GeneratorOptions.TargetNamespace;
				string path = "";
				
				// If a namespace isn't defined and the source file is an XmlSchema,
				// we use the targetnamespace of the schema as the namespace for the classes.
				if (ns == String.Empty)
				{
					XmlSchema sch = null;
					try
					{
						using (FileStream fs = new FileStream(source.FileName, FileMode.Open))
							sch = XmlSchema.Read(fs, null);
					}
					catch { }
					
					if (sch != null) ns = sch.TargetNamespace;
				}
	
				// Review folder organization.
				if (GeneratorHost.Instance.GeneratorOptions.CreateNamespaceFolders)
				{
					// Replace target namespace dots with slashes
					path = GeneratorHost.Instance.GeneratorOptions.TargetFolder + 
						Path.DirectorySeparatorChar + ns.Replace('.',
						Path.DirectorySeparatorChar);
				}
				else if (GeneratorHost.Instance.GeneratorOptions.TargetFolder != String.Empty)
				{
					path = GeneratorHost.Instance.GeneratorOptions.TargetFolder;
				}
				else
				{
					path = Path.GetDirectoryName(source.FileName);
				}

				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
				
				path += Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(source.FileName);
				
				if (GeneratorHost.Instance.GeneratorOptions.Provider is Microsoft.CSharp.CSharpCodeProvider)
					path += ".cs";
				else if (GeneratorHost.Instance.GeneratorOptions.Provider is Microsoft.VisualBasic.VBCodeProvider)
					path += ".vb";
				else
					// Dummy extension for other unknown languages.
					path += ".code";

				// Write source code to the output file.
				using (FileStream fs = new FileStream(path , FileMode.Create))
				{
                    OnProgress("Writing " + path + " source file ... ");					
					StreamWriter w = new StreamWriter(fs);
					gen.GenerateCodeFromCompileUnit(unit, w, opt);
					w.Flush();
				}

				OnFileFinished(path);
			}

			if (GeneratorHost.Instance.GeneratorOptions.OutputCompiled)
			{
				OnProgress("Generating compiled output ... ");
				ICodeCompiler comp = GeneratorHost.Instance.GeneratorOptions.Provider.CreateCompiler();
				CompilerParameters param = new CompilerParameters();
				param.TreatWarningsAsErrors = false;

				param.OutputAssembly = 
					GeneratorHost.Instance.GeneratorOptions.TargetFolder +
					Path.DirectorySeparatorChar + 
					GeneratorHost.Instance.GeneratorOptions.TargetAssembly;
				
				// Add referenced assemblies.
				string[] assemblies = new string[GeneratorHost.Instance.GeneratorOptions.AssemblyReferences.Count];
				GeneratorHost.Instance.GeneratorOptions.AssemblyReferences.CopyTo(assemblies);
				param.ReferencedAssemblies.AddRange(assemblies);

				CompilerResults results = comp.CompileAssemblyFromDom(param, unit);
				
				StringWriter w = new StringWriter();
				foreach (string msg in results.Output)
					Console.WriteLine(msg);

				if (results.Errors.Count != 0) 
				{
					foreach (CompilerError error in results.Errors)
						w.WriteLine(error);
					throw new InvalidOperationException(w.ToString());
				}
			}			
		}

		/// <summary>
		/// Bubble an inner progress event notification revieved from a plug-in.
		/// </summary>
		private void OnBubbleProgress(object sender, ProgressEventArgs e)
		{
			if (Progress != null)
				Progress(sender, e);
		}

		/// <summary>
		/// The method called for notifications.
		/// </summary>
		/// <param name="message">The message to use.</param>
		private void OnProgress(string message)
		{
			if (Progress != null)
				Progress(this, new ProgressEventArgs(message));
		}

		/// <summary>
		/// The method called each time a single file has been generated.
		/// </summary>
		/// <param name="path">The path to the file.</param>
		/// <remarks>Would it be better to have a separate argument class?</remarks>
		private void OnFileFinished(string path)
		{
			if (FileFinished != null)
				FileFinished(this, new ProgressEventArgs(path));
		}
	}
}
