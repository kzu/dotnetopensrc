namespace NMatrix.XDL.Wrappers
{
	using System;
	using System.Data;
	using System.ComponentModel;
	using NMatrix.XDL.Interfaces;

	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public class TableWrapper : MarshalByValueComponent, IDataTable
	{
		protected Factory _factory = new Engine().FactoryInstance;
		private DataTable _sourcetable;
		private IDataRowCollection _rows;
    
		internal TableWrapper(DataTable tableData)
		{
			SetState(tableData);
		}

		//Override in descendents 
		protected virtual Type GetRowType() 
		{
			return typeof(RowWrapper);
		}

		private IDataRow CreateDataRow()
		{
			return _factory.CreateIDataRow(GetRowType());
		}

		public DataTable GetState()
		{
			return _sourcetable;
		}

		public virtual void SetState(DataTable state)
		{
			_sourcetable = state;
			_rows = new RowCollectionWrapper(GetRowType(), _sourcetable.Rows);
			_sourcetable.ColumnChanged += new DataColumnChangeEventHandler(WrapColumnChanged);
			_sourcetable.ColumnChanging += new DataColumnChangeEventHandler(WrapColumnChanging);
			_sourcetable.RowChanged += new DataRowChangeEventHandler(WrapRowChanged);
			_sourcetable.RowChanging += new DataRowChangeEventHandler(WrapRowChanging);
			_sourcetable.RowDeleted += new DataRowChangeEventHandler(WrapRowDeleted);
			_sourcetable.RowDeleting += new DataRowChangeEventHandler(WrapRowDeleting);
		}

	#region Changed members
		public virtual IDataRowCollection Rows 
		{
			get {return _rows; }
		}

		public virtual IDataTable Copy() 
		{
			return _factory.CreateIDataTable(this.GetType(), _sourcetable.Copy());
		}

		public virtual IDataRow[] GetErrors()
		{
			DataRow[] res = _sourcetable.GetErrors();
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}

		public virtual IDataRow NewRow() 
		{
			IDataRow obj = CreateDataRow();
			obj.SetState(_sourcetable.NewRow());
			return obj;
		}
 
		public virtual void ImportRow(IDataRow row) 
		{
			_sourcetable.ImportRow(row.GetState());
		}

		public virtual void ImportRow(DataRow row) 
		{
			_sourcetable.ImportRow(row);
		}

		public IDataRow LoadDataRow(object[] values, bool fAcceptChanges)
		{
			DataRow r = _sourcetable.LoadDataRow(values, fAcceptChanges);
			if (r == null) return null;
			IDataRow row = CreateDataRow();
			row.SetState(r);
			return row;
		}


		public virtual IDataRow[] Select()
		{
			DataRow[] res = _sourcetable.Select();
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}
    
		public virtual IDataRow[] Select(string filterExpression) 
		{
			DataRow[] res = _sourcetable.Select(filterExpression);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}
    
		public virtual IDataRow[] Select(string filterExpression, string sort) 
		{
			DataRow[] res = _sourcetable.Select(filterExpression, sort);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}
    
		public virtual IDataRow[] Select(string filterExpression, string sort, DataViewRowState recordStates) 
		{
			DataRow[] res = _sourcetable.Select(filterExpression, sort, recordStates);
			IDataRow[] rows = new IDataRow[res.Length];
			for (int i=0; i<res.Length; i++)
			{
				IDataRow obj = CreateDataRow();
				obj.SetState(res[i]);
				rows[i] = obj;
			}
			return rows;
		}
    
    
	#endregion

	#region Delegates
		public event DataRowChangeEventHandler RowChanged;            
		public event DataRowChangeEventHandler RowChanging;
		public event DataRowChangeEventHandler RowDeleted;
		public event DataRowChangeEventHandler RowDeleting;
		public event DataColumnChangeEventHandler ColumnChanged;
		public event DataColumnChangeEventHandler ColumnChanging;

		private void WrapColumnChanged(object sender, DataColumnChangeEventArgs e) 
		{
			OnColumnChanged(e);
		}

		protected virtual void OnColumnChanged(DataColumnChangeEventArgs e) 
		{
			if ((this.ColumnChanged != null)) 
			{
				this.ColumnChanged(this, e);
			}
		}

		private void WrapColumnChanging(object sender, DataColumnChangeEventArgs e) 
		{
			OnColumnChanging(e);
		}

		protected virtual void OnColumnChanging(DataColumnChangeEventArgs e) 
		{
			if ((this.ColumnChanging != null)) 
			{
				this.ColumnChanging(this, e);
			}
		}

		private void WrapRowChanged(object sender, DataRowChangeEventArgs e) 
		{
			OnRowChanged(e);
		}
            
		protected virtual void OnRowChanged(DataRowChangeEventArgs e) 
		{
			if ((this.RowChanged != null)) 
			{
				this.RowChanged(this, e);
			}
		}

		private void WrapRowChanging(object sender, DataRowChangeEventArgs e) 
		{
			OnRowChanging(e);
		}
            
		protected virtual void OnRowChanging(DataRowChangeEventArgs e) 
		{
			if ((this.RowChanging != null)) 
			{
				this.RowChanging(this, e);
			}
		}
            
		private void WrapRowDeleted(object sender, DataRowChangeEventArgs e) 
		{
			OnRowDeleted(e);
		}
            
		protected virtual void OnRowDeleted(DataRowChangeEventArgs e) 
		{
			if ((this.RowDeleted != null)) 
			{
				this.RowDeleted(this, e);
			}
		}
            
		private void WrapRowDeleting(object sender, DataRowChangeEventArgs e) 
		{
			OnRowDeleting(e);
		}	

            
		protected virtual void OnRowDeleting(DataRowChangeEventArgs e) 
		{
			if ((this.RowDeleting != null)) 
			{
				this.RowDeleting(this, e);
			}
		}	
	#endregion
    
	#region Properties
		public virtual bool CaseSensitive 
		{
			get 
			{
				return _sourcetable.CaseSensitive;
			}
			set 
			{
				_sourcetable.CaseSensitive = value;
			}
		}
    
		public virtual DataRelationCollection ChildRelations 
		{
			get 
			{
				return _sourcetable.ChildRelations;
			}
		}
    
		public virtual DataColumnCollection Columns 
		{
			get 
			{
				return _sourcetable.Columns;
			}
		}
    
		public virtual ConstraintCollection Constraints 
		{
			get 
			{
				return _sourcetable.Constraints;
			}
		}
    
		public virtual DataSet DataSet 
		{
			get 
			{
				return _sourcetable.DataSet;
			}
		}
    
		public virtual DataView DefaultView 
		{
			get 
			{
				return _sourcetable.DefaultView;
			}
		}
    
		public virtual string DisplayExpression 
		{
			get 
			{
				return _sourcetable.DisplayExpression;
			}
			set 
			{
				_sourcetable.DisplayExpression = value;
			}
		}
    
		public virtual PropertyCollection ExtendedProperties 
		{
			get 
			{
				return _sourcetable.ExtendedProperties;
			}
		}
    
		public virtual bool HasErrors 
		{
			get 
			{
				return _sourcetable.HasErrors;
			}
		}
    
		public virtual System.Globalization.CultureInfo Locale 
		{
			get 
			{
				return _sourcetable.Locale;
			}
			set 
			{
				_sourcetable.Locale = value;
			}
		}
    
		public virtual int MinimumCapacity 
		{
			get 
			{
				return _sourcetable.MinimumCapacity;
			}
			set 
			{
				_sourcetable.MinimumCapacity = value;
			}
		}
    
		public virtual DataRelationCollection ParentRelations 
		{
			get 
			{
				return _sourcetable.ParentRelations;
			}
		}
    
		public virtual DataColumn[] PrimaryKey 
		{
			get 
			{
				return _sourcetable.PrimaryKey;
			}
			set 
			{
				_sourcetable.PrimaryKey = value;
			}
		}
    
		public virtual string TableName 
		{
			get 
			{
				return _sourcetable.TableName;
			}
			set 
			{
				_sourcetable.TableName = value;
			}
		}
    
		public virtual string Namespace 
		{
			get 
			{
				return _sourcetable.Namespace;
			}
			set 
			{
				_sourcetable.Namespace = value;
			}
		}
    
		public virtual string Prefix 
		{
			get 
			{
				return _sourcetable.Prefix;
			}
			set 
			{
				_sourcetable.Prefix = value;
			}
		}
    
		public override ISite Site 
		{
			get 
			{
				return _sourcetable.Site;
			}
			set 
			{
				_sourcetable.Site = value;
			}
		}
    
		public override IContainer Container 
		{
			get 
			{
				return _sourcetable.Container;
			}
		}
    
		public override bool DesignMode 
		{
			get 
			{
				return _sourcetable.DesignMode;
			}
		}
	#endregion    

	#region Common methods
		public virtual void EndInit() 
		{
			_sourcetable.EndInit();
		}
    
		public virtual void BeginInit() 
		{
			_sourcetable.BeginInit();
		}
    
		public virtual void Reset() 
		{
			_sourcetable.Reset();
		}
    
		public override object GetService(Type service) 
		{
			return _sourcetable.GetService(service);
		}
    
		public override int GetHashCode() 
		{
			return _sourcetable.GetHashCode();
		}
    
		public override bool Equals(object obj) 
		{
			return _sourcetable.Equals(obj);
		}
    
		public override string ToString() 
		{
			return _sourcetable.ToString();
		}
    
		public virtual void AcceptChanges() 
		{
			_sourcetable.AcceptChanges();
		}
    
		public virtual void Clear() 
		{
			_sourcetable.Clear();
		}
    
		public virtual object Compute(string expression, string filter) 
		{
			return _sourcetable.Compute(expression, filter);
		}
    
		public virtual DataTable GetChanges() 
		{
			return _sourcetable.GetChanges();
		}
    
		public virtual DataTable GetChanges(DataRowState rowStates) 
		{
			return _sourcetable.GetChanges(rowStates);
		}
    
		public virtual void RejectChanges() 
		{
			_sourcetable.RejectChanges();
		}    

		public virtual void BeginLoadData() 
		{
			_sourcetable.BeginLoadData();
		}
    
		public virtual void EndLoadData() 
		{
			_sourcetable.EndLoadData();
		}
    
	#endregion
	}
}