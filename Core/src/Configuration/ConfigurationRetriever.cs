//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ConfigurationRetriever.cs
// Retrieves configurations associated with a custom configuration file. 
// Functionality similar to the ConfigurationSettings class, but for an specific
// .config file. Validates the configuration file prior to its use if a schema is 
// specified. Manages merging configurations too.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using NMatrix.Core.Xml;

namespace NMatrix.Core.Configuration
{
	/// <summary>
	/// The class responsible for loading transient configurations.
	/// </summary>
	public class ConfigurationRetriever : IConfigurationRetriever
	{
		/// <summary>
		/// The <see cref="XmlDocument"/> to use for configuration retrieval.
		/// </summary>
		private XmlDocument _file = null;
		private string _filename;
		private FileSystemWatcher _configwatcher = new FileSystemWatcher();
		// Default to an empty document for merging purposes.
		private XmlDocument _appsettings = new XmlDocument();
		private string _appfile;
		private FileSystemWatcher _appwatcher = new FileSystemWatcher();
		private Hashtable _config = new Hashtable();

		/// <summary>
		/// Gets or sets the file document to use for configuration retrieval.
		/// </summary>
		public XmlDocument ConfigurationDocument
		{
			get { return _file; }
			set 
			{ 
				lock(this)
				{
					ConfigurationLoader.ValidateSections(value, _appsettings);
					_config.Clear();
					_file = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the file name to use to load the current document.
		/// </summary>
		public string ConfigurationFile
		{
			get { return _filename; }
			set 
			{ 
				lock(this)
				{
					if (value != String.Empty)
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(value);
						ConfigurationDocument = doc;
					}
					else
					{
						_file = null;
						_config.Clear();
					}

					_filename = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the document that contains application-wide configurations.
		/// </summary>
		public XmlDocument AppSettingsDocument
		{
			get { return _appsettings; }
			set 
			{ 
				lock(this)
				{
					ConfigurationLoader.ValidateSections(value);
					_config.Clear();
					_appsettings = value; 
				}
			}
		}

		/// <summary>
		/// Gets or sets the document that contains application-wide configurations.
		/// </summary>
		public string AppSettingsFile
		{
			get { return _appfile; }
			set 
			{ 
				lock(this)
				{
					if (value != String.Empty)
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(value);
						AppSettingsDocument = doc;
					}
					else
					{
						AppSettingsDocument = new XmlDocument();
					}

					_appfile = value;
				}
			}
		}

		#region Overloaded Constructors
		/// <summary>
		/// Initializes the class with the received parameters.
		/// </summary>
		/// <param name="configFile">The configuration file to use.</param>
		public ConfigurationRetriever(string configFile) :
			this(configFile, null, null, null)
		{	
		}

		/// <summary>
		/// Initializes the class with the received parameters.
		/// </summary>
		/// <param name="configDocument">A loaded configuration file.</param>
		public ConfigurationRetriever(XmlDocument configDocument) :
			this(null, configDocument, null, null)
		{
		}

		/// <summary>
		/// Initializes the class with the received parameters.
		/// </summary>
		/// <param name="configFile"></param>
		/// <param name="appSettingsFile"></param>
		public ConfigurationRetriever(string configFile, string appSettingsFile) :
			this(configFile, null, appSettingsFile, null)
		{
		}

		/// <summary>
		/// Initializes the class with the received parameters.
		/// </summary>
		/// <param name="configFile"></param>
		/// <param name="appSettings"></param>
		public ConfigurationRetriever(string configFile, XmlDocument appSettings) :
			this(configFile, null, null, appSettings)
		{
		}

		/// <summary>
		/// Initializes the class with the received parameters.
		/// </summary>
		/// <param name="configDocument"></param>
		/// <param name="appSettings"></param>
		public ConfigurationRetriever(XmlDocument configDocument, XmlDocument appSettings) :
			this(null, configDocument, null, appSettings)
		{
		}
		#endregion

		/// <summary>
		/// Initialization contructor.
		/// </summary>
		/// <param name="configFile"></param>
		/// <param name="configDocument"></param>
		/// <param name="appSettingsFile"></param>
		/// <param name="appSettingsDocument"></param>
		private ConfigurationRetriever(string configFile, XmlDocument configDocument, 
			string appSettingsFile, XmlDocument appSettingsDocument)
		{
			InitWatchers();

			if (appSettingsFile != null && appSettingsFile != String.Empty)
				AppSettingsFile = appSettingsFile;
			else if (appSettingsDocument != null)
				AppSettingsDocument = appSettingsDocument;

			if (configFile != null && configFile != String.Empty)
				ConfigurationFile = configFile;
			else if (configDocument != null)
				ConfigurationDocument = configDocument;
		}

		private void InitWatchers()
		{
			AppDomain.CurrentDomain.DomainUnload += new EventHandler(OnUnload);
			_configwatcher.Changed += new FileSystemEventHandler(OnConfigChanged);
			_appwatcher.Changed += new FileSystemEventHandler(OnAppSettingsChanged);
		}

		/// <summary>
		/// Reinitializes the object for easy reuse.
		/// </summary>
		/// <param name="fileName">The new file to use.</param>
		/// <remarks>The same effect as setting the ConfigurationFile property.</remarks>
		public void Initialize(string fileName)
		{
			ConfigurationFile = fileName;
		}

		/// <summary>
		/// Retrieve the configuration associated with a section name.
		/// </summary>
		/// <param name="sectionName">The section to retrieve.</param>
		/// <returns>The object returned by the section handler.</returns>
		/// <remarks>If the section handler implements <c>IMergableConfigurationSectionHandler</c>,
		/// the Merge method of this interface is called, passing the existing configuration 
		/// in the ApplicationSettings document.</remarks>
		public object GetConfig(string sectionName)
		{
			if (_file == null) 
				throw new InvalidOperationException("Configuration hasn't been initialized.");

			object result = _config[sectionName];
			if (result != null) return result;

			IConfigurationSectionHandler handler;
			handler = ConfigurationLoader.GetSectionHandler(sectionName, _file);
			// If the handler isn't found in the current config, look for the AppSettings document.
			if (handler == null)
				handler = ConfigurationLoader.GetSectionHandler(sectionName, _appsettings);

			if (handler == null)
				throw new ConfigurationException("Section " + sectionName + " doesn't have a valid handler.");

			// Handle specially the mergable handlers.
			if (handler is IMergableConfigurationSectionHandler)
			{
				// Retrieve the global application values first.
				object current = handler.Create(
					ConfigurationLoader.GetRootNode(_appsettings),
					null, ConfigurationLoader.GetNode(sectionName, _appsettings)); 
				// Merge the results.
				result = ((IMergableConfigurationSectionHandler)handler).Merge(
					current, ConfigurationLoader.GetRootNode(_file), 
					null, ConfigurationLoader.GetNode(sectionName, _file));
			}
			// Handle specially the .NET default handlers.
			else if (handler is NameValueSectionHandler || handler is NameValueFileSectionHandler)
			{
				result = InitNameValue(ConfigurationLoader.GetNode(sectionName, _file));
			}
			else if (handler is DictionarySectionHandler)
			{
				result = InitDictionary(ConfigurationLoader.GetNode(sectionName, _file));
				// Default handling of IConfigurationSectionHandler handlers.
			}
			else
			{
				result = handler.Create(ConfigurationLoader.GetRootNode(_file), 
					null, ConfigurationLoader.GetNode(sectionName, _file));
			}

			// Replace the existing cache if it exists.
			lock(this)
			{
				if (_config.ContainsKey(sectionName)) _config.Remove(sectionName);
				// Cache the loaded config for later use.
				_config.Add(sectionName, result);
			}
			return result;
		}

		/// <summary>
		/// Initializes a collection of values with the xml data in the section received.
		/// </summary>
		/// <param name="section">The <see cref="XmlNode"/> object with the data.</param>
		/// <returns>The loaded collection of values.</returns>
		public static NameValueCollection InitNameValue(XmlNode section)
		{
			NameValueCollection config = new NameValueCollection(section.ChildNodes.Count);
			foreach (XmlNode node in section.ChildNodes)
				config.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
			return config;
		}

		/// <summary>
		/// Initializes a collection of values with the xml data in the section received.
		/// </summary>
		/// <param name="section">The <see cref="XmlNode"/> object with the data.</param>
		/// <returns>The loaded collection of values.</returns>
		public static IDictionary InitDictionary(XmlNode section)
		{
			Hashtable config = new Hashtable(section.ChildNodes.Count);
			foreach (XmlNode node in section.ChildNodes)
				config.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
			return config;
		}

		/// <summary>
		/// Reload the configuration XmlDocument when changes happen.
		/// </summary>
        private void OnConfigChanged(object sender, FileSystemEventArgs e)
		{
			ConfigurationFile = e.FullPath;
		}

		/// <summary>
		/// Reload the appsettings XmlDocument when changes happen.
		/// </summary>
        private void OnAppSettingsChanged(object sender, FileSystemEventArgs e)
		{
			AppSettingsFile = e.FullPath;
		}

		/// <summary>
		/// Unregisters monitors when an assembly is unloaded.
		/// </summary>
		private void OnUnload(object sender, EventArgs e)
		{
			lock(this)
			{
				_configwatcher.Changed -= new FileSystemEventHandler(OnConfigChanged);
				_appwatcher.Changed -= new FileSystemEventHandler(OnAppSettingsChanged);
				_configwatcher.Dispose();
				_appwatcher.Dispose();
			}
		}
	}
}
