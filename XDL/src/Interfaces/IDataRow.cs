namespace NMatrix.XDL.Interfaces
{
	using System;
	using System.Data;

	public interface IDataRow
	{
		//Added to manage internal Dataset representation.
		DataRow GetState();
		void SetState(DataRow state);

	#region Changed members
		//New syntax
		IDataTable Table
		{
			get ;
		}
		IDataRow[] GetChildRows(string relationName, Type childType);
		IDataRow[] GetChildRows(string relationName, DataRowVersion version, Type childType);
		IDataRow[] GetChildRows(DataRelation relation, Type childType);
		IDataRow[] GetChildRows(DataRelation relation, DataRowVersion version, Type childType);
		IDataRow GetParentRow(string relationName, Type parentType);
		IDataRow GetParentRow(string relationName, DataRowVersion version, Type parentType);
		IDataRow GetParentRow(DataRelation relation, Type parentType);
		IDataRow GetParentRow(DataRelation relation, DataRowVersion version, Type parentType);
		IDataRow[] GetParentRows(string relationName, Type parentType);
		IDataRow[] GetParentRows(string relationName, DataRowVersion version, Type parentType);
		IDataRow[] GetParentRows(DataRelation relation, Type parentType);
		IDataRow[] GetParentRows(DataRelation relation, DataRowVersion version, Type parentType);
		void SetParentRow(IDataRow parentRow);
		void SetParentRow(IDataRow parentRow, DataRelation relation);
		//Original syntax
		DataRow[] GetChildRows(string relationName);
		DataRow[] GetChildRows(string relationName, DataRowVersion version);
		DataRow[] GetChildRows(DataRelation relation);
		DataRow[] GetChildRows(DataRelation relation, DataRowVersion version);
		DataRow GetParentRow(string relationName);
		DataRow GetParentRow(string relationName, DataRowVersion version);
		DataRow GetParentRow(DataRelation relation);
		DataRow GetParentRow(DataRelation relation, DataRowVersion version);
		DataRow[] GetParentRows(string relationName);
		DataRow[] GetParentRows(string relationName, DataRowVersion version);
		DataRow[] GetParentRows(DataRelation relation);
		DataRow[] GetParentRows(DataRelation relation, DataRowVersion version);
		void SetParentRow(DataRow parentRow);
		void SetParentRow(DataRow parentRow, DataRelation relation);
	#endregion

	#region Properties
		string RowError 
		{
			get ;
			set ;
		}

		DataRowState RowState 
		{
			get ;
		}

		object this[int columnIndex] 
		{
			get ;
			set ;
		}

		object this[string columnName] 
		{
			get ;
			set ;
		}

		object this[DataColumn column] 
		{
			get ;
			set ;
		}

		object this[int columnIndex, DataRowVersion version] 
		{
			get ;
		}

		object this[string columnName, DataRowVersion version] 
		{
			get ;
		}

		object this[DataColumn column, DataRowVersion version] 
		{
			get ;
		}

		object[] ItemArray 
		{
			get ;
			set ;
		}

		bool HasErrors 
		{
			get ;
		}
	#endregion

	#region Methods
		void AcceptChanges();
		void BeginEdit();
		void CancelEdit();
		void Delete();
		void EndEdit();
		void SetColumnError(int columnIndex, string error);
		void SetColumnError(string columnName, string error);
		void SetColumnError(DataColumn column, string error);
		string GetColumnError(int columnIndex);
		string GetColumnError(string columnName);
		string GetColumnError(DataColumn column);
		void ClearErrors();
		DataColumn[] GetColumnsInError();
		bool HasVersion(DataRowVersion version);
		bool IsNull(int columnIndex);
		bool IsNull(string columnName);
		bool IsNull(DataColumn column);
		bool IsNull(DataColumn column, DataRowVersion version);
		void RejectChanges();
	#endregion
	}
}