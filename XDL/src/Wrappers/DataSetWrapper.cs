namespace NMatrix.XDL.Wrappers
{
	using System;
	using System.Data;
	using System.ComponentModel;
	using System.Xml;
	using System.Xml.Schema;
	using System.IO;
	using NMatrix.XDL.Interfaces;

	[Serializable()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public class DataSetWrapper : MarshalByValueComponent, IDataSet
	{
		protected Factory _factory = new Engine().FactoryInstance;
		protected string _schemafile;
		private InternalDataSet _sourcedataset;

		public virtual DataSet GetState()
		{
			return _sourcedataset;
		}

		public virtual void SetState(DataSet state)
		{
			if (state.GetType() != typeof(InternalDataSet)) throw new ArgumentException("Internal state must be of type InternalDataSet.", "state");
			//Clear previous state handlers
			_sourcedataset.MergeFailed -= new MergeFailedEventHandler(WrapMergeFailed);
			_sourcedataset.DataSetGetSchemaSerializable -= new GetSchemaSerializableEventHandler(OnGetSchemaSerializable);
			_sourcedataset.DataSetReadXmlSerializable -= new ReadXmlSerializableEventHandler(OnReadXmlSerializable);
			_sourcedataset.DataSetShouldSerializeRelations -= new BooleanEventHandler(OnShouldSerializeRelations);
			_sourcedataset.DataSetShouldSerializeTables -= new BooleanEventHandler(OnShouldSerializeTables);
			_sourcedataset.DataSetOnRemoveRelation -= new OnRemoveRelationEventHandler(OnOnRemoveRelation);
			_sourcedataset.DataSetOnRemoveTable -= new OnRemoveTableEventHandler(OnOnRemoveTable);

			//Attach handlers to the new state
			_sourcedataset = (InternalDataSet) state;
			_sourcedataset.MergeFailed += new MergeFailedEventHandler(WrapMergeFailed);
			_sourcedataset.DataSetGetSchemaSerializable += new GetSchemaSerializableEventHandler(OnGetSchemaSerializable);
			_sourcedataset.DataSetReadXmlSerializable += new ReadXmlSerializableEventHandler(OnReadXmlSerializable);
			_sourcedataset.DataSetShouldSerializeRelations += new BooleanEventHandler(OnShouldSerializeRelations);
			_sourcedataset.DataSetShouldSerializeTables += new BooleanEventHandler(OnShouldSerializeTables);
			_sourcedataset.DataSetOnRemoveRelation += new OnRemoveRelationEventHandler(OnOnRemoveRelation);
			_sourcedataset.DataSetOnRemoveTable += new OnRemoveTableEventHandler(OnOnRemoveTable);
		}

	#region Constructors and Schema
		internal DataSetWrapper(string schemaFile, InternalDataSet datasetData) 
		{
			_sourcedataset = datasetData;
			SetSchema(schemaFile);
		}

		internal DataSetWrapper(string schemaFile)
		{
			_sourcedataset = new InternalDataSet();
			SetSchema(schemaFile);
		}

		private void SetSchema(string schemaFile)
		{
			_schemafile = schemaFile;
			InitClass();
		}

		//This is the "factory" method to override in descendents to change initialization.
		protected virtual void InitClass()
		{
		}

		public string SchemaFile
		{
			get { return _schemafile; }
		}
	#endregion

	#region Serialization-related dataset-"inherited" members
		protected virtual bool ShouldSerializeTables() 
		{
			return false;
		}
        
		private bool OnShouldSerializeTables(object sender, EventArgs e)
		{
			return ShouldSerializeTables();
		}
    
		protected virtual bool ShouldSerializeRelations() 
		{
			return false;
		}
        
		private bool OnShouldSerializeRelations(object sender, EventArgs e)
		{
			return ShouldSerializeRelations();
		}

		protected virtual void ReadXmlSerializable(XmlReader reader) 
		{
			this.ReadXml(reader, XmlReadMode.IgnoreSchema);
		}
        
		private void OnReadXmlSerializable(object sender, ReadXmlSerializableEventArgs e) 
		{
			ReadXmlSerializable(e.Reader);
		}
        
		protected virtual XmlSchema GetSchemaSerializable() 
		{
			MemoryStream stream = new MemoryStream();
			this.WriteXmlSchema(new XmlTextWriter(stream, null));
			stream.Position = 0;
			return XmlSchema.Read(new XmlTextReader(stream), null);
		}

		private XmlSchema OnGetSchemaSerializable(object sender, EventArgs e) 
		{
			return GetSchemaSerializable();
		}
	#endregion

	#region Other dataset-"inherited" members
		protected virtual void OnRemoveRelation(DataRelation relation) 
		{
		}

		private void OnOnRemoveRelation(object sender, OnRemoveRelationEventArgs e) 
		{
			OnRemoveRelation(e.Relation);
		}
        
		protected virtual void OnRemoveTable(DataTable table) 
		{
		}

		private void OnOnRemoveTable(object sender, OnRemoveTableEventArgs e) 
		{
			OnRemoveTable(e.Table);
		}
	#endregion

	#region Changed Methods
		public virtual IDataSet Clone() 
		{
			return _factory.CreateIDataSet(_schemafile, this.GetType(), _sourcedataset.Clone());
		}

		public virtual IDataSet Copy() 
		{
			return _factory.CreateIDataSet(_schemafile, this.GetType(), _sourcedataset.Copy());
		}

		public virtual IDataSet GetChanges() 
		{
			return _factory.CreateIDataSet(_schemafile, this.GetType(), _sourcedataset.GetChanges());
		}

		public virtual IDataSet GetChanges(DataRowState rowStates) 
		{
			return _factory.CreateIDataSet(_schemafile, this.GetType(), _sourcedataset.GetChanges(rowStates));
		}
	#endregion

	#region Merge methods
		public virtual void Merge(IDataSet dataSet) 
		{
			_sourcedataset.Merge(dataSet.GetState());
		}

		public virtual void Merge(IDataSet dataSet, bool preserveChanges) 
		{
			_sourcedataset.Merge(dataSet.GetState(), preserveChanges);
		}

		public virtual void Merge(IDataSet dataSet, bool preserveChanges, MissingSchemaAction missingSchemaAction) 
		{
			_sourcedataset.Merge(dataSet.GetState(), preserveChanges, missingSchemaAction);
		}

		public virtual void Merge(IDataTable table) 
		{
			_sourcedataset.Merge(table.GetState());
		}

		public virtual void Merge(IDataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction) 
		{
			_sourcedataset.Merge(table.GetState(), preserveChanges, missingSchemaAction);
		}

		public virtual void Merge(IDataRow[] rows) 
		{
			DataRow[] r = new DataRow[rows.Length];
			for (int i=0; i<rows.Length; i++)
				r[i] = rows[i].GetState();
			_sourcedataset.Merge(r);
		}

		public virtual void Merge(IDataRow[] rows, bool preserveChanges, MissingSchemaAction missingSchemaAction) 
		{
			DataRow[] r = new DataRow[rows.Length];
			for (int i=0; i<rows.Length; i++)
				r[i] = rows[i].GetState();
			_sourcedataset.Merge(r, preserveChanges, missingSchemaAction);
		}

		//Original methods
		public virtual void Merge(DataSet dataSet) 
		{
			_sourcedataset.Merge(dataSet);
		}

		public virtual void Merge(DataSet dataSet, bool preserveChanges) 
		{
			_sourcedataset.Merge(dataSet, preserveChanges);
		}

		public virtual void Merge(DataSet dataSet, bool preserveChanges, MissingSchemaAction missingSchemaAction) 
		{
			_sourcedataset.Merge(dataSet, preserveChanges, missingSchemaAction);
		}

		public virtual void Merge(DataTable table) 
		{
			_sourcedataset.Merge(table);
		}

		public virtual void Merge(DataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction) 
		{
			_sourcedataset.Merge(table, preserveChanges, missingSchemaAction);
		}

		public virtual void Merge(DataRow[] rows) 
		{
			_sourcedataset.Merge(rows);
		}

		public virtual void Merge(DataRow[] rows, bool preserveChanges, MissingSchemaAction missingSchemaAction) 
		{
			_sourcedataset.Merge(rows, preserveChanges, missingSchemaAction);
		}
	#endregion

	#region Delegates
		public event MergeFailedEventHandler MergeFailed;

		private void WrapMergeFailed(object sender, MergeFailedEventArgs e) 
		{
			OnMergeFailed(e);
		}	

		protected virtual void OnMergeFailed(MergeFailedEventArgs e) 
		{
			if ((this.MergeFailed != null)) 
			{
				this.MergeFailed(this, e);
			}
		}	
	#endregion

	#region Properties
		public virtual bool CaseSensitive 
		{
			get 
			{
				return _sourcedataset.CaseSensitive;
			}
			set 
			{
				_sourcedataset.CaseSensitive = value;
			}
		}

		public virtual DataViewManager DefaultViewManager 
		{
			get 
			{
				return _sourcedataset.DefaultViewManager;
			}
		}

		public virtual bool EnforceConstraints 
		{
			get 
			{
				return _sourcedataset.EnforceConstraints;
			}
			set 
			{
				_sourcedataset.EnforceConstraints = value;
			}
		}

		public virtual string DataSetName 
		{
			get 
			{
				return _sourcedataset.DataSetName;
			}
			set 
			{
				_sourcedataset.DataSetName = value;
			}
		}

		public virtual string Namespace 
		{
			get 
			{
				return _sourcedataset.Namespace;
			}
			set 
			{
				_sourcedataset.Namespace = value;
			}
		}

		public virtual string Prefix 
		{
			get 
			{
				return _sourcedataset.Prefix;
			}
			set 
			{
				_sourcedataset.Prefix = value;
			}
		}

		public virtual PropertyCollection ExtendedProperties 
		{
			get 
			{
				return _sourcedataset.ExtendedProperties;
			}
		}

		public virtual bool HasErrors 
		{
			get 
			{
				return _sourcedataset.HasErrors;
			}
		}

		public virtual System.Globalization.CultureInfo Locale 
		{
			get 
			{
				return _sourcedataset.Locale;
			}
			set 
			{
				_sourcedataset.Locale = value;
			}
		}

		public override ISite Site 
		{
			get 
			{
				return _sourcedataset.Site;
			}
			set 
			{
				_sourcedataset.Site = value;
			}
		}

		public virtual DataRelationCollection Relations 
		{
			get 
			{
				return _sourcedataset.Relations;
			}
		}

		public virtual DataTableCollection Tables 
		{
			get 
			{
				return _sourcedataset.Tables;
			}
		}

		public override IContainer Container 
		{
			get 
			{
				return _sourcedataset.Container;
			}
		}

		public override bool DesignMode 
		{
			get 
			{
				return _sourcedataset.DesignMode;
			}
		}

	#endregion

	#region Common Methods
		public virtual void EndInit() 
		{
			_sourcedataset.EndInit();
		}

		public virtual void BeginInit() 
		{
			_sourcedataset.BeginInit();
		}

		public virtual void Reset() 
		{
			_sourcedataset.Reset();
		}

		public virtual void RejectChanges() 
		{
			_sourcedataset.RejectChanges();
		}

		public override object GetService(System.Type service) 
		{
			return _sourcedataset.GetService(service);
		}

		public override int GetHashCode() 
		{
			return _sourcedataset.GetHashCode();
		}

		public override bool Equals(object obj) 
		{
			return _sourcedataset.Equals(obj);
		}

		public override string ToString() 
		{
			return _sourcedataset.ToString();
		}

		public virtual void AcceptChanges() 
		{
			_sourcedataset.AcceptChanges();
		}

		public virtual void Clear() 
		{
			_sourcedataset.Clear();
		}

		public virtual bool HasChanges() 
		{
			return _sourcedataset.HasChanges();
		}

		public virtual bool HasChanges(DataRowState rowStates) 
		{
			return _sourcedataset.HasChanges(rowStates);
		}
	#endregion

	#region Schema-related methods
		public virtual string GetXmlSchema() 
		{
			return _sourcedataset.GetXmlSchema();
		}

		public virtual void InferXmlSchema(XmlReader reader, string[] nsArray) 
		{
			_sourcedataset.InferXmlSchema(reader, nsArray);
		}

		public virtual void InferXmlSchema(Stream stream, string[] nsArray) 
		{
			_sourcedataset.InferXmlSchema(stream, nsArray);
		}

		public virtual void InferXmlSchema(TextReader reader, string[] nsArray) 
		{
			_sourcedataset.InferXmlSchema(reader, nsArray);
		}

		public virtual void InferXmlSchema(string fileName, string[] nsArray) 
		{
			_sourcedataset.InferXmlSchema(fileName, nsArray);
		}

		public virtual void ReadXmlSchema(XmlReader reader) 
		{
			_sourcedataset.ReadXmlSchema(reader);
		}

		public virtual void ReadXmlSchema(Stream stream) 
		{
			_sourcedataset.ReadXmlSchema(stream);
		}

		public virtual void ReadXmlSchema(TextReader reader) 
		{
			_sourcedataset.ReadXmlSchema(reader);
		}

		public virtual void ReadXmlSchema(string fileName) 
		{
			_sourcedataset.ReadXmlSchema(fileName);
		}

		public virtual void WriteXmlSchema(Stream stream) 
		{
			_sourcedataset.WriteXmlSchema(stream);
		}

		public virtual void WriteXmlSchema(TextWriter writer) 
		{
			_sourcedataset.WriteXmlSchema(writer);
		}

		public virtual void WriteXmlSchema(XmlWriter writer) 
		{
			_sourcedataset.WriteXmlSchema(writer);
		}

		public virtual void WriteXmlSchema(string fileName) 
		{
			_sourcedataset.WriteXmlSchema(fileName);
		}
	#endregion

	#region Xml data-related methods
		public virtual string GetXml() 
		{
			return _sourcedataset.GetXml();
		}

		public virtual XmlReadMode ReadXml(XmlReader reader) 
		{
			return _sourcedataset.ReadXml(reader);
		}

		public virtual XmlReadMode ReadXml(Stream stream) 
		{
			return _sourcedataset.ReadXml(stream);
		}

		public virtual XmlReadMode ReadXml(TextReader reader) 
		{
			return _sourcedataset.ReadXml(reader);
		}

		public virtual XmlReadMode ReadXml(string fileName) 
		{
			return _sourcedataset.ReadXml(fileName);
		}

		public virtual XmlReadMode ReadXml(XmlReader reader, XmlReadMode mode) 
		{
			return _sourcedataset.ReadXml(reader, mode);
		}

		public virtual XmlReadMode ReadXml(Stream stream, XmlReadMode mode) 
		{
			return _sourcedataset.ReadXml(stream, mode);
		}

		public virtual XmlReadMode ReadXml(TextReader reader, XmlReadMode mode) 
		{
			return _sourcedataset.ReadXml(reader, mode);
		}

		public virtual XmlReadMode ReadXml(string fileName, XmlReadMode mode) 
		{
			return _sourcedataset.ReadXml(fileName, mode);
		}

		public virtual void WriteXml(Stream stream) 
		{
			_sourcedataset.WriteXml(stream);
		}

		public virtual void WriteXml(TextWriter writer) 
		{
			_sourcedataset.WriteXml(writer);
		}

		public virtual void WriteXml(XmlWriter writer) 
		{
			_sourcedataset.WriteXml(writer);
		}

		public virtual void WriteXml(string fileName) 
		{
			_sourcedataset.WriteXml(fileName);
		}

		public virtual void WriteXml(Stream stream, XmlWriteMode mode) 
		{
			_sourcedataset.WriteXml(stream, mode);
		}

		public virtual void WriteXml(TextWriter writer, XmlWriteMode mode) 
		{
			_sourcedataset.WriteXml(writer, mode);
		}

		public virtual void WriteXml(XmlWriter writer, XmlWriteMode mode) 
		{
			_sourcedataset.WriteXml(writer, mode);
		}

		public virtual void WriteXml(string fileName, XmlWriteMode mode) 
		{
			_sourcedataset.WriteXml(fileName, mode);
		}
	#endregion

	}
}