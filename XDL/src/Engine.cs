namespace NMatrix.XDL
{
	using System;
	using System.Data;
	using System.Xml;
	using System.Xml.XPath;
	using System.Xml.Schema;
	using System.IO;
	using Microsoft.Data.SqlXml;
	using System.Collections.Specialized;
	using System.Text;
	using NMatrix.XDL.Interfaces;

	/// <summary>
	/// The class responsible for loading and updating data.
	/// </summary>
	public class Engine
	{
		private static Factory _factoryinstance;
		// Change connection string to poing to you SQL Server database
		private string _connection = @"Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Initial Catalog=pubs;Data Source=(local)\NetSdk;";
		private StringBuilder _sberrors;

		/// <summary>
		///		Singleton pattern for the abstract factory.
		/// </summary>
		public Factory FactoryInstance
		{
			get
			{
				if (_factoryinstance == null) _factoryinstance = new Factory();
				return _factoryinstance;
			}
		}

		public Engine()
		{
		}

		public void Load(IDataSet dataSet, string xpathQuery)
		{
			//Temporary variables.
			IDataSet dsTmp = FactoryInstance.CreateDataSet(dataSet.SchemaFile, typeof(BaseDataSet));
			bool inserted;

			//Read schemas inside the datasets.
			dsTmp.ReadXmlSchema(dsTmp.SchemaFile);
			dataSet.ReadXmlSchema(dataSet.SchemaFile);

			//Split the query in parts, with the element name and its filters.
			string[] parts = xpathQuery.Split('/');

			//Reject queries with parent or current node filter expressions.
			//i.e.: publishers[pub_id="1389" and ./publisherTitles/price>20] should be converted to
			//		publishers[pub_id="1389"]/publisherTitles[price>20]	
			if (xpathQuery.IndexOf(".") != -1) throw new ArgumentException("Parent or current node references are not allowed in the expression.", "xpathQuery");

			//Save a collection with elements in the query, without its filters.
			StringCollection queryelements = new StringCollection();
			for (int i=0; i<parts.Length; i++)
			{
				if (parts[i].IndexOf("[") == -1) queryelements.Add(parts[i]);
				else queryelements.Add(parts[i].Substring(0, parts[i].IndexOf("[")));
			}

			//Build a collection of filters, with keys pointing to the element name.
			StringDictionary queryfilters = new StringDictionary();
			for (int i=0; i<parts.Length; i++)
			{
				if (parts[i].IndexOf("[") != -1)
					//Append element replacing brackets with node paths.
					queryfilters.Add(queryelements[i], (parts[i].Replace("[", "/")).Replace("]", ""));
			}

			//Rebuild the query to retrieve the firt node and all the children.
			StringBuilder sb = new StringBuilder();
			inserted = false;
			sb.Append(parts[0].Replace("]", ""));

			for (int i=1; i<queryelements.Count; i++)
			{
				//If there's a filter in the element, add the condition to the query.
				if (queryfilters.ContainsKey(queryelements[i]))
				{
					inserted = true;
					sb.Append(" and .");
					for (int j=1; j<i; j++) sb.Append("/" + queryelements[j]);
					sb.Append("/" + queryfilters[queryelements[i]]);
				}
			}
			//Remove additional "and" if no filter is present in the first element.
			if (inserted && parts[0].IndexOf("[") == -1) 
			{
				//Recover the length in characters, not bytes, from the string.
				int qty = parts[0].ToCharArray().Length;
				sb.Remove(qty, 5);
				sb.Insert(qty, "[");
			}
			if (inserted || (queryfilters.Count == 1 && parts[0].IndexOf("[") != -1)) sb.Append("]");

			//Look for an element matching the dataset name, and add it to the query if present in the schema.
			XmlDocument doc = new XmlDocument();
			doc.Load(dataSet.SchemaFile);
			//Add a namespace resolver for "xsd" prefix.
			XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
			mgr.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
			//If we find the enclosing dataset element, insert it at the beginning of the query.
			if (doc.SelectNodes("//xsd:element[@name='" + dsTmp.DataSetName + "']", mgr).Count != 0) 
				sb.Insert(0, dsTmp.DataSetName + "/");

			//Load document with all the children to start filtering undesired nodes.
			SqlXmlCommand cmd = new SqlXmlCommand(_connection);
			//Set the root equal to the DataSetName.
			cmd.RootTag = dsTmp.DataSetName;

			cmd.CommandText = sb.ToString();
			cmd.CommandType = SqlXmlCommandType.XPath;
			cmd.SchemaPath = dataSet.SchemaFile;
			
			MemoryStream stm = new MemoryStream();
			cmd.CommandStream = stm;
			XmlReader r = cmd.ExecuteXmlReader();
			
			dsTmp.ReadXml(r);

			SqlXmlAdapter ad = new SqlXmlAdapter(cmd);
			
			ad.Fill(dsTmp.GetState());
			XmlDataDocument docsrc = new XmlDataDocument(dsTmp.GetState());
			XmlDataDocument docdest = new XmlDataDocument(dataSet.GetState());
			dataSet.EnforceConstraints = false;

			//Rebuild the query to retrieve the filtered nodes, and append them without their children.
			sb = new StringBuilder();
			for (int i=0; i<queryelements.Count; i++)
			{
				//Append all the previous node elements in the "chain".
				for (int j=0; j<i; j++) sb.Append(parts[j] + "/");
				//Add the current element filter, without the closing bracket, to append the next conditions.
				sb.Append(parts[i].Replace("]", ""));
				inserted = false;

				//Look for subsequent elements to add to the query.
				for (int j=i+1; j<queryelements.Count; j++)
				{
					//If there's a filter in the element, add the condition to the query, otherwise ignore it.
					if (queryfilters.ContainsKey(queryelements[j]))
					{
						inserted = true;
						sb.Append(" and .");
						//Append again all the previous node elements in this element "chain".
						for (int k=i+1; k<j; k++) sb.Append("/" + queryelements[k]);
						//Add the current filter found.
						sb.Append("/" + queryfilters[queryelements[j]]);
					}
				}
				//Remove additional "and" if no filter is present in the first element.
				if (inserted && parts[i].IndexOf("[") == -1) 
				{
					//Recover the length in characters, not bytes, from the string.
					int qty = 0;
					for (int k=0; k<=i; k++) qty += parts[k].ToCharArray().Length;
					//Add the number of forward slashes appended to concatenate filters.
					qty += i;
					sb.Remove(qty, 5);
					sb.Insert(qty, "[");
				}
				//Close the filter expression.
				if (inserted || parts[i].IndexOf("[") != -1) sb.Append("]");

				//Execute the XPath query starting from the root document element.
				XmlNodeList nodes = docsrc.SelectNodes("//" + sb.ToString());

				//Iterate the nodes found with the query.
				foreach (XmlNode node in nodes)
				{
					//Retrieve the row corresponding to the node found.
					DataRow row = docsrc.GetRowFromElement((XmlElement)node);
					//Import the row into the destination DataSet.
					dataSet.Tables[row.Table.TableName].ImportRow(row);
				}

				sb = new StringBuilder();
			}
			dataSet.EnforceConstraints = true;
		}

		public string GetXml(string schemaFile, string xpathQuery)
		{
			IDataSet ds = FactoryInstance.CreateDataSet(schemaFile, typeof(BaseDataSet));
			Load(ds, xpathQuery);
			return ds.GetXml();
		}

		public void GetXml(string schemaFile, string xpathQuery, XmlWriter writer)
		{
			IDataSet ds = FactoryInstance.CreateDataSet(schemaFile, typeof(BaseDataSet));
			Load(ds, xpathQuery);
			ds.WriteXml(writer, XmlWriteMode.IgnoreSchema);
		}

		public void Update(IDataSet dataSet)
		{
			_sberrors = new StringBuilder();

			//Hold temporary xml in memory.
			MemoryStream mem = new MemoryStream();
			//Validate only changes to the dataset
			dataSet.GetChanges().WriteXml(mem, XmlWriteMode.IgnoreSchema);
			//Reset current position in the temporary memory representation.
			mem.Position = 0;
			//Read schema.
			XmlSchema sch;
			//We don't need the full path because the FileStream class can reads relative paths.
			using (FileStream fs = new FileStream(dataSet.SchemaFile, FileMode.Open))
			{	
				sch = XmlSchema.Read(fs, new ValidationEventHandler(this.OnValidation));
				sch.Compile(new ValidationEventHandler(this.OnValidation));
			}

			//Validate incoming data.
			XmlValidatingReader reader = new XmlValidatingReader(new XmlTextReader(mem));
			reader.Schemas.Add(sch);
			reader.ValidationType = ValidationType.Schema;
			reader.ValidationEventHandler += new ValidationEventHandler(this.OnValidation);

			//Read to end and check errors afterwards.
			while (reader.Read()) { }

			if (_sberrors.Length != 0) throw new ArgumentException(_sberrors.ToString(), "BaseDataSet");

			SqlXmlCommand cmd = new SqlXmlCommand(_connection);
			cmd.CommandType = SqlXmlCommandType.DiffGram;
			cmd.SchemaPath = dataSet.SchemaFile;
			SqlXmlAdapter ad = new SqlXmlAdapter(cmd);
			//Update database with DataSet data.
			ad.Update(dataSet.GetState());
		}

		public void Update(string xmlData, string schemaFile, XmlUpdateType updateType)
		{
			IDataSet ds = FactoryInstance.CreateDataSet(schemaFile, typeof(BaseDataSet));
			MemoryStream mem = new MemoryStream();
			StreamWriter w = new StreamWriter(mem);
			w.Write(xmlData);
			w.Flush();
			//Reset stream current position for the reader to start from the beginning
			mem.Position = 0;
			StreamReader r = new StreamReader(mem);

			if (updateType == XmlUpdateType.DiffGram)
                ds.ReadXml(r, XmlReadMode.DiffGram);
			else
				ds.ReadXml(r, XmlReadMode.IgnoreSchema);

			Update(ds);
		}

		private void OnValidation(object sender, ValidationEventArgs e)
		{
            _sberrors.Append(e.Message);
			_sberrors.Append(Environment.NewLine);
		}

		/// <summary>
		/// Newsgroup test method.
		/// </summary>
		public DataSet Test(string schemaFile)
		{
			try
			{
				DataSet dataSet = new DataSet("test");
				dataSet.ReadXmlSchema(schemaFile);
				string sql = "select * from publishers, titles where publishers.pub_id=titles.pub_id";
				SqlXmlAdapter ad = new SqlXmlAdapter(sql, SqlXmlCommandType.Sql, _connection);
				ad.Fill(dataSet);
				return dataSet;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
			}
			return null;
		}
	}

	public enum XmlUpdateType
	{
		DiffGram,
		XmlData
	}
}
