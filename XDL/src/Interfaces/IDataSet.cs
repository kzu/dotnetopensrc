namespace NMatrix.XDL.Interfaces
{
	using System;
	using System.Data;
	using System.ComponentModel;
	using System.Xml;
	using System.IO;

	public interface IDataSet
	{
		event MergeFailedEventHandler MergeFailed;

		//Added to manage internal Dataset representation.
		DataSet GetState();
		void SetState(DataSet state);

		string SchemaFile
		{
			get ;
		}

	#region Properties
		bool CaseSensitive 
		{
			get ;
		}

		DataViewManager DefaultViewManager 
		{
			get ;
		}

		bool EnforceConstraints 
		{
			get ;
			set ;
		}

		string DataSetName 
		{
			get ;
			set ;
		}

		string Namespace 
		{
			get ;
			set ;
		}

		string Prefix 
		{
			get ;
			set ;
		}

		PropertyCollection ExtendedProperties 
		{
			get ;
		}

		bool HasErrors 
		{
			get ;
		}

		System.Globalization.CultureInfo Locale 
		{
			get ;
			set ;
		}

		ISite Site 
		{
			get ;
			set ;
		}

		DataRelationCollection Relations 
		{
			get ;
		}

		DataTableCollection Tables 
		{
			get ;
		}

		IContainer Container 
		{
			get ;
		}

		bool DesignMode 
		{
			get ;
		}
	#endregion

	#region Methods
		IDataSet Clone();
		IDataSet Copy();
		IDataSet GetChanges() ;
		IDataSet GetChanges(DataRowState rowStates);
		void EndInit();
		void BeginInit();
		void Reset();
		void RejectChanges();
		void AcceptChanges();
		void Clear();
		bool HasChanges();
		bool HasChanges(DataRowState rowStates);
	#endregion

	#region Merge methods
		//Extended methods
		void Merge(IDataSet dataSet);
		void Merge(IDataSet dataSet, bool preserveChanges);
		void Merge(IDataSet	dataSet, bool preserveChanges, MissingSchemaAction missingSchemaAction);
		void Merge(IDataTable table);
		void Merge(IDataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction);
		void Merge(IDataRow[] rows);
		void Merge(IDataRow[] rows, bool preserveChanges, MissingSchemaAction missingSchemaAction);
		//Original DataSet methods
		void Merge(DataSet dataSet);
		void Merge(DataSet dataSet, bool preserveChanges);
		void Merge(DataSet dataSet, bool preserveChanges, MissingSchemaAction missingSchemaAction);
		void Merge(DataTable table);
		void Merge(DataTable table, bool preserveChanges, MissingSchemaAction missingSchemaAction);
		void Merge(DataRow[] rows);
		void Merge(DataRow[] rows, bool preserveChanges, MissingSchemaAction missingSchemaAction);
	#endregion

	#region Schema-related methods
		string GetXmlSchema();
		void InferXmlSchema(XmlReader reader, string[] nsArray);
		void InferXmlSchema(Stream stream, string[] nsArray);
		void InferXmlSchema(TextReader reader, string[] nsArray);
		void InferXmlSchema(string fileName, string[] nsArray);
		//Read
		void ReadXmlSchema(XmlReader reader);
		void ReadXmlSchema(Stream stream);
		void ReadXmlSchema(TextReader reader);
		void ReadXmlSchema(string fileName);
		//Write
		void WriteXmlSchema(Stream stream);
		void WriteXmlSchema(TextWriter writer);
		void WriteXmlSchema(XmlWriter writer);
		void WriteXmlSchema(string fileName);
	#endregion

	#region Xml data-related methods
		string GetXml();
		//Read
		XmlReadMode ReadXml(XmlReader reader);
		XmlReadMode ReadXml(Stream stream);
		XmlReadMode ReadXml(TextReader reader);
		XmlReadMode ReadXml(string fileName);
		XmlReadMode ReadXml(XmlReader reader, XmlReadMode mode);
		XmlReadMode ReadXml(Stream stream, XmlReadMode mode);
		XmlReadMode ReadXml(TextReader reader, XmlReadMode mode);
		XmlReadMode ReadXml(string fileName, XmlReadMode mode);
		//Write
		void WriteXml(Stream stream);
		void WriteXml(TextWriter writer);
		void WriteXml(XmlWriter writer);
		void WriteXml(string fileName);
		void WriteXml(Stream stream, XmlWriteMode mode);
		void WriteXml(TextWriter writer, XmlWriteMode mode);
		void WriteXml(XmlWriter writer, XmlWriteMode mode);
		void WriteXml(string fileName, XmlWriteMode mode);
	#endregion
	}
}