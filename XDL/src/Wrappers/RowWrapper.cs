namespace NMatrix.XDL.Wrappers
{
	using System;
	using System.Data;
	using NMatrix.XDL.Interfaces;

	public class RowWrapper : IDataRow
	{
		protected Factory _factory = new Engine().FactoryInstance;
		private DataRow _sourcerow;
		private IDataTable _sourcetable;

		internal RowWrapper(DataRow rowData)
		{
			SetState(rowData);
		}

		internal RowWrapper()
		{
		}

		protected virtual Type GetRowType() 
		{
			return typeof(RowWrapper);
		}

		protected virtual Type GetTableType() 
		{
			return typeof(TableWrapper);
		}

		protected IDataRow CreateDataRow()
		{
			return _factory.CreateIDataRow(GetRowType());
		}

		public DataRow GetState()
		{
			return _sourcerow;
		}

		public virtual void SetState(DataRow state)
		{
			_sourcerow = state;
		}


	#region Changed members
		public virtual IDataTable Table 
		{
			get 
			{ 
				if (_sourcetable == null) 
				{
					_sourcetable = _factory.CreateIDataTable(GetTableType(), _sourcerow.Table);
				}
				return _sourcetable; 
			}
		}

		public virtual IDataRow[] GetChildRows(string relationName, Type childType) 
		{
			DataRow[] res = _sourcerow.GetChildRows(relationName);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow[] GetChildRows(string relationName, DataRowVersion version, Type childType) 
		{
			DataRow[] res = _sourcerow.GetChildRows(relationName, version);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow[] GetChildRows(DataRelation relation, Type childType) 
		{
			DataRow[] res = _sourcerow.GetChildRows(relation);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow[] GetChildRows(DataRelation relation, DataRowVersion version, Type childType) 
		{
			DataRow[] res = _sourcerow.GetChildRows(relation, version);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow GetParentRow(string relationName, Type parentType) 
		{
			DataRow res = _sourcerow.GetParentRow(relationName);
			if (res != null) 
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res);
				return obj;
			}				
			return null;
		}

		public virtual IDataRow GetParentRow(string relationName, DataRowVersion version, Type parentType) 
		{
			DataRow res = _sourcerow.GetParentRow(relationName, version);
			if (res != null) 
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res);
				return obj;
			}				
			return null;
		}

		public virtual IDataRow GetParentRow(DataRelation relation, Type parentType) 
		{
			DataRow res = _sourcerow.GetParentRow(relation);
			if (res != null) 
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res);
				return obj;
			}				
			return null;
		}

		public virtual IDataRow GetParentRow(DataRelation relation, DataRowVersion version, Type parentType) 
		{
			DataRow res = _sourcerow.GetParentRow(relation, version);
			if (res != null) 
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res);
				return obj;
			}				
			return null;
		}

		public virtual IDataRow[] GetParentRows(string relationName, Type parentType) 
		{
			DataRow[] res = _sourcerow.GetParentRows(relationName);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow[] GetParentRows(string relationName, DataRowVersion version, Type parentType) 
		{
			DataRow[] res = _sourcerow.GetParentRows(relationName, version);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow[] GetParentRows(DataRelation relation, Type parentType) 
		{
			DataRow[] res = _sourcerow.GetParentRows(relation);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow[] GetParentRows(DataRelation relation, DataRowVersion version, Type parentType) 
		{
			DataRow[] res = _sourcerow.GetParentRows(relation, version);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = _factory.CreateIDataRow(parentType);
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual void SetParentRow(IDataRow parentRow) 
		{
			_sourcerow.SetParentRow(parentRow.GetState());
		}

		public virtual void SetParentRow(IDataRow parentRow, DataRelation relation) 
		{
			_sourcerow.SetParentRow(parentRow.GetState(), relation);
		}

		//Original syntax
		public DataRow[] GetChildRows(string relationName)
		{
			return _sourcerow.GetChildRows(relationName);
		}
		public DataRow[] GetChildRows(string relationName, DataRowVersion version)
		{
			return _sourcerow.GetChildRows(relationName, version);
		}
		public DataRow[] GetChildRows(DataRelation relation)
		{
			return _sourcerow.GetChildRows(relation);
		}
		public DataRow[] GetChildRows(DataRelation relation, DataRowVersion version)
		{
			return _sourcerow.GetChildRows(relation, version);
		}
		public DataRow GetParentRow(string relationName)
		{
			return _sourcerow.GetParentRow(relationName);
		}
		public DataRow GetParentRow(string relationName, DataRowVersion version)
		{
			return _sourcerow.GetParentRow(relationName, version);
		}
		public DataRow GetParentRow(DataRelation relation)
		{
			return _sourcerow.GetParentRow(relation);
		}
		public DataRow GetParentRow(DataRelation relation, DataRowVersion version)
		{
			return _sourcerow.GetParentRow(relation, version);
		}
		public DataRow[] GetParentRows(string relationName)
		{
			return _sourcerow.GetParentRows(relationName);
		}
		public DataRow[] GetParentRows(string relationName, DataRowVersion version)
		{
			return _sourcerow.GetParentRows(relationName, version);
		}
		public DataRow[] GetParentRows(DataRelation relation)
		{
			return _sourcerow.GetParentRows(relation);
		}
		public DataRow[] GetParentRows(DataRelation relation, DataRowVersion version)
		{
			return _sourcerow.GetParentRows(relation, version);
		}
		public void SetParentRow(DataRow parentRow)
		{
			_sourcerow.SetParentRow(parentRow);
		}
		public void SetParentRow(DataRow parentRow, DataRelation relation)
		{
			_sourcerow.SetParentRow(parentRow, relation);
		}
	#endregion

	#region Properties
		public virtual string RowError 
		{
			get 
			{
				return _sourcerow.RowError;
			}
			set 
			{
				_sourcerow.RowError = value;
			}
		}

		public virtual DataRowState RowState 
		{
			get 
			{
				return _sourcerow.RowState;
			}
		}

		public virtual object this[int columnIndex] 
		{
			get 
			{
				return _sourcerow[columnIndex];
			}
			set 
			{
				_sourcerow[columnIndex] = value;
			}
		}

		public virtual object this[string columnName] 
		{
			get 
			{
				return _sourcerow[columnName];
			}
			set 
			{
				_sourcerow[columnName] = value;
			}
		}

		public virtual object this[DataColumn column] 
		{
			get 
			{
				return _sourcerow[column];
			}
			set 
			{
				_sourcerow[column] = value;
			}
		}

		public virtual object this[int columnIndex, DataRowVersion version] 
		{
			get 
			{
				return _sourcerow[columnIndex, version];
			}
		}

		public virtual object this[string columnName, DataRowVersion version] 
		{
			get 
			{
				return _sourcerow[columnName, version];
			}
		}

		public virtual object this[DataColumn column, DataRowVersion version] 
		{
			get 
			{
				return _sourcerow[column, version];
			}
		}

		public virtual object[] ItemArray 
		{
			get 
			{
				return _sourcerow.ItemArray;
			}
			set 
			{
				_sourcerow.ItemArray = value;
			}
		}

		public virtual bool HasErrors 
		{
			get 
			{
				return _sourcerow.HasErrors;
			}
		}
	#endregion

	#region Common methods
		public override int GetHashCode() 
		{
			return _sourcerow.GetHashCode();
		}

		public override bool Equals(object obj) 
		{
			return _sourcerow.Equals(obj);
		}

		public override string ToString() 
		{
			return _sourcerow.ToString();
		}

		public virtual void AcceptChanges() 
		{
			_sourcerow.AcceptChanges();
		}

		public virtual void BeginEdit() 
		{
			_sourcerow.BeginEdit();
		}

		public virtual void CancelEdit() 
		{
			_sourcerow.CancelEdit();
		}

		public virtual void Delete() 
		{
			_sourcerow.Delete();
		}

		public virtual void EndEdit() 
		{
			_sourcerow.EndEdit();
		}

		public virtual void SetColumnError(int columnIndex, string error) 
		{
			_sourcerow.SetColumnError(columnIndex, error);
		}

		public virtual void SetColumnError(string columnName, string error) 
		{
			_sourcerow.SetColumnError(columnName, error);
		}

		public virtual void SetColumnError(DataColumn column, string error) 
		{
			_sourcerow.SetColumnError(column, error);
		}

		public virtual string GetColumnError(int columnIndex) 
		{
			return _sourcerow.GetColumnError(columnIndex);
		}

		public virtual string GetColumnError(string columnName) 
		{
			return _sourcerow.GetColumnError(columnName);
		}

		public virtual string GetColumnError(DataColumn column) 
		{
			return _sourcerow.GetColumnError(column);
		}

		public virtual void ClearErrors() 
		{
			_sourcerow.ClearErrors();
		}

		public virtual DataColumn[] GetColumnsInError() 
		{
			return _sourcerow.GetColumnsInError();
		}

		public virtual bool HasVersion(DataRowVersion version) 
		{
			return _sourcerow.HasVersion(version);
		}

		public virtual bool IsNull(int columnIndex) 
		{
			return _sourcerow.IsNull(columnIndex);
		}

		public virtual bool IsNull(string columnName) 
		{
			return _sourcerow.IsNull(columnName);
		}

		public virtual bool IsNull(DataColumn column) 
		{
			return _sourcerow.IsNull(column);
		}

		public virtual bool IsNull(DataColumn column, DataRowVersion version) 
		{
			return _sourcerow.IsNull(column, version);
		}

		public virtual void RejectChanges() 
		{
			_sourcerow.RejectChanges();
		}

	#endregion
	}
}