namespace NMatrix.XDL.Interfaces
{
	using System;
	using System.Data;
	using System.ComponentModel;
	using System.Xml;
	using System.IO;
	using NMatrix.XDL.Wrappers;

	public interface IDataTable
	{
		event DataRowChangeEventHandler RowChanged;            
		event DataRowChangeEventHandler RowChanging;
		event DataRowChangeEventHandler RowDeleted;
		event DataRowChangeEventHandler RowDeleting;
		event DataColumnChangeEventHandler ColumnChanged;
		event DataColumnChangeEventHandler ColumnChanging;

		//Added to manage internal Dataset representation.
		DataTable GetState();
		void SetState(DataTable state);

	#region Changed members
		//New syntax
		IDataRowCollection Rows
		{
			get ;
		}
		IDataTable Copy();
		IDataRow[] GetErrors();
		IDataRow NewRow();
		void ImportRow(IDataRow row);
		IDataRow LoadDataRow(object[] values, bool fAcceptChanges);
		IDataRow[] Select();
		IDataRow[] Select(string filterExpression);
		IDataRow[] Select(string filterExpression, string sort);
		IDataRow[] Select(string filterExpression, string sort, DataViewRowState recordStates);
		//Original methods
		void ImportRow(DataRow row);
	#endregion

	#region Properties
		bool CaseSensitive
		{
			get ;
			set ;
		}
    
		DataRelationCollection ChildRelations 
		{
			get ;
		}
    
		DataColumnCollection Columns 
		{
			get ;
		}
    
		ConstraintCollection Constraints 
		{
			get ;
		}
    
		DataSet DataSet 
		{
			get ;
		}
    
		DataView DefaultView 
		{
			get ;
		}
    
		string DisplayExpression 
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
    
		int MinimumCapacity 
		{
			get ;
			set ;
		}
    
		DataRelationCollection ParentRelations 
		{
			get ;
		}
    
		DataColumn[] PrimaryKey 
		{
			get ;
			set ;
		}
    
		string TableName 
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
    
		ISite Site 
		{
			get ;
			set ;
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

	#region Common Methods
		void EndInit();
		void BeginInit();
		void Reset();
		object GetService(Type service);
		void Dispose();
		void AcceptChanges();
		void Clear();
		object Compute(string expression, string filter);
		DataTable GetChanges();
		DataTable GetChanges(DataRowState rowStates);
		void RejectChanges();
		void BeginLoadData();
		void EndLoadData();
	#endregion
	}
}