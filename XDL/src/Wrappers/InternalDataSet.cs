namespace NMatrix.XDL.Wrappers 
{
	using System;
	using System.Data;
	using System.Xml;
	using System.Xml.Schema;
	using System.IO;
	using System.Runtime.Serialization;
    
    
	[Serializable()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public class InternalDataSet : System.Data.DataSet 
	{   
		public event BooleanEventHandler DataSetShouldSerializeTables;
		public event BooleanEventHandler DataSetShouldSerializeRelations;
		public event ReadXmlSerializableEventHandler DataSetReadXmlSerializable;
		public event GetSchemaSerializableEventHandler DataSetGetSchemaSerializable;
		public event OnRemoveRelationEventHandler DataSetOnRemoveRelation;
		public event OnRemoveTableEventHandler DataSetOnRemoveTable;

		internal InternalDataSet()
		{
		}

		private InternalDataSet(SerializationInfo info, StreamingContext context) 
		{
			this.GetSerializationData(info, context);
		}
        
		protected override bool ShouldSerializeTables() 
		{
			if (DataSetShouldSerializeTables != null)
				return DataSetShouldSerializeTables(this, EventArgs.Empty);
			else
				return false; 
		}
        
		protected override bool ShouldSerializeRelations() 
		{
			if (DataSetShouldSerializeRelations != null)
				return DataSetShouldSerializeRelations(this, EventArgs.Empty);
			else
				return false;
		}
        
		protected override void ReadXmlSerializable(XmlReader reader) 
		{
			if (DataSetReadXmlSerializable != null)
				DataSetReadXmlSerializable(this, new ReadXmlSerializableEventArgs(reader));
			else
				this.ReadXml(reader, XmlReadMode.IgnoreSchema);
		}
        
		protected override XmlSchema GetSchemaSerializable() 
		{
			if (DataSetGetSchemaSerializable != null)
				return DataSetGetSchemaSerializable(this, EventArgs.Empty);
			else
			{
				MemoryStream stream = new MemoryStream();
				this.WriteXmlSchema(new XmlTextWriter(stream, null));
				stream.Position = 0;
				return XmlSchema.Read(new XmlTextReader(stream), null);
			}
		}

		protected override void OnRemoveRelation(DataRelation relation)
		{
			if (DataSetOnRemoveRelation != null)
				DataSetOnRemoveRelation(this, new OnRemoveRelationEventArgs(relation));
		}

		protected override void OnRemoveTable(DataTable table)
		{
			if (DataSetOnRemoveTable != null)
				DataSetOnRemoveTable(this, new OnRemoveTableEventArgs(table));
		}
	}

	public delegate bool BooleanEventHandler(object sender, EventArgs e);
	public delegate XmlSchema GetSchemaSerializableEventHandler(object sender, EventArgs e);
	public delegate void ReadXmlSerializableEventHandler(object sender, ReadXmlSerializableEventArgs e);
	public delegate void OnRemoveRelationEventHandler(object sender, OnRemoveRelationEventArgs e);
	public delegate void OnRemoveTableEventHandler(object sender, OnRemoveTableEventArgs e);

#region Argument classes
	public class ReadXmlSerializableEventArgs : EventArgs
	{
		private XmlReader _reader;

		internal ReadXmlSerializableEventArgs(XmlReader reader)
		{
			_reader = reader;
		}
		
		public XmlReader Reader
		{
			get { return _reader; }
			set { _reader = value; }
		}
	}

	public class OnRemoveRelationEventArgs : EventArgs
	{
		private DataRelation _relation;

		internal OnRemoveRelationEventArgs(DataRelation relation)
		{
			_relation = relation;
		}
		
		public DataRelation Relation
		{
			get { return _relation; }
			set { _relation = value; }
		}
	}

	public class OnRemoveTableEventArgs : EventArgs
	{
		private DataTable _table;

		internal OnRemoveTableEventArgs(DataTable table)
		{
			_table = table;
		}
		
		public DataTable Table
		{
			get { return _table; }
			set { _table = value; }
		}
	}
#endregion
}
