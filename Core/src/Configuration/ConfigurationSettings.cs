//===============================================================================
// NMatrix Core.
// http://sourceforge.net/projects/dotnetopensrc/
//
// ConfigurationSettings.cs
// Provides configuration management for the class library generator, as .NET 
// Framework configuration isn't available to these project types.
// It is a facade in front of the ConfigurationRetriever.
// 
// Author: Daniel Cazzulino
//===============================================================================

using System;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;

namespace NMatrix.Core.Configuration
{
	/// <summary>
	/// Provides access to library configurations.
	/// </summary>
	public sealed class ConfigurationSettings
	{
		static Hashtable _watchers = new Hashtable();

		private ConfigurationSettings()
		{
		}

		/// <summary>
		/// Retrieves a configuration from the specified section.
		/// </summary>
		/// <param name="sectionName">The section to use.</param>
		/// <returns>The configuration object.</returns>
		public static object GetConfig(string sectionName)
		{
			ConfigurationRetriever retriever = (ConfigurationRetriever)
				AppDomain.CurrentDomain.GetData(AppKey + "ConfigurationRetriever");

			if (retriever == null)
			{
				retriever = new ConfigurationRetriever(ConfigurationDocument);
				AppDomain.CurrentDomain.SetData(AppKey + "ConfigurationRetriever", retriever);
			}
			return retriever.GetConfig(sectionName);
		}

		/// <summary>
		/// Provides access to the "appSettings" section in the library configuration file.
		/// </summary>
		/// <remarks>We don't use GetConfig because appSettings section doesn't need a 
		/// handler explicitly declared, as it is always a NameValueSectionHandler.</remarks>
		public static NameValueCollection AppSettings
		{
			get 
			{
				NameValueCollection file = (NameValueCollection)
					AppDomain.CurrentDomain.GetData(AppKey + "appSettings");

				if (file == null)
				{
					file = ConfigurationRetriever.InitNameValue(
						ConfigurationLoader.GetNode("appSettings", ConfigurationDocument));
					AppDomain.CurrentDomain.SetData(AppKey + "appSettings", file);
				}
				return file;
			}
		}

		/// <summary>
		/// A key to use to save config information in the AppDomain.
		/// </summary>
		private static string AppKey
		{
			get { return Assembly.GetCallingAssembly().FullName; }
		}

		/// <summary>
		/// Returns the application configuration file path.
		/// </summary>
		public static string AppPathConfig
		{
			get { return Assembly.GetCallingAssembly().Location + ".config"; }
		}

		/// <summary>
		/// Retrieves the configuration XmlDocument.
		/// </summary>
		/// <remarks>If the document isn't found in the current domain associated data, it is 
		/// loaded and saved to it.</remarks>
		public static XmlDocument ConfigurationDocument
		{
			get
			{
				XmlDocument doc = (XmlDocument)
					AppDomain.CurrentDomain.GetData(AppKey + "ConfigurationDocument");

				if (doc == null)
				{
					doc = new XmlDocument();
					doc.Load(AppPathConfig);
					AppDomain.CurrentDomain.SetData(AppKey + "ConfigurationDocument", doc);
					AddMonitor(); 
				}
                return doc;
			}
		}

		/// <summary>
		/// Begins monitoring the configuration file for the executing assembly.
		/// </summary>
		private static void AddMonitor()
		{
			AppDomain.CurrentDomain.DomainUnload += new EventHandler(OnUnload);
			string file = AppPathConfig;
			if (!_watchers.ContainsKey(file))
			{
				FileSystemWatcher watcher = 
					new FileSystemWatcher(Path.GetDirectoryName(file), file);
				watcher.Changed += new FileSystemEventHandler(OnChanged);
				_watchers.Add(file, watcher);
			}	
		}

		/// <summary>
		/// Unloads the configuration XmlDocument when the file changes.
		/// </summary>
        private static void OnChanged(object sender, FileSystemEventArgs e)
		{
			AppDomain.CurrentDomain.SetData(AppKey + "ConfigurationDocument", null);
		}

		/// <summary>
		/// Unregisters monitors when an assembly is unloaded.
		/// </summary>
		private static void OnUnload(object sender, EventArgs e)
		{
			string file = AppPathConfig;
			if (_watchers.ContainsKey(file))
			{
				FileSystemWatcher watcher = (FileSystemWatcher) _watchers[file];
				watcher.Dispose();
				_watchers.Remove(file);
			}	
		}
	}
}