namespace NMatrix.XDL
{
	using System;
	using System.Data;
    using System.Xml;
	using System.Runtime.Serialization;
	using System.Collections;
	using System.ComponentModel;
	using NMatrix.XDL.Wrappers;
	using NMatrix.XDL.Interfaces;

	#region BaseDataSet
	/// <summary>
	///	Base class for the data access layer datasets.
	/// </summary>	
	[Serializable()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public class BaseDataSet : DataSetWrapper
	{
		//Cache already created tables.
		private Hashtable _datatables = new Hashtable();

		/// <summary>
		///	Internal constructor.
		/// </summary>	
		/// <param name="schemaFile"></param>
		protected internal BaseDataSet(string schemaFile) : base(schemaFile)
		{
		}

		/// <summary>
		///	Internal constructor.
		/// </summary>	
		/// <param name="schemaFile"></param>
		/// <param name="state"></param>
		protected internal BaseDataSet(string schemaFile, InternalDataSet state) : base(schemaFile, state)
		{
		}

		/// <summary>
		///	Check existence of the requested table and returns the correct type with a Prototype pattern.
		/// </summary>	
		/// <param name="tableType">The table to look for.</param>
		/// <param name="tableName">Tha table name.</param>
		protected BaseDataTable RetrieveDataTable(Type tableType, string tableName)
		{
			DataTable tb = this.Tables[tableName];
			if (tb == null) throw new NotLoadedException(String.Format("The table [{0}] hasn't been loaded in the current schema.", tableName));
			if (_datatables.ContainsKey(tableName))
			{
				return (BaseDataTable) _datatables[tableName];
			}
			else
			{
				BaseDataTable instance = (BaseDataTable)_factory.CreateIDataTable(tableType, tb);
				_datatables.Add(tableName, instance);
				return  instance;
			}
		}
	}
	#endregion

	#region BaseDataTable
	/// <summary>
	///	Base class for the data access layer datatables.
	/// </summary>	
	public class BaseDataTable : TableWrapper, IEnumerable
	{            
		/// <summary>
		///	Internal constructor.
		/// </summary>	
		/// <param name="table"></param>
		protected internal BaseDataTable(DataTable table) : base(table)
		{
			this.InitClass();
		}
		
		/// <summary>
		///	Returns the right datatype to build.
		/// </summary>	
		protected override Type GetRowType()
		{
			return typeof(BaseRow);
		}
        
		/// <summary>
		///	Looks for the specified column in the datatable.
		/// </summary>	
		/// <param name="columnName"></param>
		public DataColumn RetrieveDataColumn(string columnName)
		{
			DataColumn col = this.Columns[columnName]; 
			if (col == null) throw new NotLoadedException(String.Format("The field [{0}] hasn't been loaded in the current schema.", columnName));
			return col;
		}
            
		/// <summary>
		///	The number of rows in the datatable.
		/// </summary>	
		[Browsable(false)]
		public int Count 
		{
			get { return this.Rows.Count; }
		}
        
		/// <summary>
		///	<c>IEnumerable</c> implementation.
		/// </summary>	
		/// <param name="columnName"></param>
		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		/// <summary>
		///	Method to override in desdencent classes to customize initialization of the class.
		/// </summary>	
		protected virtual void InitClass() 
		{
		}
	}
	#endregion        

	#region BaseRow
	/// <summary>
	///	Base class for the data access layer data rows.
	/// </summary>	
	public class BaseRow : RowWrapper 
	{	
		/// <summary>
		///	Internal constructor.
		/// </summary>	
		/// <param name="row"></param>
		protected internal BaseRow(DataRow row) : base(row) 
		{
		}

		/// <summary>
		///	Internal constructor.
		/// </summary>	
		protected internal BaseRow()
		{
		}

		/// <summary>
		///	Returns the right datatype to build.
		/// </summary>	
		protected override Type GetRowType()
		{
			return typeof(BaseRow);
		}

		/// <summary>
		///	Returns the right datatype to build.
		/// </summary>	
		protected override Type GetTableType()
		{
			return typeof(BaseDataTable);
		}

		/// <summary>
		///	Returns the current value for the specified column.
		/// </summary>	
		/// <param name="columnName"></param>
		protected object GetValue(string columnName)
		{
			return this[((BaseDataTable)this.Table).RetrieveDataColumn(columnName)];
		}

		/// <summary>
		///	Sets the value for a field.
		/// </summary>	
		/// <param name="columnName"></param>
		/// <param name="value"></param>
		protected void SetValue(string columnName, object value)
		{
			this[((BaseDataTable)this.Table).RetrieveDataColumn(columnName)] = value;
		}

		/// <summary>
		///	Checks if the column is null.
		/// </summary>	
		/// <param name="columnName"></param>
		protected bool IsNullColumn(string columnName)
		{
			return this.IsNull(((BaseDataTable)this.Table).RetrieveDataColumn(columnName));
		}

		/// <summary>
		///	Sets the column to null.
		/// </summary>	
		/// <param name="columnName"></param>
		protected void SetNullColumn(string columnName)
		{
			this[((BaseDataTable)this.Table).RetrieveDataColumn(columnName)] = System.Convert.DBNull;
		}                                    
	}
	#endregion
}
